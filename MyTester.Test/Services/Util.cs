namespace MyTester.Test.Services
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using OpenQA.Selenium;
    using System.Threading;
    public class Util
    {
        // usage: Util.NavigatorTo<?>(driver)
        public static T NavigatorTo<T>(IWebDriver driver) where T : class => (T)Activator.CreateInstance(typeof(T), driver);

        public static void Snapshot(IWebDriver driver, string fileName = null)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            String fullName = $"{DateTime.Now.ToString("yyyyMMddHHmmssss")}{fileName}";
            string path = $@"{Directory.GetParent("../../../")}/TestResults/{fullName}.png";
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(path, ScreenshotImageFormat.Png);
        }

        public static bool IfExistDeleteIt(string file, bool deleteIt = false)
        {
            string UserProfile = System.Environment.GetEnvironmentVariable("USERPROFILE");
            string DownloadFolder = Path.Combine(UserProfile, "Downloads");
            bool result = false;

            string fileFullName = Path.Combine(DownloadFolder, file);
            Debug.WriteLine($"fileFullName: {fileFullName}");

            try
            {
                if (File.Exists(fileFullName))
                {
                    if (deleteIt)
                    {
                        File.Delete(fileFullName);
                    }
                    result = true;
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"Util::IfExistDeleteIt {ex}");
            }

            return result;
        }
    }
}