# Gamified Hospital Rescue System

A 3D first-person hospital rescue simulation game built with Unity.

## Project Overview

**Project Title:** Gamified Hospital Rescue System
**Genre:** 3D First-Person Medical Rescue Simulation
**Engine:** Unity (URP)
**Platform:** PC
**Repository:** HospitalRescueSimulator-GitWork

## Project Purpose

This project serves as a coursework submission demonstrating a playable 3D game built in Unity. The game simulates a hospital rescue scenario where the player must navigate a hospital environment, locate patients, assess their symptoms, and apply the correct treatment to stabilize them before time runs out.

## Main Objective

The player must stabilize **5 patients** within the hospital by:
1. Approaching a patient (within 2.5m)
2. Pressing **E** to open the assessment UI
3. Reading the patient's symptoms
4. Selecting the correct treatment from three options
5. Completing treatment before the mission timer expires

Mission succeeds when all 5 patients are stabilized. Mission fails if the timer runs out.

## Gameplay Flow

1. Player spawns in the hospital environment
2. Locate a patient (interaction prompt appears when close)
3. Press **E** to open assessment panel
4. Review symptoms and choose the best treatment from 3 options
5. Correct treatment: patient stabilized, +100 score
6. Wrong treatment: -25 penalty, can retry
7. Repeat for all 5 patients
8. Mission complete when all patients stabilized

## Treatment Types

- Bandage
- Water
- AED (Automated External Defibrillator)
- Oxygen Mask
- Glucose
- First Aid Kit
- Splint

## Project Structure

```
Assets/
├── HospitalHero/
│   ├── Animations/     # Game animations
│   ├── Art/            # Visual art assets
│   ├── Audio/          # Sound assets
│   ├── Materials/      # Hospital floor, wall, accent materials
│   ├── Prefabs/        # Reusable game prefabs
│   ├── Scenes/         # Main game scene (HospitalGame.unity)
│   ├── Scripts/        # Core gameplay scripts
│   └── UI/             # User interface elements
├── PolyPeople_Hospital_Free/  # Hospital staff character assets
├── StarterAssets/             # Unity Starter Assets (First Person)
├── Floreswa/                   # General material assets
└── Furniture/                  # Hospital furniture assets
```

## Core Scripts

| Script | Purpose |
|--------|---------|
| `MissionManager` | Tracks patient progress, total patients (5), triggers win/lose |
| `PatientInteractable` | Handles player proximity detection and E-key interaction |
| `PatientTreatment` | Manages treatment choices, correct/wrong feedback, scoring |
| `AssessmentUIController` | Displays patient symptoms and treatment options |
| `ScoreManager` | Tracks and updates score (+100 correct, -25 wrong) |
| `GameTimer` | Countdown timer, triggers mission failed on expiry |
| `PauseMenu` | In-game pause functionality |
| `MissionCompleteUI` | Shown when all 5 patients stabilized |
| `MissionFailedUI` | Shown when timer expires before completion |

## Controls

| Action | Key |
|--------|-----|
| Move | WASD |
| Look | Mouse |
| Interact | E |
| Pause | Escape |

*Note: Sprint, crouch, and jump controls may exist but require manual verification in-play.*

## Scoring

- Correct treatment: **+100 points**
- Wrong treatment: **-25 points**
- Final score displayed at mission end

## Getting Started

1. Open Unity Hub
2. Open project: `HospitalRescueSimulator-GitWork`
3. Wait for package import and Library regeneration
4. Open scene: `Assets/HospitalHero/Scenes/HospitalGame.unity`
5. Press Play to start the game

## Development Status

- [x] Hospital environment with floor, walls, corridors
- [x] Patient interaction system (E key, proximity-based)
- [x] Assessment UI with symptom display
- [x] Treatment selection with 3 options
- [x] Scoring system (correct/wrong feedback)
- [x] Mission tracking (5 patients to stabilize)
- [x] Mission complete UI
- [x] Mission failed UI (timer expiry)
- [x] Pause menu
- [x] Visual environment polish (materials, lighting)

## License

Proprietary — All rights reserved
