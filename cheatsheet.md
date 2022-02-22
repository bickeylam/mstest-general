<![CDATA[
-- author: lambilly@hotmail.com
-- description: Selenium cheatsheet
-- date: 19/02/2022
-- sample: ((JavaScriptExecutor)driver).ExecuteScript("arguments[0].", elem);
]]>

# Cheatsheet

## references
- [Selenium WebDriver Recipes in C#](https://www.amazon.com.au/Selenium-WebDriver-Recipes-C-Second-ebook/dp/B01JG1FAZI/ref=sr_1_1?crid=2Y4GNQC2LEY1V&keywords=Selenium+WebDriver+Recipes+in+C%23&qid=1645234629&s=books&sprefix=selenium+webdriver+recipes+in+c+%2Cstripbooks%2C227&sr=1-1)

## TextField and TextArea
### via JavaScript
```
IWebElement elem = driver.FindElement(By.Id(""));
((IJavaScriptExecutor)driver).ExecuteScript("argments[0].click();", elem);
```

### focus on control
```
IWebElement elem = driver.FindElement(By.Id(""));
((JavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", elem);
```
### Disabled an element
```
IWebElement elem = driver.FindElement(By.Id(""));
Assert.IsTrue(elem.Enabled);
((IJavaScriptexecutor)driver).ExecuteScript("arguments[0].disabled = true;", elem);
Assert.IsFalse(elem.Enabled);
```

### Set and assert the value of hidden field
```
<input type="hidden" name="currency" value="AUD" />
IWebElement elem = driver.FindElement(By.Name("currency"));
Assert.AreEqual("AUD", elem.GetAttribute("value"));
((IJavaScriptexecutor)driver).ExecuteScript("arguments[0].value="AUD";", elem);
Assert.AreEqual("AUD", elem.GetAttribute("value"));
```

## Radio Button
### Select or Clear button
``` 
<input type="radio" name="gender" value="male" id="radio_male" checked="true">Male
driver.FindElement(By.XPath("//input[@name='gender' and @value='female']")).Click();

-- or --
driver.FindElement(By.Id("radio_male")).Click();
driver.FindElement(By.Id("radio_male")).Clear();
driver.FindElement(By.Name("gender"))[0].Click();

-- click button by the following label
driver.FindElement(By.XPath("//div[@id='div1']//label[contains(.,'Yes')]/../input[@type='radio']")).Click();
```

## Select List
```
<select name="cbo" id="cbo">
  <option value="">--- Select ---</option>
  <option value="one">One</option>
  <option value="two">Two</option>
</select>

IWebElement elem = driver.FindElement(By.Name("cbo"));
SelectElement select = new SelectElement(elem);
select.SelectByText("one");
select.SelectByValue("One");
select.SelectByIndex(1); // 0 based index
select.DeselectAll();

foreach(IWebElement option in elem.FindElements(By.TagName("option))) {
  if (option.Text.Equals("One")) {
    option.Click();
  }
}
```

## Navigation and Browser
```
driver.Url = "";
driver.Navigate().GoToUrl("");
driver.Navigate().Back();
driver.Navigate().Refresh();
driver.Navigate().Forward();

--- not being tested ---
IWebElement elem = driver.FindElement(By.Id(""));
int pos = elem.Location.Y;
((IJavaScriptExecutor)driver).ExecuteScript($"window.scroll(0, {pos});");
elem.Click();

--- scroll to the bottom of a page ---
String js = "window.scrollTo(0, document.body.scrollHeight);";
((IJavaScriptExecutor)driver).ExecuteScript(js);
driver.FindElement(By.Id("")).SendKeys(Keys.Control + Keys.End);
```

### Assert text in table
```
string xpath = @"//table/tbody/tr[2]/td[2]";
Assert.AreEqual<string>("expected", driver.FindElement(By.XPath(xpth)).Text);
```

## HTML and Javascript
### Invoke JavsScript events `onChange`
```
IWebElement elem = driver.FindElement(By.Id(""));
((IJavaScrptExecutor)driver).ExecuteScript("$('#elem_id').trigger('change')");
Assert.IsTrue(elem.Text.Equals("..."));
```

## Optimisation
- Enter large text into a text box
```
string text = new string('*', 5000);
IWebElement elem = driver.FindElement(By.Id(""));
elem.SendKeys(text);

--- or (faster) ---
((IJavaScriptExecutor)driver).ExecuteScript("document.getElementById('').value=arguments[0];", text);
```
- Use environment variables as configuration
```
set BROWSER=chrome
String browser = Environment.GetEnvironmentVariable("BROWSER");
if (!String.IsNullOrEmpty(browser) && browser.Equals("chrome")) {
    driver = new ChromeDriver();
}
```