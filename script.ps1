[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateSet('test', 'clean', 'selftest')]
    [string]$Argument = 'selftest',
    [switch]$Clean
)

# ## reference
# - [reference](https://github.com/sergueik/powershell_selenium)
# - [chromedriver](https://chromedriver.chromium.org/downloads)
# - [reference](https://adamtheautomator.com/selenium-powershell/)

$ErrorActionPreference = 'STOP'
Set-Variable -Name BaseUrl -Value 'https://www.google.com.au' -Option Constant -Force
Set-Variable -Name Timestamp -Value (Get-Date -f yyyyMMddHHmmssss) -Option Constant -Force
Set-Variable -Name TranscriptPath -Value "./logs/$($Timestamp)_transcript.log" -Option Constant -Force
Set-Variable -Name DefaultWait -Value ([System.TimeSpan]::FromSeconds(30)) -Option Constant -Force

$CurrentPath = $PSScriptRoot

# --- driver related ---
$driverPath = "${CurrentPath}\MyTester.Test\bin\Debug\net6.0"
if (($Env:Path -split ';') -notcontains $driverPath) {
    $Env:Path += ";$driverPath"
    Write-Verbose ($Env:Path)
}
# -- either one --
Add-Type -Path "$driverPath\WebDriver.dll"
# Import-Module "$driverPath\WebDriver.dll"
#[System.Reflection.Assembly]::LoadFrom("$DriverPath\WebDriver.dll")
$driver # Global Web driver

# $snapshotPath = './snapshot'
$OutputPath = './output'
if (!(Test-Path $OutputPath)) {
    New-Item -Path $OutputPath -ItemType "directory"
}
$titlePath = "${OutputPath}/$($Timestamp)_title.txt"
$pageSourcePath = "${OutPutPath}/$($Timestamp)_pagesource.html"



function LoadDriver {
    $options = New-Object OpenQA.Selenium.Chrome.ChromeOptions
    $options.AddArguments(@(
            "--disable-dev-shm-usage",
            "--disable-download-protection",
            "--disable-extensions",
            "--disable-gpu",
            "--disable-notifications",
            "--ignore-certificate-errors",
            "--no-sandbox",
            "--safebrowsing-disable-download-protection",
            "--safebrowsing-disable-extension-blacklist",
            "--start-maximized"      
        ))
    $options.AcceptInsecureCertificates = $true    
    $Global:driver = New-Object OpenQA.Selenium.Chrome.ChromeDriver($options)   
}

function BeforeTest {
    LoadDriver
    $driver.Manage().Cookies.DeleteAllCookies()
    $driver.Manage().Timeouts().PageLoad = $DefaultWait
    $driver.Manage().Timeouts().ImplicitWait = $DefaultWait
}

function AfterTest {
    $driver.Manage().Cookies.DeleteAllCookies()
    $driver.Close()
    $driver.Quit()
    Stop-ChromeDriver
}

function Stop-ChromeDriver {
    #Get-Process -Name chrome -ErrorAction SilentlyContinue | Stop-Process -ErrorAction SilentlyContinue
}

function CleanAll {
    @(
        './output/*.*'
        './logs/*.*'
    ) | ForEach-Object {
        if (Test-Path $_) {
            Write-Verbose "Removing content of $_"
            Remove-Item -Path $_ -Recurse -Force -ErrorAction Ignore
        }
    }
}

function StringAssert {
    Param(
        [Parameter(Mandatory, Position = 0)][string] $Expected, 
        [Parameter(Mandatory, Position = 1)][string] $Actual
    )

    if ($Expected -ne $Actual) {
        Write-Verbose "Expected: $Expected"
        Write-Verbose "Actual: $Actual"
        Write-Error "Expected '$Expected' but was '$Actual'"
    }
}

function StringStartsWith {
    Param(
        [Parameter(Mandatory, Position = 0)][string] $Expected, 
        [Parameter(Mandatory, Position = 1)][string] $Actual
    )

    if (-Not ($Actual.StartsWith($Expected))) {
        Write-Verbose "Expected: $Expected"
        Write-Verbose "Actual: $Actual"
        Write-Error "Expected starts with '$Expected' but was '$Actual'"
    }
}

function SelfTest {
    Param(
        [Parameter(Mandatory = $true, Position = 0)][string] $SearchTerm
    )	

    BeforeTest
    $driver.Url = $BaseUrl
    $elem = $driver.FindElement([OpenQA.Selenium.By]::Name('q'))
    $elem.SendKeys('selenium')
    $elem.Submit()
    Start-Sleep -Seconds 0
    Write-Host "PageTitle: $($driver.Title)"
    StringStartsWith $SearchTerm $driver.Title
    # StringAssert 'powershell selenium - Google Search' $driver.Title
    
    $driver.Title | Out-File $titlePath
    $driver.PageSource | Out-File $pageSourcePath
    AfterTest
}

function Main {
    if ($Clean.IsPresent) {
        CleanAll
    }
    switch ($Argument) {
        clean { CleanAll }     
        selftest { SelfTest -SearchTerm 'selenium' }   
        default { }
    }
}


Clear-Host
Start-Transcript -Path $transcriptPath
Main
Stop-Transcript
