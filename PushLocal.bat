@echo off
echo PRESS ANY KEY TO INSTALL TO LOCAL NUGET FEED
echo Remember to generate the up-to-date package.
c:\exe\nuget add .\Cadmus.Philology.Parts\bin\Debug\Cadmus.Philology.Parts.11.0.0.nupkg -source C:\Projects\_NuGet
c:\exe\nuget add .\Cadmus.Philology.Tools\bin\Debug\Cadmus.Philology.Tools.0.0.1.nupkg -source C:\Projects\_NuGet
c:\exe\nuget add .\Cadmus.Seed.Philology.Parts\bin\Debug\Cadmus.Seed.Philology.Parts.11.0.0.nupkg -source C:\Projects\_NuGet
pause
