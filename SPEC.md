# Hospital Rescue Simulator — Spec

## What it is

A first-person 3D game made in Unity for coursework. You play as someone
walking around a hospital, finding patients, and treating them correctly
before the clock runs out.

Engine: Unity (URP)
Platform: PC, built for a normal laptop, nothing fancy required.

## The core loop

1. Walk around the hospital and find a patient.
2. Get close enough (about 2.5m) and an interaction prompt shows up.
3. Press E to open the assessment screen.
4. Read the patient's symptoms.
5. Pick one of three treatment options.
6. Correct choice = patient stabilized, +100 score. Wrong choice = -25,
   try again.

Do that for all 5 patients before the timer runs out and you win. If the
timer hits zero first, you fail the mission.

## Treatments

Bandage, Water, AED, Oxygen Mask, Glucose, First Aid Kit, Splint — the
patient's symptoms decide which one is actually correct.

## Controls

- WASD to move
- Mouse to look
- E to interact
- Escape to pause

## Scripts (roughly what each one does)

- `MissionManager` — keeps count of how many patients are stabilized,
  decides when the mission is won or lost
- `PatientInteractable` — detects when the player is close enough to a
  patient and listens for the E key
- `PatientTreatment` — handles the treatment choice and whether it was
  right or wrong
- `AssessmentUIController` — shows the symptoms and the three treatment
  buttons
- `ScoreManager` — adds/subtracts points
- `GameTimer` — counts down, fails the mission if it hits zero
- `PauseMenu`, `MissionCompleteUI`, `MissionFailedUI` — the obvious ones

## What actually got built vs. the original plan

Early on this had a much bigger plan attached to it — modular architecture
layers, a full asset pipeline, third-party audio middleware, that kind of
thing. Most of that got dropped once real development started, because it
was more than a coursework-sized project needed. What's in the repo now is
the simpler version above: one hospital scene, 5 patients, one interaction
loop, done properly rather than half-building a bigger system.

## Known limitations

- Sprint/crouch/jump inputs may be wired up from the base Unity starter
  assets but aren't guaranteed to be tuned or required for the mission
- No save/load between sessions — each playthrough starts fresh
- Visual polish (materials, lighting) was done in later passes and isn't
  meant to look production-quality, just presentable for a demo
