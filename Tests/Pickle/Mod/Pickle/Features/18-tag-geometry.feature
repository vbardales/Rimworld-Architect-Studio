# Not a test of the mod: a measurement, kept only until the click at 150% is fixed. It asserts
# nothing beyond having measured something, and attaches what Pickle's tag store is handed for the
# Architect Studio button, and what it turns that into, at both scales.
#
# The pair is the point. At 100% the resolved rect sits inside the window that drew the button; at
# 150% it falls below it, which is why the pointer is sent off screen. The two attachments side by
# side say which part of the conversion carries the scale and which does not - and that is what the
# fix in Pickle needs before it can be written.
@review
Feature: what Pickle measures for a tagged button

  Background:
    Given the save "test-colony" is loaded

  Scenario: at 100 percent
    When I close all dialogs
    And I open the "Architect" tab
    And I record what Pickle measures for the Architect Studio button keyed "ArchitectStudio.ArchitectButton"

  Scenario: at 150 percent
    When I close all dialogs
    And I set the interface scale to 150 percent
    And I open the "Architect" tab
    And I record what Pickle measures for the Architect Studio button keyed "ArchitectStudio.ArchitectButton"
