# TESTING.md scenario 1. No save needed: everything here is settled at the main menu.
Feature: Architect Studio loads and its patches apply

  Scenario: the mod loads after Harmony
    Then mod "nelim.architectstudio" is loaded
    And mod "nelim.architectstudio" loads after "brrainz.harmony"

  Scenario: the build keeps the access-check waiver
    Then the Architect Studio assembly carries the access-check waiver
    When the access check probe runs
    Then no errors were logged

  Scenario: the patches from the constructor and the deferred ones are all in place
    Then Architect Studio patched "RimWorld.MainTabWindow_Architect::WinHeight"
    And Architect Studio patched "RimWorld.MainTabWindow_Architect::DoWindowContents"
    And Architect Studio patched "RimWorld.MainTabWindow_Architect::DoCategoryButton"
    And Architect Studio patched "Verse.DesignationCategoryDef::ResolveDesignators"
    And Architect Studio patched "RimWorld.Designator_Build::Visible"
    And Architect Studio patched "Verse.DesignationCategoryDef::Visible"

  Scenario: the key binding def is loaded
    Then def "ArchitectStudio_OpenDropdowns" of type "KeyBindingDef" exists

  Scenario: nothing from Architect Studio in the startup log
    # Covers the v1.0.0 threading crash, a lost publicizer waiver and a renamed Better Architect
    # Menu cache method, all of which announce themselves here and nowhere else.
    Then the game log holds nothing from Architect Studio since startup
