param([System.IO.FileInfo]$publishDirectory=$variable:pwd.Path + "/Publish")

$targetProject = ".\src\SummaryCreator\SummaryCreator.csproj"

dotnet restore
dotnet build
dotnet test
dotnet publish -c Release -o $publishDirectory.FullName $targetProject