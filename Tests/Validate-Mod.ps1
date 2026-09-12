$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$checks = 0
function Assert($condition, $message) {
    if (-not $condition) { throw $message }
    $script:checks++
}
function Read-Xml($path) {
    $doc = New-Object System.Xml.XmlDocument
    $doc.XmlResolver = $null
    $doc.Load($path)
    return ,$doc
}
foreach ($file in Get-ChildItem "$root/Mod" -Recurse -Filter *.xml) {
    $doc = Read-Xml $file.FullName
    $expected = if ($file.FullName -match '[\\/]Languages[\\/]') { 'LanguageData' }
        elseif ($file.FullName -match '[\\/]Defs[\\/]') { 'Defs' } else { 'ModMetaData' }
    Assert ($doc.DocumentElement.LocalName -ceq $expected) "Wrong XML root: $file"
}
$about = (Read-Xml "$root/Mod/About/About.xml").ModMetaData
Assert ($about.packageId -ceq 'nelim.architectstudio') 'Save-compatible packageId changed'
Assert ($about.supportedVersions.li -contains '1.6') 'Missing RimWorld 1.6 support'
Assert ($about.modDependencies.li.packageId -contains 'brrainz.harmony') 'Harmony dependency missing'
$github = 'https://github.com/vbardales/Rimworld-Architect-Studio'
Assert ($about.url -ceq $github) 'Incorrect source URL'
Assert ($about.description.Contains($github)) 'Description lacks GitHub link'
$languages = @{}
foreach ($language in @('English', 'French')) {
    $entries = [System.Collections.Generic.Dictionary[string,string]]::new([StringComparer]::Ordinal)
    foreach ($file in Get-ChildItem "$root/Mod/Languages/$language/Keyed" -Filter *.xml) {
        foreach ($node in (Read-Xml $file.FullName).DocumentElement.ChildNodes) {
            if ($node.NodeType -ne 'Element') { continue }
            Assert (-not $entries.ContainsKey($node.Name)) "Duplicate key: $language/$($node.Name)"
            Assert (-not [string]::IsNullOrWhiteSpace($node.InnerText)) "Empty translation: $language/$($node.Name)"
            $entries.Add($node.Name, $node.InnerText)
        }
    }
    $languages[$language] = $entries
}
foreach ($language in @('English', 'French')) {
    foreach ($key in $languages[$language].Keys) {
        Assert ($languages.English.ContainsKey($key) -and $languages.French.ContainsKey($key)) "Missing translation: $key"
        $en = @([regex]::Matches($languages.English[$key], '\{\d+(?:[^{}]*)\}') | ForEach-Object Value | Sort-Object) -join '|'
        $fr = @([regex]::Matches($languages.French[$key], '\{\d+(?:[^{}]*)\}') | ForEach-Object Value | Sort-Object) -join '|'
        Assert ($en -ceq $fr) "Translation placeholders differ: $key"
    }
}
foreach ($file in Get-ChildItem "$root/Source" -Recurse -Filter *.cs) {
    foreach ($match in [regex]::Matches([IO.File]::ReadAllText($file.FullName), '"(ArchitectStudio\.[^"]+)"\s*\.Translate\(')) {
        Assert ($languages.English.ContainsKey($match.Groups[1].Value)) "Missing source translation: $($match.Groups[1].Value)"
    }
}
$defs = (Read-Xml "$root/Mod/Defs/KeyBindings.xml").Defs.KeyBindingDef
Assert ($defs.defName -ceq 'ArchitectStudio_OpenDropdowns') 'KeyBinding DefOf reference broken'
Assert ($defs.category -ceq 'Game') 'Incorrect keybinding category'
Assert ($defs.defaultKeyCodeA -ceq 'None') 'Default shortcut collides with game tabs'
foreach ($node in (Read-Xml "$root/Mod/Languages/French/DefInjected/KeyBindingDef/KeyBindings.xml").DocumentElement.ChildNodes) {
    if ($node.NodeType -ne 'Element') { continue }
    Assert ($node.Name -ceq "$($defs.defName).label") "Invalid injected keybinding field: $($node.Name)"
    Assert (-not [string]::IsNullOrWhiteSpace($node.InnerText)) 'Empty injected label'
}
foreach ($name in @('LICENSE', 'LICENSE-fernyrepos.txt', 'ATTRIBUTION.md')) {
    Assert ((Get-FileHash "$root/$name").Hash -eq (Get-FileHash "$root/Mod/$name").Hash) "Distribution notice differs: $name"
}
$unwanted = @(Get-ChildItem "$root/Mod" -Recurse -File | Where-Object { $_.Extension -in '.cs', '.csproj', '.pdb' -or $_.Name -eq 'Assembly-CSharp.dll' })
Assert ($unwanted.Count -eq 0) 'Development files or game assembly in published mod'
Write-Output "PASS: $checks checks (XML, metadata, localization, keybinding, distribution)."

