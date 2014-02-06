@echo off
IF NOT EXIST NuGetOutput md NuGetOutput

nuget pack GameFramework.nuspec -OutputDirectory NuGetOutput
nuget pack Windows.nuspec -OutputDirectory NuGetOutput

pause