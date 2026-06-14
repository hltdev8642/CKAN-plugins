using System;
using System.Reflection;
using System.Windows.Forms;

class Test {
    static void Main() {
        try {
            Assembly.LoadFrom(@"C:\Users\jared\Documents\_KSP\_software\CKAN\ckan.exe");
            var pluginAsm = Assembly.LoadFrom(@"C:\Users\jared\Documents\_KSP\_dev\CKAN-plugins\PartManagerPlugin\bin\DebugNew\PartManagerPlugin.dll");
            var type = pluginAsm.GetType("PartManagerPlugin.PartManagerPlugin");
            var instance = (dynamic)Activator.CreateInstance(type);
            Console.WriteLine("Instance created: " + (instance != null));
            Console.WriteLine("Name: " + instance.GetName());
            Console.WriteLine("Version: " + instance.GetVersion());
            Console.WriteLine("Calling Initialize...");
            instance.Initialize();
            Console.WriteLine("SUCCESS");
        } catch (Exception ex) {
            Console.WriteLine("ERROR: " + ex.GetType().FullName);
            Console.WriteLine("Message: " + ex.Message);
            Console.WriteLine("Stack: " + ex.StackTrace);
            if (ex.InnerException != null) {
                Console.WriteLine("Inner: " + ex.InnerException.GetType().FullName);
                Console.WriteLine("InnerMsg: " + ex.InnerException.Message);
                Console.WriteLine("InnerStack: " + ex.InnerException.StackTrace);
            }
        }
    }
}
