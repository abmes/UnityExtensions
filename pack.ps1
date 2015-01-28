Set-Alias msbuild "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
msbuild .\Abmes.UnityExtensions\Abmes.UnityExtensions.csproj /verbosity:minimal

if (Test-Path -Path bin)
{
    Remove-Item bin -Recurse -Force
}

mkdir bin

Copy-Item .\Abmes.UnityExtensions.nuspec bin
mkdir bin\lib\net45


Copy-Item .\Abmes.UnityExtensions\bin\Release\Abmes.UnityExtensions.dll .\bin\lib\net45


echo ""
echo "Downloading NuGet.exe ..."
echo "~~~~~~~~~~~~~~~~~~~~~~~~~"

$webClient = New-Object System.Net.WebClient
$webClient.DownloadFile("https://nuget.org/nuget.exe", "nuget.exe")

cd bin
..\nuget.exe pack Abmes.UnityExtensions.nuspec
cd..

Remove-Item nuget.exe


if (-not (Test-Path "build"))
{
    mkdir build
}

Copy-Item .\bin\*.nupkg .\build

Remove-Item bin -Recurse -Force

pause