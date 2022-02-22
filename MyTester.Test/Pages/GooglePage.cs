namespace MyTester.Test.Pages
{
    using OpenQA.Selenium;
    public class GooglePage : PageBase
    {
        public readonly string Title = @"Google";
        public string SearchResultTitle = @"selenium - Google Search";
        public GooglePage(IWebDriver driver) : base(driver)
        { }

        public void EnterSearchTerm(string value)
        {
            SearchResultTitle = $"{value} - Google Search";
            InputText.Clear();
            InputText.SendKeys(value);
        }

        public void Submit()
        {
            InputText.Submit();
            WaitFor(5);   // waiting for result page to load and render
        }

        private void TakesSnapshot() {
            driver.FindElement(By.XPath(@"//body")).SendKeys(Keys.Control + Keys.End);
            base.Snapshot();
        }

        private IWebElement InputText => driver.FindElement(By.Name("q"));
    }
}