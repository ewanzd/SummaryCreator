param([System.IO.FileInfo]$publishDirectory=$variable:pwd.Path + "/publish")

$targetProject = ".\src\SummaryCreator\SummaryCreator.csproj"

dotnet restore
dotnet build
dotnet test
dotnet publish -r win-x64 -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:IncludeNativeLibrariesForSelfExtract=true --self-contained true -o $publishDirectory.FullName $targetProject