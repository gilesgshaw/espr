using System;
using System.Reflection;
using System.Windows.Forms;

namespace intp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain() { Text = ((AssemblyProductAttribute)AssemblyProductAttribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute))).Product });
        }
    }
}
