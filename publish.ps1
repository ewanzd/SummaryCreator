param([System.IO.FileInfo]$publishDirectory=$variable:pwd.Path + "/Publish")

$targetProject = ".\src\SummaryCreator\SummaryCreator.csproj"

dotnet restore
dotnet build
dotnet test
dotnet publish -r win10-x64 /p:PublishSingleFile=true -o $publishDirectory.FullName $targetProject