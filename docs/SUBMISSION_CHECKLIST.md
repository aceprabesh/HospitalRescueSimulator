# Gamified Hospital Rescue System — Final Demo & Submission Checklist

Document version: 1.0
Date: August 2026

---

## Pre-Submission Checklist

### Project Download & Setup
- [ ] Clone or download repository from https://github.com/aceprabesh/HospitalRescueSimulator
- [ ] Open the `HospitalRescueSimulator-GitWork` folder in Unity Hub
- [ ] Wait for Unity to regenerate the Library folder (first open only)
- [ ] Confirm Unity version compatibility

### Unity Project Verification
- [ ] Open `HospitalGame.unity` scene (Assets/HospitalHero/Scenes/)
- [ ] Press Play to start the game
- [ ] Confirm scene loads without errors
- [ ] Check Console for any Error-level messages
- [ ] Confirm no missing prefab/material warnings

### Gameplay Verification
- [ ] Player spawns correctly in hospital environment
- [ ] WASD movement works
- [ ] Mouse look works
- [ ] Patient interaction prompt appears (within 2.5m of patient)
- [ ] E key opens assessment UI
- [ ] Treatment options display correctly
- [ ] Correct/wrong feedback displays
- [ ] Score updates (+100 / -25)
- [ ] Stabilized counter increments
- [ ] Timer counts down
- [ ] Mission complete screen appears after all 5 patients
- [ ] Mission failed screen appears when timer expires
- [ ] Pause menu opens with Escape

### GitHub Verification
- [ ] All commits visible on GitHub repository
- [ ] Commit history shows meaningful progress
- [ ] README.md is accurate and current
- [ ] No sensitive data committed

### Trello Verification
- [ ] Trello board exists for project
- [ ] Cards reflect actual work done
- [ ] Screenshots attached to relevant cards
- [ ] Cards moved to "Done" where applicable

### Screenshot Capture
- [ ] Hospital environment screenshot
- [ ] Player movement screenshot
- [ ] Patient interaction screenshot
- [ ] Assessment UI screenshot
- [ ] Mission complete screenshot
- [ ] Mission failed screenshot
- [ ] Trello board screenshot
- [ ] GitHub commit history screenshot

### Backup
- [ ] Keep a backup of the entire project folder
- [ ] Keep a backup of the git bundle (if created)
- [ ] Keep a backup of screenshots

---

## Demonstration Flow

Follow these steps during coursework demonstration:

### 1. Open Project
1. Open Unity Hub
2. Select `HospitalRescueSimulator-GitWork`
3. Wait for project to fully load
4. Open scene: `Assets/HospitalHero/Scenes/HospitalGame.unity`

### 2. Start Game
1. Press the **Play** button in Unity
2. Wait for scene to initialize
3. Confirm player spawns in hospital

### 3. Demonstrate Player Movement
1. Use **WASD** to move forward, back, left, right
2. Use **mouse** to look around
3. Navigate through the hospital corridor

### 4. Show Hospital Environment
1. Pan camera to show walls
2. Show floor materials
3. Show entrance/pavement area
4. Point out visual style improvements (color, lighting)

### 5. Demonstrate Rescue Objective
1. Move toward a patient in the scene
2. Wait for interaction prompt to appear
3. Press **E** to open assessment UI
4. Read the patient symptoms aloud
5. Select the treatment option
6. Show correct/wrong feedback (green/red)
7. If wrong, retry until correct

### 6. Show Relevant UI
1. Point out stabilized counter (e.g., "Stabilized: 1/5")
2. Point out score display
3. Point out timer
4. Show pause menu (press **Escape**)

### 7. Complete Mission
1. Treat remaining patients (steps 5-6)
2. Show counter incrementing
3. Show mission complete screen when all 5 stabilized

### 8. Show GitHub History
1. Open browser to https://github.com/aceprabesh/HospitalRescueSimulator
2. Show commit history
3. Point out meaningful commit messages
4. Show the 5 visual-polish commits
5. Show the 5 documentation commits

### 9. Show Trello Board
1. Open Trello board for the project
2. Show task cards
3. Show attached screenshots
4. Show completed vs in-progress vs to-do

### 10. Final Submission Check
1. Confirm all files are on GitHub
2. Confirm Unity project is playable
3. Confirm all documentation is present
4. Confirm screenshots are captured and attached
5. Confirm backup is saved

---

## File Inventory (Submission Package)

| File/Folder | Description | Required |
|------------|------------|----------|
| `README.md` | Project overview and getting started | Yes |
| `docs/GAMEPLAY_CONTROLS.md` | Controls documentation | Yes |
| `docs/QA_TESTING.md` | QA testing checklist | Yes |
| `docs/DEVELOPMENT_EVIDENCE.md` | Development evidence log | Yes |
| `docs/SUBMISSION_CHECKLIST.md` | Final demo and submission checklist | Yes |
| `Assets/HospitalHero/Scenes/HospitalGame.unity` | Main playable scene | Yes |
| `Assets/HospitalHero/Scripts/` | Core gameplay scripts | Yes |
| Screenshots folder | Evidence screenshots | Yes |
| `HospitalRescueSimulator-GitWork/` | Complete Unity project | Yes |

---

## Key Commit Messages (for reference)

```
docs: improve hospital rescue project overview
docs: add gameplay controls guide
test: add hospital rescue gameplay QA checklist
docs: add coursework development evidence log
docs: add final demo and submission checklist
style: polish hospital environment details
style: improve hospital entrance pavement detail
style: improve hospital interior lighting
style: refine hospital wall colors
style: improve hospital floor appearance
```

---

## Notes

- Do NOT modify gameplay scripts, player controller, or game mechanics during demonstration
- Keep the game in a clean, playable state for demo
- Ensure Unity Console shows no errors before demo
- Have screenshots ready and named clearly
- Know the key commit hashes in case they are needed
