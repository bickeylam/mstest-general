namespace MyTester.Test
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyTester.Test.Pages;
    using MyTester.Test.Services;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Edge;

    [TestClass]
    public class BrowserBasedTest
    {
        private readonly string BaseUrl = @"https://www.google.com.au/";
        private readonly TimeSpan DefaultWait = TimeSpan.FromSeconds(30);
        private readonly string DriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private IWebDriver driver;
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void BeforeAll(TestContext context) {
        }

        [ClassCleanup]
        public static void AfterAll() {
        }

        [TestInitialize]
        public void BeforeTest()
        {
            driver = LoadWebDriver(browser: "edge");
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Timeouts().ImplicitWait = DefaultWait;
            driver.Manage().Timeouts().PageLoad = DefaultWait;
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(BaseUrl);
        }

        [TestCleanup]
        public void AfterTest()
        {
            driver?.Manage().Cookies.DeleteAllCookies();
            driver?.Close();  // close active browser tab
            driver?.Quit();  // close all browsers
        }

        [DataTestMethod, Priority(1), TestCategory("SelfTest")]
        [DataRow("selenium")]
        // [DataRow("loadrunner")]
        public void WhenSearchTermEnteredThenResultPageLoaded(string term)
        {
            var page = Util.NavigatorTo<GooglePage>(driver);
            page.EnterSearchTerm(term);
            page.Submit();

            String title = $"driver.Title:{driver.Title}";
            TestContext.WriteLine(title);
            // Trace.WriteLine(title);
            // Debug.WriteLine(title);
            // Console.WriteLine(title);
            Assert.IsTrue(driver.Title.Contains(page.SearchResultTitle));
            Assert.AreEqual<string>(page.SearchResultTitle, driver.Title, "Page title is not equal");
            Util.Snapshot(driver, "WhenSearchTermEnteredThenResultPageLoaded");
        }

        private IWebDriver LoadWebDriver(String browser = "edge")
        {
            IWebDriver driver = null;
            switch (browser)
            {
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments(
                        "--disable-dev-shm-usage",
                        "--disable-download-protection",
                        "--disable-extensions",
                        "--disable-gpu",
                        "--disable-notifications",
                        "--ignore-certificate-errors",
                        "--no-sandbox",
                        "--safebrowsing-disable-download-protection",
                        "--safebrowsing-disable-extension-blacklist",
                        "--start-fullscreen"
                    );
                    driver = new ChromeDriver(DriverPath, chromeOptions);
                    break;
                case "edge":
                    EdgeOptions edgeOptions = new EdgeOptions();
                    edgeOptions.AcceptInsecureCertificates = true;
                    edgeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
                    // edgeOptions.BinaryLocation = @"c:\bin\"; // using `dotnet add package Selenium.WebDriver.MSEdgeDriver -Version 98.0.1108.56` or PATH environment instead
                    // edgeOptions.UseWebView = true;
                    driver = new EdgeDriver(edgeOptions);
                    break;
            }
            return driver;
        }
    }
}
