# Gamified Hospital Rescue System — QA Testing Checklist

Document version: 1.0
Date: August 2026

---

## Project Launch

| Test | Status |
|------|--------|
| Unity project opens without errors | NOT TESTED |
| No missing script compilation errors | NOT TESTED |
| Console shows no Error-level messages on startup | NOT TESTED |

---

## Scene Loading

| Test | Status |
|------|--------|
| HospitalGame.unity scene loads | NOT TESTED |
| Scene loads within reasonable time | NOT TESTED |
| No missing prefab warnings in Console | NOT TESTED |
| No missing material warnings in Console | NOT TESTED |

---

## Player Movement

| Test | Status |
|------|--------|
| Player spawns at correct position | NOT TESTED |
| WASD moves player forward/back/left/right | NOT TESTED |
| Mouse controls camera look direction | NOT TESTED |
| Sprint (Shift) increases movement speed | NOT TESTED |
| Crouch (Ctrl) lowers player height | NOT TESTED |
| Jump (Space) works | NOT TESTED |
| Player cannot walk through walls/objects | NOT TESTED |
| Player stays within hospital boundary | NOT TESTED |

---

## Camera Movement

| Test | Status |
|------|--------|
| Camera follows player movement | NOT TESTED |
| Mouse look rotates view smoothly | NOT TESTED |
| Camera does not clip through walls | NOT TESTED |
| Vertical look (up/down) is constrained | NOT TESTED |

---

## Collision & Physics

| Test | Status |
|------|--------|
| Player collides with hospital walls | NOT TESTED |
| Player collides with furniture/objects | NOT TESTED |
| No falling through floor | NOT TESTED |
| Patient objects have correct collision | NOT TESTED |
| Doors respond to player collision | NOT TESTED |

---

## Patient Interaction

| Test | Status |
|------|--------|
| Interaction prompt appears when near patient (<2.5m) | NOT TESTED |
| Interaction prompt disappears when far from patient | NOT TESTED |
| E key opens assessment UI when prompt is visible | NOT TESTED |
| Assessment UI shows correct patient name | NOT TESTED |
| Assessment UI shows correct symptoms | NOT TESTED |
| Treatment options are displayed | NOT TESTED |
| Selecting correct treatment shows green feedback | NOT TESTED |
| Selecting wrong treatment shows red feedback | NOT TESTED |
| Correct treatment awards +100 score | NOT TESTED |
| Wrong treatment applies -25 penalty | NOT TESTED |
| Wrong treatment allows retry | NOT TESTED |
| Treated patient no longer shows interaction prompt | NOT TESTED |

---

## Mission Progression

| Test | Status |
|------|--------|
| Stabilized counter updates after each patient | NOT TESTED |
| Counter shows correct format (e.g. "Stabilized: 1/5") | NOT TESTED |
| Mission completes when all 5 patients stabilized | NOT TESTED |
| Mission complete UI displays at end | NOT TESTED |
| Mission fails when timer reaches zero | NOT TESTED |
| Mission failed UI displays when timer expires | NOT TESTED |

---

## Scoring System

| Test | Status |
|------|--------|
| Score starts at 0 or default value | NOT TESTED |
| Correct treatment adds +100 to score | NOT TESTED |
| Wrong treatment subtracts -25 from score | NOT TESTED |
| Score cannot go below 0 | NOT TESTED |
| Final score is displayed at mission end | NOT TESTED |

---

## Timer

| Test | Status |
|------|--------|
| Timer counts down from correct start value | NOT TESTED |
| Timer displays in MM:SS or SS format | NOT TESTED |
| Timer stops when mission complete | NOT TESTED |
| Timer triggers mission failed at zero | NOT TESTED |
| Pause does not pause the timer (realtime) | NOT TESTED |

---

## UI / User Interface

| Test | Status |
|------|--------|
| Stabilized counter is visible on screen | NOT TESTED |
| Score display is visible on screen | NOT TESTED |
| Timer is visible on screen | NOT TESTED |
| Pause menu appears on Escape press | NOT TESTED |
| Pause menu can be closed | NOT TESTED |
| UI elements do not overlap | NOT TESTED |
| Assessment panel is readable | NOT TESTED |

---

## Audio

| Test | Status |
|------|--------|
| Background music plays (if any) | NOT TESTED |
| Interaction sounds play on E press | NOT TESTED |
| Treatment feedback sounds play | NOT TESTED |
| Timer warning sounds near end | NOT TESTED |
| Mission complete jingle plays | NOT TESTED |
| Mission failed audio plays | NOT TESTED |

---

## Unity Console Errors

| Check | Status |
|-------|--------|
| No Error-level messages during normal gameplay | NOT TESTED |
| No NullReferenceException on scene load | NOT TESTED |
| No missing component warnings | NOT TESTED |
| No shader/material missing errors | NOT TESTED |

---

## Restart / Replay

| Test | Status |
|------|--------|
| Game can be restarted after mission complete | NOT TESTED |
| Game can be restarted after mission failed | NOT TESTED |
| Player position resets on restart | NOT TESTED |
| Score resets on restart | NOT TESTED |
| Timer resets on restart | NOT TESTED |
| All patients reset on restart | NOT TESTED |

---

## Build & Export

| Test | Status |
|------|--------|
| Project builds without errors | NOT TESTED |
| Standalone .exe runs outside Unity | NOT TESTED |
| All assets included in build | NOT TESTED |
| No missing DLL errors on standalone run | NOT TESTED |

---

## Summary

Total Tests: 68
Passed: 0
Failed: 0
Not Tested: 68

*This checklist should be updated after manual testing is performed.*
