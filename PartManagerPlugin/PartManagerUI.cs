using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using CKAN;
using CKAN.GUI;
using Newtonsoft.Json;

namespace PartManagerPlugin
{

    public enum FilterType
    {
        Path,
        Name,
        Title
    }

    public partial class PartManagerUI : UserControl
    {

        private Dictionary<string, ConfigNode> m_DisabledParts = new Dictionary<string, ConfigNode>();

        private readonly string ConfigPath = "PartManager/PartManager.json";

        private string m_Filter = null;
        private bool m_FilterRegex = false;
        private FilterType m_FilterType;

        private RepositoryDataManager m_RepoData;
        private Registry m_Registry;
        private Dictionary<string, List<string>> m_AllCraftParts = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> m_MissingParts = new Dictionary<string, List<string>>();
        private Dictionary<string, string> m_CraftFilePaths = new Dictionary<string, string>();
        private bool m_ScanInProgress = false;

        private Registry GetRegistry()
        {
            if (m_RepoData == null)
            {
                m_RepoData = ServiceLocator.Container.Resolve<RepositoryDataManager>();
            }
            if (Main.Instance?.CurrentInstance != null)
            {
                m_Registry = RegistryManager.Instance(Main.Instance.CurrentInstance, m_RepoData).registry;
            }
            return m_Registry;
        }

        private void LoadConfig()
        {
            var ckanDir = Main.Instance?.CurrentInstance?.CkanDir;
            if (ckanDir == null) return;

            var fullPath = Path.Combine(ckanDir, ConfigPath);
            if (!File.Exists(fullPath))
            {
                return;
            }

            var partManagerPath = Path.Combine(ckanDir, "PartManager");
            if (!Directory.Exists(partManagerPath))
            {
                Directory.CreateDirectory(partManagerPath);
            }

            var cachePath = Path.Combine(partManagerPath, "cache");
            if (!Directory.Exists(cachePath))
            {
                Directory.CreateDirectory(cachePath);
            }

            var json = File.ReadAllText(fullPath);
            PartManagerConfig config = (PartManagerConfig) JsonConvert.DeserializeObject<PartManagerConfig>(json);
            if (config?.disabledParts != null)
            {
                foreach (var item in config.disabledParts)
                {
                    var configNode = ConfigNodeReader.FileToConfigNode(Path.Combine(cachePath, item.Key));
                    if (configNode != null)
                    {
                        m_DisabledParts.Add(item.Key, configNode);
                    }
                }
            }
        }

        private void SaveConfig()
        {
            var ckanDir = Main.Instance?.CurrentInstance?.CkanDir;
            if (ckanDir == null) return;

            var fullPath = Path.Combine(ckanDir, ConfigPath);
            var directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            PartManagerConfig config = new PartManagerConfig();
            config.disabledParts = new List<KeyValuePair<string, string>>();
            foreach (var part in m_DisabledParts)
            {
                config.disabledParts.Add(new KeyValuePair<string, string>(part.Key, null));
            }

            var json = JsonConvert.SerializeObject(config);
            File.WriteAllText(fullPath, json);
        }

        public PartManagerUI()
        {
            InitializeComponent();
        }

        private void PartManagerUI_Load(object sender, EventArgs e)
        {
            LoadConfig();
            RefreshInstalledModsList();
            UpdateStatsLabel();
        }

        public void OnModChanged()
        {
            // Re-apply disabled parts: if a part .cfg was restored (e.g. after mod update),
            // move it back to the cache to keep it disabled
            var gameDir = Main.Instance?.CurrentInstance?.GameDir;
            if (gameDir == null) return;

            foreach (var disabledPart in m_DisabledParts)
            {
                var fullPath = Path.Combine(gameDir, disabledPart.Key);
                if (File.Exists(fullPath))
                {
                    Cache.RemovePartFromCache(disabledPart.Key);
                    Cache.MovePartToCache(disabledPart.Key);
                }
            }

            RefreshInstalledModsList();
        }

        private void RefreshInstalledModsList()
        {
            var registry = GetRegistry();
            if (registry == null) return;

            var installedMods = registry.InstalledModules;

            InstalledModsListBox.Items.Clear();

            foreach (var mod in installedMods)
            {
                var parts = GetInstalledModParts(mod.identifier);
                if (parts != null && parts.Any())
                {
                    InstalledModsListBox.Items.Add(String.Format("{0} | {1}", mod.identifier, mod.Module.version));
                }
            }
        }

