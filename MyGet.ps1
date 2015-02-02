%MsBuildExe% .\Abmes.UnityExtensions\Abmes.UnityExtensions.csproj /verbosity:minimal

if (Test-Path -Path bin)
{
    Remove-Item bin -Recurse -Force
}

mkdir bin

Copy-Item .\Abmes.UnityExtensions.nuspec bin
mkdir bin\lib\net45


Copy-Item .\Abmes.UnityExtensions\bin\Release\Abmes.UnityExtensions.dll .\bin\lib\net45

cd bin
%NuGet% pack Abmes.UnityExtensions.nuspec -Version %PackageVersion%
cd..


if (-not (Test-Path "build"))
{
    mkdir build
}

Copy-Item .\bin\*.nupkg .\build

Remove-Item bin -Recurse -Force
