using System.Collections.Generic;
using System.Windows.Forms;
using CKAN;
using CKAN.GUI;
using CKAN.Versioning;

namespace PartManagerPlugin
{

    public class PartManagerConfig
    {
        public List<KeyValuePair<string, string>> disabledParts;
    }

    public class PartManagerPlugin : IGUIPlugin
    {

        private readonly ModuleVersion VERSION = new ModuleVersion("v2.0.0");

        private PartManagerUI m_UI = null;
        private TabPage m_TabPage = null;

        public override void Initialize()
        {
            m_TabPage = new TabPage();
            m_TabPage.Name = "PartManager";
            m_TabPage.Text = "PartManager";

            m_UI = new PartManagerUI();
            m_UI.Dock = DockStyle.Fill;
            m_TabPage.Controls.Add(m_UI);

            // Subscribe to registry changes (fires after install/update/remove)
            if (Main.Instance?.ManageMods != null)
            {
                Main.Instance.ManageMods.OnRegistryChanged += m_UI.OnModChanged;
            }

            // Find the main TabControl and add our tab page
            var tabControl = FindMainTabControl();
            if (tabControl != null)
            {
                tabControl.TabPages.Add(m_TabPage);
                tabControl.SelectedTab = m_TabPage;
            }
        }

        public override void Deinitialize()
        {
            if (Main.Instance?.ManageMods != null)
            {
                Main.Instance.ManageMods.OnRegistryChanged -= m_UI.OnModChanged;
            }

            var tabControl = FindMainTabControl();
            if (tabControl != null && m_TabPage != null)
            {
                tabControl.TabPages.Remove(m_TabPage);
            }
        }

        public override string GetName()
        {
            return "PartManager by nlight";
        }

        public override ModuleVersion GetVersion()
        {
            return VERSION;
        }

        /// <summary>
        /// Finds the main TabControl in the CKAN GUI form hierarchy.
        /// First tries to find it by name, then falls back to type-based search.
        /// </summary>
        private static TabControl FindMainTabControl()
        {
            if (Main.Instance == null)
                return null;

            // Try to find by name first (more reliable)
            var byName = FindControlByName(Main.Instance, "MainTabControl") as TabControl;
            if (byName != null)
                return byName;

            // Fall back to type-based search
            return FindControlByType<TabControl>(Main.Instance);
        }

        /// <summary>
        /// Recursively searches for a control with the given name in the control tree.
        /// </summary>
        private static Control FindControlByName(Control parent, string name)
        {
            if (parent.Name == name)
                return parent;

            foreach (Control c in parent.Controls)
            {
                if (c.Name == name)
                    return c;
                if (c.Controls.Count > 0)
                {
                    var result = FindControlByName(c, name);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Recursively searches for a control of type T in the control tree.
        /// </summary>
        private static T FindControlByType<T>(Control parent) where T : Control
        {
            foreach (Control c in parent.Controls)
            {
                if (c is T t)
                    return t;
                if (c.Controls.Count > 0)
                {
                    var result = FindControlByType<T>(c);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

    }

}
