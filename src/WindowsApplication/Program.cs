using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;

namespace WindowsApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException +=
            (sender, args) => HandleUnhandledException(args.ExceptionObject as Exception);
            Application.ThreadException +=
                (sender, args) => HandleUnhandledException(args.Exception);

            //


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var mainForm = new MainForm();
            mainForm.Show();
            //mainForm.TabFormControl.Pages.Clear();
            mainForm.OpenMainMenu();
            Application.Run();
        }

        static void HandleUnhandledException(Exception e)
        {
            MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // show report sender and close the app or whatever
        }
    }
}