        private Dictionary<string, ConfigNode> GetInstalledModParts(string identifier)
        {
            var registry = GetRegistry();
            if (registry == null) return null;

            var module = registry.InstalledModule(identifier);

            if (module == null)
            {
                return null;
            }

            Dictionary<string, ConfigNode> parts = new Dictionary<string, ConfigNode>();

            foreach (var item in module.Files)
            {
                if (m_DisabledParts.ContainsKey(item))
                {
                    parts.Add(item, m_DisabledParts[item]);
                    continue;
                }

                var filename = Path.GetFileName(item);

                if (filename.EndsWith(".cfg", StringComparison.OrdinalIgnoreCase))
                {
                    var configNode = LoadPart(item);
                    if (configNode != null)
                    {
                        parts.Add(item, configNode);
                    }
                }
            }

            return parts;
        }

        private ConfigNode LoadPart(string part)
        {
            var gameDir = Main.Instance?.CurrentInstance?.GameDir;
            var ckanDir = Main.Instance?.CurrentInstance?.CkanDir;
            if (gameDir == null || ckanDir == null) return null;

            var fullPath = Path.Combine(gameDir, part);
            if (!File.Exists(fullPath))
            {
                var cachePath = Path.Combine(ckanDir, "PartManager", "cache");
                fullPath = Path.Combine(cachePath, part);
                if (!File.Exists(fullPath))
                {
                    return null;
                }
            }

            var configNode = ConfigNodeReader.FileToConfigNode(fullPath);
            return configNode;
        }

        private void InstalledModsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InstalledModsListBox.SelectedItems == null || InstalledModsListBox.SelectedItems.Count == 0)
            {
                return;
            }

            PartsGridView.Rows.Clear();

