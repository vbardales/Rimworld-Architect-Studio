# Generates FRENCH_REVIEW.md (repository root, never inside Mod/) from the shipped XML: one table per source
# file, one row per player-facing text, columns Key or path | Original | English | French | ?.
# TRANSLATIONS.md, "Review file": the columns are read from the files, never typed, so they cannot drift.
#   pwsh -NoProfile -File Tests/New-FrenchReview.ps1
param(
    [string]$Output
)
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
if (-not $Output) { $Output = Join-Path $root 'FRENCH_REVIEW.md' }
$mod = Join-Path $root 'Mod'

# Rows the session doubts, with the reason (fifth column). Everything else is left blank.
$doubts = @{
    'ArchitectStudio.Dropdowns.MoreResults.Zero'     = 'Unused form: the game draws this line only when results exceed the limit (never with 0). Kept for the validator.'
    'ArchitectStudio.Dropdowns.ConfirmDissolve.Zero' = 'Unused form: an empty group uses ConfirmDissolveEmpty instead. Kept for the validator.'
    'ArchitectStudio.Dropdowns.HiddenCount.Zero'     = 'Unused form: drawn only when at least one group is hidden.'
    'ArchitectStudio.Common.RemoveGlyph'             = 'A one-letter glyph, not a word; same in both languages.'
    'ArchitectStudio_Settings.description'           = "Typographic apostrophe ($([char]0x2019)) here, straight (') everywhere else in Keyed."
}

function Get-Entries([string]$path) {
    $xml = New-Object System.Xml.XmlDocument
    $xml.PreserveWhitespace = $false
    $xml.Load($path)
    $out = [ordered]@{}
    foreach ($n in $xml.DocumentElement.ChildNodes) {
        if ($n.NodeType -eq 'Element') { $out[$n.Name] = $n.InnerText }
    }
    $out
}
function Cell([string]$s) { if ($null -eq $s) { '' } else { ($s -replace '\|', '\|' -replace "`r?`n", '<br>') } }
function Table($title, $source, $fr, $en, $extraNote) {
    $sb = New-Object System.Text.StringBuilder
    [void]$sb.AppendLine("## $title")
    [void]$sb.AppendLine()
    [void]$sb.AppendLine("Source: ``$source``$extraNote")
    [void]$sb.AppendLine()
    [void]$sb.AppendLine('| Key or path | Original | English | French | ? |')
    [void]$sb.AppendLine('|---|---|---|---|---|')
    foreach ($k in $fr.Keys) {
        $e = if ($en.Contains($k)) { $en[$k] } else { '' }
        $d = if ($doubts.ContainsKey($k)) { $doubts[$k] } else { '' }
        [void]$sb.AppendLine("| ``$k`` | $(Cell $e) | $(Cell $e) | $(Cell $fr[$k]) | $(Cell $d) |")
    }
    [void]$sb.AppendLine()
    $sb.ToString()
}

$rev = (git -C $root rev-parse --short HEAD).Trim()
$dirty = (git -C $root status --short -- Mod/Languages Mod/Defs) -join ''
$enKeyed = Get-Entries (Join-Path $mod 'Languages/English/Keyed/ArchitectStudio.xml')
$frKeyed = Get-Entries (Join-Path $mod 'Languages/French/Keyed/ArchitectStudio.xml')

# Def fields: the English text lives in the Def itself (the game's native fallback).
$enDef = [ordered]@{}
$kb = New-Object System.Xml.XmlDocument; $kb.Load((Join-Path $mod 'Defs/KeyBindings.xml'))
foreach ($d in $kb.SelectNodes('//KeyBindingDef')) { $enDef["$($d.defName).label"] = $d.label }
$mb = New-Object System.Xml.XmlDocument; $mb.Load((Join-Path $mod 'Defs/MainButtons.xml'))
foreach ($d in $mb.SelectNodes('//MainButtonDef')) { $enDef["$($d.defName).label"] = $d.label; $enDef["$($d.defName).description"] = $d.description }

$text = New-Object System.Text.StringBuilder
[void]$text.AppendLine('# Architect Studio: French review')
[void]$text.AppendLine()
[void]$text.AppendLine("Generated from revision ``$rev`` by ``Tests/New-FrenchReview.ps1`` (reads the shipped XML; do not edit by hand)." + $(if ($dirty) { " Uncommitted changes in ``Mod/Languages`` or ``Mod/Defs`` at generation: $($dirty.Trim())." } else { ' Working tree clean for `Mod/Languages` and `Mod/Defs`.' }))
[void]$text.AppendLine()
[void]$text.AppendLine('The mod has no source in another language (it is original work, studying two English mods), so the **Original** column repeats the English text.')
[void]$text.AppendLine()
[void]$text.AppendLine('**Gender agreement.** None of these texts agrees with a pawn: the mod names no colonist, no pawn and no person, so no `{PAWN_gender ? ... }` switch exists or is needed. Counted phrases use `.Zero`, `.One` and `.Many` key families chosen by the count. The French register avoids the bare imperative and the tutoiement: it describes the possibility or uses the infinitive.')
[void]$text.AppendLine()
[void]$text.AppendLine('The fifth column carries `?` rows: texts the session doubts, with the reason. No review line is written here; it belongs to the reviewer in `STATUS.md`.')
[void]$text.AppendLine()
[void]$text.Append((Table 'Keyed' 'Mod/Languages/French/Keyed/ArchitectStudio.xml' $frKeyed $enKeyed " (English: ``Mod/Languages/English/Keyed/ArchitectStudio.xml``)"))
$frKb = Get-Entries (Join-Path $mod 'Languages/French/DefInjected/KeyBindingDef/KeyBindings.xml')
[void]$text.Append((Table 'DefInjected: KeyBindingDef' 'Mod/Languages/French/DefInjected/KeyBindingDef/KeyBindings.xml' $frKb $enDef ' (English: the Def itself, `Mod/Defs/KeyBindings.xml`)'))
$frMb = Get-Entries (Join-Path $mod 'Languages/French/DefInjected/MainButtonDef/MainButtons.xml')
[void]$text.Append((Table 'DefInjected: MainButtonDef' 'Mod/Languages/French/DefInjected/MainButtonDef/MainButtons.xml' $frMb $enDef ' (English: the Def itself, `Mod/Defs/MainButtons.xml`)'))
[void]$text.AppendLine('## Grammar')
[void]$text.AppendLine()
[void]$text.AppendLine('No `Languages/French/Strings` or grammar rules ship with this mod.')

[System.IO.File]::WriteAllText($Output, ($text.ToString() -replace "`r`n", "`n"), (New-Object System.Text.UTF8Encoding($false)))
Write-Host "Wrote $Output ($($frKeyed.Count) keyed, $($frKb.Count + $frMb.Count) def rows)"
