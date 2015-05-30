pushd (Join-Path $PSScriptRoot src)

$regkey =  (Get-ItemProperty REGISTRY::HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\12.0).MSBuildToolsPath

$msbuild = $regkey + "msbuild.exe"

& $msbuild /p:Configuration=Release Bridge.sln 

robocopy /MIR bridge\bin\release ..\Artifacts
dir ..\Artifacts

Write-Host 
Write-Host For a quick test run -  (Join-Path $PSScriptRoot "Artifacts\ensureBridge.ps1") -foregroundcolor "green" -backgroundcolor "black"
Write-Host
popd