            foreach (var selectedItem in InstalledModsListBox.SelectedItems)
            {
                var item = (selectedItem as string).Split('|');
                var identifier = item[0].Trim();

                var parts = GetInstalledModParts(identifier);

                foreach (var part in parts)
                {
                    if (m_FilterType == FilterType.Path && !FilterString(part.Key))
                    {
                        continue;
                    }

                    var row = new DataGridViewRow();
                    row.Tag = part;

                    var enabledCheckbox = new DataGridViewCheckBoxCell();
                    enabledCheckbox.Value = !m_DisabledParts.ContainsKey(part.Key);
                    row.Cells.Add(enabledCheckbox);

                    var titleTextbox = new DataGridViewTextBoxCell();
                    var title = part.Value.GetValue("title");

                    if (m_FilterType == FilterType.Title && !FilterString(title))
                    {
                        continue;
                    }

                    titleTextbox.Value = title;

                    row.Cells.Add(titleTextbox);

                    var nameTextbox = new DataGridViewTextBoxCell();
                    nameTextbox.Value = part.Value.GetValue("name");
                    row.Cells.Add(nameTextbox);

                    if (m_FilterType == FilterType.Name && !FilterString(part.Value.GetValue("name")))
                    {
                        continue;
                    }

                    var pathTextbox = new DataGridViewTextBoxCell();
                    pathTextbox.Value = part.Key;
                    row.Cells.Add(pathTextbox);

                    PartsGridView.Rows.Add(row);
                }
            }
            UpdateStatsLabel();
        }

        private void PartsGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex < 0)
            {
                return;
            }

            var grid = sender as DataGridView;
            var row = grid.Rows[e.RowIndex];
            var columnIndex = e.ColumnIndex;

            if (columnIndex != 0)
            {
                return;
            }

            var part = (KeyValuePair<string, ConfigNode>) row.Tag;

            var gridViewCell = row.Cells[columnIndex] as DataGridViewCheckBoxCell;
            var state = (bool)gridViewCell.Value;
            if (state == false)
            {
                if (m_DisabledParts.ContainsKey(part.Key))
                {
                    return;
                }

                m_DisabledParts.Add(part.Key, part.Value);
                Cache.MovePartToCache(part.Key);
                SaveConfig();
                UpdateStatsLabel();
            }
            else
            {
                if (!m_DisabledParts.ContainsKey(part.Key))
                {
                    return;
                }

                m_DisabledParts.Remove(part.Key);
                Cache.MovePartFromCache(part.Key);
                SaveConfig();
                UpdateStatsLabel();
            }
        }

        private bool FilterString(string value)
        {
            if (m_Filter == null)
            {
                return true;
            }

            if (value == null)
            {
                return false;
            }

            if (m_Filter.Length == 0)
            {
                return true;
            }

            if (m_FilterRegex)
            {
                try
                {
                    return Regex.IsMatch(value, m_Filter);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return value.ToLower().Contains(m_Filter.ToLower());
            }
        }

        private void ApplyFilterButton_Click(object sender, EventArgs e)
        {
            m_Filter = FilterTextBox.Text;
            m_FilterRegex = RegexCheckbox.Checked;
            ClearFilterbutton.Enabled = true;

            try
            {
                m_FilterType = (FilterType)Enum.Parse(typeof(FilterType), FilterTypeCombobox.Text, true);
            }
            catch (Exception)
            {
                FilterTypeCombobox.Text = "Path";
                m_FilterType = FilterType.Path;                
            }
            InstalledModsListBox_SelectedIndexChanged(null, new EventArgs());
        }

        private void ClearFilterbutton_Click(object sender, EventArgs e)
        {
            ClearFilterbutton.Enabled = false;
            m_Filter = null;
            InstalledModsListBox_SelectedIndexChanged(null, new EventArgs());
        }

        private void UpdateStatsLabel()
        {
            var total = PartsGridView.Rows.Count;
            var disabled = m_DisabledParts.Count;
            StatsLabel.Text = $"Parts: {total} total, {disabled} disabled, {total - disabled} enabled";
        }

        private void EnableAllButton_Click(object sender, EventArgs e)
        {
            var changed = false;
            foreach (DataGridViewRow row in PartsGridView.Rows)
            {
                var part = (KeyValuePair<string, ConfigNode>)row.Tag;
                if (!m_DisabledParts.ContainsKey(part.Key))
                    continue;

                m_DisabledParts.Remove(part.Key);
                Cache.MovePartFromCache(part.Key);
                (row.Cells[0] as DataGridViewCheckBoxCell).Value = true;
                changed = true;
            }
            if (changed)
            {
                SaveConfig();
                UpdateStatsLabel();
            }
        }

        private async void ScanShipsButton_Click(object sender, EventArgs e)
        {
            if (m_ScanInProgress) return;

            var gameDir = Main.Instance?.CurrentInstance?.GameDir;
            if (gameDir == null)
            {
                CraftStatusLabel.Text = "Error: No game instance loaded";
                return;
            }

            CraftStatusLabel.Text = "Scanning craft files...";
            ScanShipsButton.Enabled = false;
            ScanSelectedButton.Enabled = false;
            m_ScanInProgress = true;

            try
            {
                // Run heavy scanning on background thread to prevent UI hang
                var result = await Task.Run(() =>
                {
                    // Clear cache so we get fresh GameData scan
                    PartScanner.ClearCache();

                    var allParts = PartScanner.ScanAllCraftFiles(gameDir, out var filePaths);
                    var missing = PartScanner.FindMissingParts(gameDir, allParts);
                    return new { allParts, missing, filePaths };
                });

                m_AllCraftParts = result.allParts;
                m_MissingParts = result.missing;
                m_CraftFilePaths = result.filePaths;

                // Update UI on main thread
                PopulateCraftFilesList();
                PopulateMissingPartsList();

                var totalCraft = m_AllCraftParts.Count;
                var totalMissing = m_MissingParts.Sum(kvp => kvp.Value.Count);

                if (totalCraft == 0)
                {
                    CraftStatusLabel.Text = "No .craft files found in ships/VAB/ or ships/SPH/. Build some ships in KSP first!";
                }
                else if (totalMissing == 0)
                {
                    CraftStatusLabel.Text = $"All parts found! Scanned {totalCraft} craft files, no missing parts detected.";
                }
                else
                {
                    CraftStatusLabel.Text = $"Scanned {totalCraft} craft files, found {totalMissing} missing parts across {m_MissingParts.Count} craft files.";
                }
            }
            catch (Exception ex)
            {
                CraftStatusLabel.Text = $"Error: {ex.Message}";
            }
            finally
            {
                ScanShipsButton.Enabled = true;
                ScanSelectedButton.Enabled = true;
                m_ScanInProgress = false;
            }
        }

        /// <summary>
        /// Populates the craft files listbox with all found .craft files.
        /// Shows missing part count next to each craft name.
        /// </summary>
        private void PopulateCraftFilesList()
        {
            CraftFilesListBox.Items.Clear();

            if (m_AllCraftParts.Count == 0)
            {
                CraftFilesListBox.Items.Add("(No .craft files found — go build some ships!)");
                return;
            }

            foreach (var kvp in m_AllCraftParts)
            {
                var craftName = kvp.Key;
                var partCount = kvp.Value.Count;
                var missingCount = m_MissingParts.ContainsKey(craftName) ? m_MissingParts[craftName].Count : 0;

                if (missingCount > 0)
                {
                    CraftFilesListBox.Items.Add($"{craftName} ({partCount} parts, {missingCount} missing)");
                }
                else
                {
                    CraftFilesListBox.Items.Add($"{craftName} ({partCount} parts, OK)");
                }
            }
        }

        /// <summary>
        /// Populates the missing parts listbox based on currently selected craft files.
        /// </summary>
        private void PopulateMissingPartsList()
        {
            MissingPartsListBox.Items.Clear();

            if (m_MissingParts.Count == 0)
            {
                MissingPartsListBox.Items.Add("(No missing parts — scan craft files first)");
                return;
            }

            // If craft files are selected, only show missing parts for those
            if (CraftFilesListBox.SelectedItems.Count > 0)
            {
                foreach (var selItem in CraftFilesListBox.SelectedItems)
                {
                    var craftName = selItem.ToString().Split('(')[0].Trim();
                    if (m_MissingParts.TryGetValue(craftName, out var parts))
                    {
                        foreach (var part in parts)
                        {
                            MissingPartsListBox.Items.Add($"[{craftName}] {part}");
                        }
                    }
                }
            }
            else
            {
                // No selection: show all missing parts
                foreach (var kvp in m_MissingParts)
                {
                    foreach (var part in kvp.Value)
                    {
                        MissingPartsListBox.Items.Add($"[{kvp.Key}] {part}");
                    }
                }
            }

            if (MissingPartsListBox.Items.Count == 0)
            {
                MissingPartsListBox.Items.Add("(No missing parts for selected craft)");
            }
            CraftStatusLabel.Text = $"{MissingPartsListBox.Items.Count} missing part(s) shown";
        }

        private async void ScanSelectedButton_Click(object sender, EventArgs e)
        {
            if (m_ScanInProgress) return;
            if (CraftFilesListBox.SelectedItems.Count == 0)
            {
                CraftStatusLabel.Text = "Select one or more craft files from the list to scan";
                return;
            }

            var gameDir = Main.Instance?.CurrentInstance?.GameDir;
            if (gameDir == null)
            {
                CraftStatusLabel.Text = "Error: No game instance loaded";
                return;
            }

            ScanSelectedButton.Enabled = false;
            ScanShipsButton.Enabled = false;
            CraftStatusLabel.Text = "Scanning selected craft files...";
            m_ScanInProgress = true;

            try
            {
                // Build list of craft names to scan
                var selectedNames = new List<string>();
                foreach (var selItem in CraftFilesListBox.SelectedItems)
                {
                    var craftName = selItem.ToString().Split('(')[0].Trim();
                    selectedNames.Add(craftName);
                }

                // Run on background thread
                var result = await Task.Run(() =>
                {
                    PartScanner.ClearCache();
                    var missingDict = new Dictionary<string, List<string>>();

                    foreach (var craftName in selectedNames)
                    {
                        if (m_AllCraftParts.TryGetValue(craftName, out var parts))
                        {
                            var missing = PartScanner.FindMissingPartsForCraft(gameDir, craftName, parts);
                            if (missing.Count > 0)
                            {
                                missingDict[craftName] = missing;
                            }
                        }
                    }

                    return missingDict;
                });

                // Update missing parts dict (keep only what was scanned + preserve unscanned)
                // Actually, just update the entries for selected crafts
                foreach (var kvp in result)
                {
                    m_MissingParts[kvp.Key] = kvp.Value;
                }
                // Remove missing entries for selected crafts that no longer have missing parts
                foreach (var craftName in selectedNames)
                {
                    if (!result.ContainsKey(craftName) && m_MissingParts.ContainsKey(craftName))
                    {
                        m_MissingParts.Remove(craftName);
                    }
                }

                PopulateCraftFilesList();
                PopulateMissingPartsList();

                var totalMissing = m_MissingParts.Sum(kvp => kvp.Value.Count);
                if (totalMissing == 0)
                {
                    CraftStatusLabel.Text = $"All parts found for selected craft files!";
                }
                else
                {
                    CraftStatusLabel.Text = $"Found {totalMissing} missing parts across selected craft files.";
                }
            }
            catch (Exception ex)
            {
                CraftStatusLabel.Text = $"Error: {ex.Message}";
            }
            finally
            {
                ScanSelectedButton.Enabled = true;
                ScanShipsButton.Enabled = true;
                m_ScanInProgress = false;
            }
        }

        private void CraftFilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateMissingPartsList();
        }

        private List<string> GetSelectedMissingParts()
        {
            var parts = new List<string>();
            foreach (var item in MissingPartsListBox.SelectedItems)
            {
                var text = item as string;
                if (text != null)
                {
                    // Format: "[CraftName] partName"
                    var idx = text.IndexOf("] ");
                    if (idx >= 0)
                    {
                        parts.Add(text.Substring(idx + 2));
                    }
                    else
                    {
                        parts.Add(text);
                    }
                }
            }
            return parts;
        }

        private void LookupCkanButton_Click(object sender, EventArgs e)
        {
            var parts = GetSelectedMissingParts();
            if (parts.Count == 0)
            {
                CraftStatusLabel.Text = "Select a missing part first to look up on CKAN";
                return;
            }
            foreach (var part in parts)
            {
                OpenUrl($"https://github.com/KSP-CKAN/CKAN/issues?q=is%3Aissue+{Uri.EscapeDataString(part)}");
            }
        }

        private void LookupSpacedockButton_Click(object sender, EventArgs e)
        {
            var parts = GetSelectedMissingParts();
            if (parts.Count == 0)
            {
                CraftStatusLabel.Text = "Select a missing part first to look up on Spacedock";
                return;
            }
            foreach (var part in parts)
            {
                OpenUrl($"https://spacedock.info/search?q={Uri.EscapeDataString(part)}");
            }
        }

        private void LookupGithubButton_Click(object sender, EventArgs e)
        {
            var parts = GetSelectedMissingParts();
            if (parts.Count == 0)
            {
                CraftStatusLabel.Text = "Select a missing part first to look up on GitHub";
                return;
            }
            foreach (var part in parts)
            {
                OpenUrl($"https://github.com/search?q={Uri.EscapeDataString(part)}+ksp+mod&type=repositories");
            }
        }

        private void LookupKerbalxButton_Click(object sender, EventArgs e)
        {
            var parts = GetSelectedMissingParts();
            if (parts.Count == 0)
            {
                CraftStatusLabel.Text = "Select a missing part first to look up on KerbalX";
                return;
            }
            foreach (var part in parts)
            {
                OpenUrl($"https://kerbalx.com/craft?search={Uri.EscapeDataString(part)}");
            }
        }

        private static void OpenUrl(string url)
        {
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to open URL {url}: {ex.Message}");
            }
        }

        private void DisableAllButton_Click(object sender, EventArgs e)
        {
            var changed = false;
            foreach (DataGridViewRow row in PartsGridView.Rows)
            {
                var part = (KeyValuePair<string, ConfigNode>)row.Tag;
                if (m_DisabledParts.ContainsKey(part.Key))
                    continue;

                m_DisabledParts.Add(part.Key, part.Value);
                Cache.MovePartToCache(part.Key);
                (row.Cells[0] as DataGridViewCheckBoxCell).Value = false;
                changed = true;
            }
            if (changed)
            {
                SaveConfig();
                UpdateStatsLabel();
            }
        }

    }
}
