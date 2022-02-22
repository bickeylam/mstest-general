# MSTest-Selenium

## References

- [Windows Authentication Automation with WebDriver and DevTools API](https://babu-testleaf.medium.com/solved-windows-authentication-automation-with-webdriver-and-devtools-api-1894d1c2dd91)
- [TestDataMethod examples](https://www.meziantou.net/mstest-v2-data-tests.htm)
- [Microsoft Selenium](https://docs.microsoft.com/en-us/microsoft-edge/webdriver-chromium/?tabs=c-sharp)
- [EdgeDriver](https://developer.microsoft.com/microsoft-edge/tools/webdriver)

## Usages

- [MSTest filter options](https://docs.microsoft.com/en-us/dotnet/core/testing/selective-unit-tests?pivots=mstest)
- [filter options](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test#filter-option-details)
- [MSTest logger options](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test)

```code
dotnet test --filter 'TestCategory=SelfTest'
dotnet test --filter FullyQualifiedName=
dotnet test --filter Name~TestMethod1 # Runs tests whose name contains TestMethod1.
dotnet test --filter ClassName=
dotnet test --filter TestCategory=CategoryName
dotnet test --filter Priority=2
dotnet test --logger trx --filter <Expression> # options: =, !=, ~ (contain), ~! (not contain) | (or) & (and) 
dotnet test --logger trx --filter 'FullyQualifiedName=MyTester.Test.BrowserBasedTest.WhenSearchTermEnteredThenResultPageLoaded'
```

## Packages
Updating packages to the latest version using
```
dotnet add package <package_name>
dotnet add package Selenium.WebDriver
dotnet add package Selenium.Support
dotnet add package Selenium.WebDriver --version
dotnet add package Selenium.WebDriver.MSEdgeDriver -Version 98.0.1108.56
```

