using System;
using System.Threading;
using System.Windows.Forms;

namespace IOTController
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Application.Run(new LoginScreen());
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Logger.Error("Unhandled UI thread exception", e.Exception);
            MessageBox.Show("Unexpected error: " + e.Exception.Message + "\n\nCheck IOTController.log in the EXE folder.", "IOT Controller", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception ?? new Exception("Unknown unhandled exception");
            Logger.Error("Unhandled app-domain exception", ex);
        }
    }
}
