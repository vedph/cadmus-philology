@echo off
echo BUILD Cadmus Philology Packages
del .\Cadmus.Philology.Parts\bin\Debug\*.nupkg
del .\Cadmus.Philology.Parts\bin\Debug\*.snupkg
del .\Cadmus.Seed.Philology.Parts\bin\Debug\*.nupkg
del .\Cadmus.Seed.Philology.Parts\bin\Debug\*.snupkg
del .\Cadmus.Philology.Tools\bin\Debug\*.nupkg
del .\Cadmus.Philology.Tools\bin\Debug\*.snupkg

cd .\Cadmus.Philology.Parts
dotnet pack -c Debug -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg
cd..

cd .\Cadmus.Seed.Philology.Parts
dotnet pack -c Debug -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg
cd..

cd .\Cadmus.Philology.Tools
dotnet pack -c Debug -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg
cd..

pause
