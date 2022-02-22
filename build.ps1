[CmdletBinding()]
Param(
    [Parameter(Position = 0, Mandatory = $false)]
    [ValidateSet('run', 'clean', 'selftest')]
    [string]$Argument = 'run',
    [switch]$Clean
)

New-Variable -Name Timestamp -Value (Get-Date -f yyyyMMddHHmmssss) -Option Constant -Force
New-Variable -Name TranscriptPath -Value "./logs/$($Timestamp)_transcript.log" -Option Constant -Force

$ErrorActionPreference = 'STOP'

function StartClean {
    @(
        './logs/*.log'
        './MyTester.Test/TestResults/*'
    ).ForEach({
            if (Test-Path $_) {
                Write-Verbose "deleting $_ content"
                Remove-Item -Path $_ -Recurse -Force -ErrorAction Ignore
            }
        })
}

function StartRun {
    dotnet test -l trx --filter 'FullyQualifiedName=MyTester.Test.BrowserBasedTest.WhenSearchTermEnteredThenResultPageLoaded'
}

function doSelfTest {
    dotnet test --logger trx --filter 'TestCategory=SelfTest'
}

function Main {
    if ($Clean.IsPresent) {
        StartClean
    }
    switch ($Argument) {
        run { StartRun }
        clean { StartClean }
        selftest { doSelfTest }
        default { doSelfTest }
    }
}

Clear-Host
Start-Transcript -Path $TranscriptPath
Write-Host $PSScriptRoot
Main
Stop-Transcript
