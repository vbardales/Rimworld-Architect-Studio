param(
    [string]$Managed = 'C:\Program Files (x86)\Steam\steamapps\common\RimWorld\RimWorldWin64_Data\Managed'
)
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$output = Join-Path $root '.build/behavior-tests'
New-Item -ItemType Directory -Force $output | Out-Null
if (-not (Test-Path (Join-Path $Managed 'Assembly-CSharp.dll'))) { throw "Missing game assemblies: $Managed" }
dotnet build (Join-Path $PSScriptRoot 'BehaviorTests.csproj') -c Release "-p:Managed=$Managed" --nologo
if ($LASTEXITCODE -ne 0) { throw 'Behavior harness build failed' }
dotnet (Join-Path $root '.build/behavior/bin/Release/net8.0/BehaviorTests.dll') $Managed $root $output
if ($LASTEXITCODE -ne 0) { throw 'Behavior tests failed' }

