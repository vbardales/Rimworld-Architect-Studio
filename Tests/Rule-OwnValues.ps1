param([Parameter(Mandatory)][string]$ModRoot, [string]$SharedDir)

# Own values and facts of Architect Studio (published id and Harmony dependency, counted-text exemptions, key binding, hidden settings
# shortcut): one consumer, nothing shareable; Tests/Test-Mod.ps1 (generated) runs it after the shared checks. Prints findings, exit 1 on any.
# Parse, key and placeholder parity, DefInjected, licence copies and development files are done by the shared checks.
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path -LiteralPath $ModRoot).Path
$script:findings = @()

function It([string]$name, [scriptblock]$body) {
    try { $problems = @(& $body | Where-Object { $_ }) } catch { $problems = @("threw: $($_.Exception.GetBaseException().Message)") }
    foreach ($p in $problems) { $script:findings += "[$name] $p" }
}
function Read-Xml([string]$path) { [xml](Get-Content -LiteralPath $path -Raw -Encoding UTF8) }
function Keys([string]$language) {
    $keys = @{}
    foreach ($file in Get-ChildItem "$root/Mod/Languages/$language/Keyed" -Filter *.xml) {
        foreach ($node in (Read-Xml $file.FullName).DocumentElement.ChildNodes) { if ($node.NodeType -eq 'Element') { $keys[$node.Name] = $node.InnerText } }
    }
    $keys
}

It 'Published id, Harmony dependency and RimWorld 1.6 support' {
    $about = (Read-Xml "$root/Mod/About/About.xml").ModMetaData
    if ($about.packageId -cne 'nelim.architectstudio') { 'Save-compatible packageId changed' }
    if ($about.supportedVersions.li -notcontains '1.6') { 'Missing RimWorld 1.6 support' }
    if ($about.modDependencies.li.packageId -notcontains 'brrainz.harmony') { 'Harmony dependency missing' }
}

# The .Zero form of these keys cannot be shown by the game as written (owner's call, STATUS.md). In Dialog_DropdownGroups:
#   ConfirmDissolve: DeleteConfirmationText routes a member count of 0 to ConfirmDissolveEmpty instead;
#   HiddenCount: drawn only under `if (hiddenCount > 0)`; MoreResults: drawn only when `total > shown.Count`, so at least 1.
# OverrideCount is drawn unconditionally: its .Zero is real and stays required.
It 'Every CountedText key has its counted forms in English and French' {
    $noZeroForm = @('ArchitectStudio.Dropdowns.ConfirmDissolve', 'ArchitectStudio.Dropdowns.HiddenCount', 'ArchitectStudio.Dropdowns.MoreResults')
    $languages = @{ English = Keys 'English'; French = Keys 'French' }
    foreach ($file in Get-ChildItem "$root/Source" -Recurse -Filter *.cs) {
        foreach ($match in [regex]::Matches([IO.File]::ReadAllText($file.FullName), 'CountedText\.Get\(\s*"(ArchitectStudio\.[^"]+)"')) {
            $key = $match.Groups[1].Value
            $forms = if ($noZeroForm -ccontains $key) { @('One', 'Many') } else { @('Zero', 'One', 'Many') }
            foreach ($language in $languages.Keys) { foreach ($form in $forms) {
                if (-not $languages[$language].ContainsKey("$key.$form")) { "Missing counted form: $language/$key.$form" }
            } }
        }
    }
}

It 'Key binding is the Game-category shortcut with no default key, labelled in French' {
    $def = (Read-Xml "$root/Mod/Defs/KeyBindings.xml").Defs.KeyBindingDef
    if ($def.defName -cne 'ArchitectStudio_OpenDropdowns') { 'KeyBinding DefOf reference broken' }
    if ($def.category -cne 'Game') { 'Incorrect keybinding category' }
    if ($def.defaultKeyCodeA -cne 'None') { 'Default shortcut collides with game tabs' }
    foreach ($node in (Read-Xml "$root/Mod/Languages/French/DefInjected/KeyBindingDef/KeyBindings.xml").DocumentElement.ChildNodes) {
        if ($node.NodeType -ne 'Element') { continue }
        if ($node.Name -cne "$($def.defName).label") { "Invalid injected keybinding field: $($node.Name)" }
    }
}

It 'Settings shortcut is hidden by default, usable without a map, and opens the settings dialog' {
    $button = (Read-Xml "$root/Mod/Defs/MainButtons.xml").Defs.MainButtonDef
    if ($button.defName -cne 'ArchitectStudio_Settings') { 'Settings shortcut identifier changed' }
    if ($button.workerClass -cne 'ArchitectStudio.MainButtonWorker_ArchitectStudio') { 'Settings shortcut worker mismatch' }
    if ($button.buttonVisible -cne 'false') { 'Settings shortcut must be hidden by default' }
    if ($button.validWithoutMap -cne 'true') { 'Settings shortcut must remain usable without a map' }
    if (-not [string]::IsNullOrEmpty($button.tabWindowClass)) { 'Settings shortcut must open the existing settings dialog, not a separate tab' }
    $fr = (Read-Xml "$root/Mod/Languages/French/DefInjected/MainButtonDef/MainButtons.xml").LanguageData
    foreach ($field in 'label', 'description') {
        if ([string]::IsNullOrWhiteSpace($button.$field)) { "Missing English shortcut $field" }
        if ([string]::IsNullOrWhiteSpace($fr."ArchitectStudio_Settings.$field")) { "Missing French shortcut $field" }
    }
}

if ($script:findings.Count -gt 0) { $script:findings | ForEach-Object { Write-Output $_ }; exit 1 }
Write-Output 'PASS: Rule-OwnValues'
