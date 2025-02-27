﻿namespace Zenex
{
    public static class ErrorLog
    {
        public static void WriteToFile(string ControllerAction, Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFiles");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                DateTime dt = DateTime.Today;
                DateTime ystrdy = DateTime.Today.AddDays(-15);//keep 15 days backup
                string yday = ystrdy.ToString("yyyyMMdd");
                string today = dt.ToString("yyyyMMdd");
                string Log = today + ".txt";
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\LogFiles\\Log_" + yday + ".txt"))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\LogFiles\\Log_" + yday + ".txt");
                }
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFiles\\Log_" + Log, true);
                sw.WriteLine($"{DateTime.Now.ToString()} : {ControllerAction} :- {ex.Message}");
                if (ex.Message.Contains("inner exception") && ex.InnerException != null)
                {
                    sw.WriteLine($"{DateTime.Now.ToString()} : {ControllerAction} Inner :- {ex.InnerException.Message}");
                }
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }

        }

        public static void WriteToFile(string Message)
        {
            StreamWriter sw = null;
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFiles");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                DateTime dt = DateTime.Today;
                DateTime ystrdy = DateTime.Today.AddDays(-15);//keep 15 days backup
                string yday = ystrdy.ToString("yyyyMMdd");
                string today = dt.ToString("yyyyMMdd");
                string Log = today + ".txt";
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\LogFiles\\Log_" + yday + ".txt"))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\LogFiles\\Log_" + yday + ".txt");
                }
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFiles\\Log_" + Log, true);
                sw.WriteLine($"{DateTime.Now.ToString()} : {Message}");
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }

        }

        //public static void WriteProcessLog(string Message)
        //{
        //    StreamWriter sw = null;
        //    try
        //    {
        //        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProcessFiles");
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        DateTime dt = DateTime.Today;
        //        DateTime ystrdy = DateTime.Today.AddDays(-60);
        //        string yday = ystrdy.ToString("yyyyMMdd");
        //        string today = dt.ToString("yyyyMMdd");
        //        string Log = today + ".txt";
        //        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\ProcessFiles\\Log_" + yday + ".txt"))
        //        {
        //            System.GC.Collect();
        //            System.GC.WaitForPendingFinalizers();
        //            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\ProcessFiles\\Log_" + yday + ".txt");
        //        }
        //        sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ProcessFiles\\Log_" + Log, true);
        //        sw.WriteLine(string.Format(DateTime.Now.ToString()) + ":" + Message);
        //        sw.Flush();
        //        sw.Close();
        //    }
        //    catch
        //    {

        //    }
        //}

    }
}
