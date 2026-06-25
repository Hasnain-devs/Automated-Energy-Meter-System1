using System;
using System.IO;
using System.Text;

namespace IOTController
{
    static class Logger
    {
        private static readonly object LockObj = new object();
        private static readonly string LogPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IOTController.log");

        public static void Error(string message, Exception ex)
        {
            Write("ERROR", message + " :: " + ex.Message + Environment.NewLine + ex.StackTrace);
        }

        public static void Info(string message)
        {
            Write("INFO", message);
        }

        private static void Write(string level, string message)
        {
            lock (LockObj)
            {
                var line = string.Format("{0:u} [{1}] {2}{3}", DateTime.UtcNow, level, message, Environment.NewLine);
                File.AppendAllText(LogPath, line, Encoding.UTF8);
            }
        }
    }
}
