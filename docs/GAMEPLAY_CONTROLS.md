# Gamified Hospital Rescue System — Gameplay Controls

Document version: 1.0
Last verified: August 2026

## Keyboard & Mouse Controls (PC)

| Action | Key | Confirmed |
|--------|-----|----------|
| Move Forward | W | Confirmed |
| Move Backward | S | Confirmed |
| Move Left | A | Confirmed |
| Move Right | D | Confirmed |
| Look Around | Mouse | Confirmed |
| Interact with Patient | E | Confirmed |
| Pause Game | Escape | Confirmed |
| Sprint | Shift | Needs manual verification |
| Jump | Space | Needs manual verification |
| Crouch | Ctrl | Needs manual verification |

## Gamepad Controls

| Action | Button | Confirmed |
|--------|--------|----------|
| Move | D-Pad / Left Stick | Confirmed |
| Look | Right Stick | Confirmed |
| Interact | Button North (Y) | Confirmed |
| Pause | Needs verification | Needs manual verification |

## Interaction System

### Patient Interaction
1. Approach a patient until within **2.5 meters**
2. An interaction prompt will appear when in range
3. Press **E** (keyboard) or **Button North** (gamepad) to open the assessment UI
4. Review the patient's symptoms displayed in the assessment panel
5. Select one of three treatment options
6. Correct treatment stabilizes the patient and awards +100 score
7. Wrong treatment applies a -25 penalty but allows retry

### Assessment UI
- Displayed via `AssessmentUIController.cs`
- Shows patient name, symptoms, and treatment instruction
- Three selectable treatment buttons (Option 1, Option 2, Option 3)
- Feedback text shows result: green for correct, red for incorrect

## Pause Menu
- Press **Escape** to toggle pause
- When paused, the pause panel is displayed
- Pause is disabled automatically during mission-complete sequence

## Confirmed vs Unconfirmed Controls

The following controls are confirmed from source code inspection (`StarterAssetsInputs.cs`, `PatientInteractable.cs`, `PauseMenu.cs`, `InputSystem_Actions.inputactions`):

**Confirmed:**
- WASD movement
- Mouse look
- E key interaction (keyboard)
- Escape for pause

**Needs Manual Verification:**
- Sprint (Shift)
- Jump (Space)
- Crouch (Ctrl)
- Gamepad-specific pause button
- Any secondary interaction keys

## In-Game UI

| Element | Description |
|---------|-------------|
| Stabilized Counter | Shows patients stabilized / total (e.g., "Stabilized: 2/5") |
| Score Display | Current score (updated on correct/wrong treatment) |
| Timer | Countdown timer (mission fails when it reaches zero) |
| Interaction Prompt | Appears near patients when within range |
| Assessment Panel | Opens on E press — shows symptoms and treatment options |
| Mission Complete | Displayed when all 5 patients are stabilized |
| Mission Failed | Displayed when the timer expires |

## Notes

- Controls use Unity's Input System package (new) with fallback to legacy Input Manager
- The `PatientInteractable.cs` script hardcodes `Input.GetKeyDown(KeyCode.E)` for patient interaction
- Pause uses `Input.GetKeyDown(KeyCode.Escape)` from `PauseMenu.cs`
- Movement and look come from `StarterAssetsInputs.cs` via the Starter Assets First Person package
