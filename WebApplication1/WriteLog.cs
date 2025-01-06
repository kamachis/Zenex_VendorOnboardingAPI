using Microsoft.AspNetCore.Builder;
using System.IO;
using Zenex.Migrations;

namespace Zenex
{
    public class WriteLog
    {
        public static void WriteToFile(string ControllerAction, Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                string path = @"C:\\Exalca\\DotNet\\Projects\\Zenex\\WebApplication1\\bin";
                string folderpath = Path.Combine(path, "LogFiles");
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }
                DateTime dt = DateTime.Today;
                DateTime ystrdy = DateTime.Today.AddDays(-15);//keep 15 days backup
                string yday = ystrdy.ToString("yyyyMMdd");
                string today = dt.ToString("yyyyMMdd");
                string Log = today + ".txt";
                if (File.Exists(folderpath + "\\LogFiles\\Log_" + yday + ".txt"))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    File.Delete(folderpath + "\\LogFiles\\Log_" + yday + ".txt");
                }
                sw = new StreamWriter(folderpath + "\\LogFiles\\Log_" + Log, true);
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

        public static void WriteToFile(string ControllerAction, string text)
        {
            StreamWriter sw = null;
            try
            {
                string path = @"C:\\inetpub\\wwwroot\\VendorOnBoarding\\Service\\LogFiles";
                //string path = Path.Combine(folderpath, "LogHistoryFiles");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                DateTime dt = DateTime.Today;
                DateTime ystrdy = DateTime.Today.AddDays(-15);//keep 15 days backup
                string yday = ystrdy.ToString("yyyyMMdd");
                string today = dt.ToString("yyyyMMdd");
                string Log = today + ".txt";
                if (File.Exists(path + "\\Log_" + yday + ".txt"))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    File.Delete(path + "\\Log_" + yday + ".txt");
                }
                sw = new StreamWriter(path + "\\Log_" + Log, true);
                sw.WriteLine($"{DateTime.Now.ToString()} : {ControllerAction} :- {text}");
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
                //string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                ////string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogHistoryFiles");
                //DirectoryInfo baseDirInfo = new DirectoryInfo(baseDirectory);
                //DirectoryInfo parentDirInfo = baseDirInfo.Parent;

                //if (parentDirInfo != null)
                //{
                //string newBaseDirectory = parentDirInfo.FullName;
                string newBaseDirectory = @"C:\\Exalca\\DotNet\\Projects\\Zenex\\WebApplication1\\bin";
                string path = Path.Combine(newBaseDirectory, "LogHistoryFiles");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    DateTime dt = DateTime.Today;
                    DateTime ystrdy = DateTime.Today.AddDays(-15);//keep 15 days backup
                    string yday = ystrdy.ToString("yyyyMMdd");
                    string today = dt.ToString("yyyyMMdd");
                    string Log = today + ".txt";
                    if (File.Exists(newBaseDirectory + "\\LogHistoryFiles\\Log_" + yday + ".txt"))
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        File.Delete(newBaseDirectory + "\\LogHistoryFiles\\Log_" + yday + ".txt");
                    }
                    sw = new StreamWriter(newBaseDirectory + "\\LogHistoryFiles\\Log_" + Log, true);
                    sw.WriteLine($"{DateTime.Now.ToString()} :- {Message}");
                    sw.Flush();
                    sw.Close();
                    //Console.WriteLine("New base directory: " + newBaseDirectory);
                //}
                //else
                //{
                //    Console.WriteLine("net6.0 folder is already the root directory.");
                //}

                
            }
            catch
            {

            }

        }
    }
}
