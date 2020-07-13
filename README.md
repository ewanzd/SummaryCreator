# SummaryCreator

SummaryCreator collects time series data from `.csv` and meteorological data from `.xml` files, evaluates them and writes the result to an `.xlsx` file.

## Requirements

- .NET Core 3.1 SDK ([Download](https://dotnet.microsoft.com/download))

## Build and Publish

There are two (or more) ways:

- 1: Run following line in Powershell in project root directory:

`.\publish.ps1 <publishing directory>`

- 2: Right click on `publish.ps1` and click "Run with PowerShell" and confirm with `Y` if there appears a warning about "Execution Policy Change". A folder with the name "Publish" is created in project root directory.
