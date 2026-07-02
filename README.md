# Hospital Rescue Simulator

A premium 3D first-person medical rescue simulation game built with Unity 6.

## Project Overview

**Genre:** 3D First-Person Medical Rescue Simulation
**Engine:** Unity 6000.0 (URP)
**Platform:** PC (Steam)
**Target:** Mid-range gaming laptop, 60+ FPS

## Development Phases

### Phase 1: Project Foundation (Current)
- [x] Unity project structure
- [x] URP configuration
- [x] Player controller system
- [x] Interaction system
- [x] Hospital blockout generator
- [x] Save/Load framework
- [x] Placeholder UI

### Phase 2: Core Gameplay
- Detailed hospital models
- Inventory system
- Patient system
- Emergency scenarios
- NPC AI

### Phase 3: Advanced Systems
- Full story implementation
- Audio integration
- Performance optimization

## Controls

| Action | Key |
|--------|-----|
| Move | WASD |
| Look | Mouse |
| Sprint | Shift |
| Crouch | Ctrl |
| Jump | Space |
| Interact | E |
| Pause | Escape |

## Architecture

```
Assets/_Project/
├── Application/     # Controllers, GameManagers, UseCases
├── Domain/          # Entities, Interfaces, ScriptableObjects
├── Infrastructure/  # Persistence, Audio, Services
└── Presentation/    # UI, Cameras, Input, VFX
```

## Getting Started

1. Open Unity Hub
2. Click "Open" and select the `HospitalRescueSimulator` folder
3. Wait for package import
4. Open the bootstrap scene and press Play

## Documentation

See `SPEC.md` for complete technical specification.

## License

Proprietary - All rights reserved
