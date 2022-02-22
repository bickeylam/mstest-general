namespace MyTester.Test.Pages
{
    using System;
    using System.IO;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public abstract class PageBase
    {
        protected readonly IWebDriver driver;

        public PageBase(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void WaitFor(int second)
        {
            System.Threading.Thread.Sleep(second * 1000);
        }

        public void WaitFor(By by, int second = 10)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(second))
                .Until(d => d.FindElement(by));
        }

        protected void Snapshot(string fileName = null)
        {
            WaitFor(1);
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = DateTime.Now.ToString("yyyyMMddHHmmssss");
            }
            string path = $@"{Directory.GetParent("../../../")}/TestResults/{fileName}.png";
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(path, ScreenshotImageFormat.Png);
        }

        protected void SelectByText(IWebElement element, string option) {
            new SelectElement(element).SelectByText(option);
        }

        protected void ClearAndSendKeys(IWebElement element, string value) {
            element.Clear();
            element.SendKeys(value);
        }
    }
}