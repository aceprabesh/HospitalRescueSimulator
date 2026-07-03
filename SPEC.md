# Hospital Rescue Simulator — Technical Specification

## Phase 1: Project Foundation

---

## 1. Project Overview

**Project Name:** Hospital Rescue Simulator
**Engine:** Unity 6 (6000.0.58f2) — URP (Universal Render Pipeline)
**Genre:** 3D First-Person Medical Rescue Simulation
**Platform Target:** PC (Steam), Mid-range gaming laptop optimized
**Core Loop:** Respond to hospital emergencies, stabilize patients, make critical decisions, save lives

---

## 2. Visual Direction

### Art Style
- **Reference Games:** Ready or Not, Phasmophobia (environment), The Mortuary Assistant (lighting)
- **Style:** Realistic, dark, atmospheric hospital environment
- **Rendering:** Physically Based Rendering (PBR), URP
- **Lighting:** Cinematic, high-contrast, volumetric lighting
- **No:** Cartoon graphics, low-poly, voxel art

### Color Palette
| Role | Color | Hex |
|------|-------|-----|
| Primary Dark | Hospital Charcoal | `#1A1D21` |
| Secondary | Steel Blue | `#2C3E50` |
| Accent | Medical Blue | `#00D4FF` |
| Alert/Error | Critical Red | `#FF3B3B` |
| Success | Vital Green | `#00FF88` |
| Warning | Amber | `#FFB800` |
| UI Background | Deep Navy | `#0D1117` |
| Text Primary | Clean White | `#FFFFFF` |
| Text Secondary | Muted Gray | `#8B9298` |

---

## 3. Architecture

### 3.1 Clean Architecture Layers

```
┌─────────────────────────────────────────────────────────┐
│                    Presentation Layer                    │
│  (MonoBehaviours, UI, Controllers, Cameras, Input)     │
├─────────────────────────────────────────────────────────┤
│                    Application Layer                     │
│  (Use Cases, Game Managers, State Machines, Events)     │
├─────────────────────────────────────────────────────────┤
│                     Domain Layer                         │
│  (Entities, Value Objects, Interfaces, Game Data SO)   │
├─────────────────────────────────────────────────────────┤
│                  Infrastructure Layer                    │
│  (Save System, Addressables, Audio, Unity Services)    │
└─────────────────────────────────────────────────────────┘
```

### 3.2 Unity Project Structure

```
HospitalRescueSimulator/
├── Assets/
│   ├── _Project/
│   │   ├── Application/
│   │   │   ├── Controllers/
│   │   │   ├── GameManagers/
│   │   │   ├── UseCases/
│   │   │   └── StateMachines/
│   │   ├── Domain/
│   │   │   ├── Entities/
│   │   │   ├── ValueObjects/
│   │   │   ├── Interfaces/
│   │   │   └── ScriptableObjects/
│   │   ├── Infrastructure/
│   │   │   ├── Persistence/
│   │   │   ├── Audio/
│   │   │   └── Services/
│   │   └── Presentation/
│   │       ├── UI/
│   │       │   ├── Screens/
│   │       │   │   ├── HUD/
│   │       │   │   ├── PauseMenu/
│   │       │   │   ├── GameOver/
│   │       │   │   └── MainMenu/
│   │       │   ├── Components/
│   │       │   │   ├── HealthMonitor/
│   │       │   │   ├── VitalsDisplay/
│   │       │   │   ├── Timer/
│   │       │   │   ├── Objectives/
│   │       │   │   └── Inventory/
│   │       │   └── Styles/
│   │       ├── Cameras/
│   │       ├── Input/
│   │       └── VisualEffects/
│   ├── Art/
│   │   ├── Models/
│   │   │   ├── Characters/
│   │   │   ├── Props/
│   │   │   ├── Furniture/
│   │   │   ├── MedicalEquipment/
│   │   │   └── Architecture/
│   │   ├── Textures/
│   │   ├── Materials/
│   │   ├── Shaders/
│   │   └── Animations/
│   ├── Audio/
│   │   ├── Music/
│   │   ├── SFX/
│   │   └── Voice/
│   ├── Addressables/
│   │   ├── Labels/
│   │   └── Groups/
│   └── Resources/
│
├── ProjectSettings/
├── Packages/
└── UserSettings/
```

---

## 4. Phase 1 Deliverables

### 4.1 Unity Project Configuration
- [ ] Create Unity project with URP template
- [ ] Configure URP settings (shadows, post-processing)
- [ ] Set up editor layout and preferences
- [ ] Configure color space (Linear for PC)
- [ ] Set up Player settings (product name, icon, etc.)

### 4.2 Git Configuration
- [ ] Initialize Git repository
- [ ] Create .gitignore (Unity template)
- [ ] Create initial commit
- [ ] Set up .gitattributes for large files

### 4.3 Coding Standards
- [ ] Namespace conventions: `HospitalRescue.{Layer}.{Feature}`
- [ ] Folder structure enforcement
- [ ] MonoBehaviour lifecycle guidelines
- [ ] Serialization rules
- [ ] Event system patterns

### 4.4 Player Controller
- [ ] First-person character controller
- [ ] WASD + Mouse look
- [ ] Sprint system
- [ ] Crouch system
- [ ] Jump (single jump, realistic)
- [ ] Head bob (subtle)
- [ ] Footstep sounds
- [ ] Collision detection

### 4.5 Camera System
- [ ] First-person camera
- [ ] Mouse look (vertical clamped)
- [ ] FOV settings
- [ ] Camera collision (clip prevention)
- [ ] Weapon/viewmodel idle sway

### 4.6 Interaction System
- [ ] Raycast-based interaction
- [ ] Interaction prompt UI
- [ ] Interactable interface
- [ ] Interaction range configuration
- [ ] Hold-to-interact support

### 4.7 Hospital Blockout
- [ ] Prototype room layouts
- [ ] Basic geometry (no final assets)
- [ ] Room connectivity
- [ ] Basic lighting
- [ ] Navigation mesh (NavMeshSurface)

### 4.8 Lighting Setup
- [ ] URP lighting configuration
- [ ] Real-time lighting basics
- [ ] Ambient lighting
- [ ] Basic Volumetrics

### 4.9 Save/Load Framework
- [ ] SaveData structure
- [ ] JsonSerialization helper
- [ ] PlayerPrefs wrapper (Phase 1 simple)
- [ ] Auto-save checkpoints

### 4.10 Placeholder UI
- [ ] HUD canvas setup
- [ ] Health display placeholder
- [ ] Crosshair
- [ ] Interaction prompt
- [ ] Pause menu (basic)
- [ ] Debug console (dev)

---

## 5. Technical Specifications

### 5.1 Target Performance
| Metric | Target | Minimum |
|--------|--------|---------|
| FPS | 60+ | 45 |
| GPU | Mid-range GTX 1060 / RTX 3050 | Integrated |
| RAM | 8GB | 6GB |
| VRAM | 4GB | 2GB |
| Build Size | < 10GB | - |

### 5.2 Quality Settings
- **Ultra:** Shadows on, Volumetrics on, 2x Texture scale
- **High:** Shadows on, Volumetrics medium
- **Medium:** Soft shadows, No volumetrics
- **Low:** No shadows, No volumetrics, Simplified materials

### 5.3 Optimization Targets
- Occlusion culling enabled
- LOD system for complex meshes
- Baked lighting for static elements
- GPU Instancing for props
- Texture atlases for props
- Object pooling for frequently spawned objects

---

## 6. Dependencies & Packages

### 6.1 Unity Registry
| Package | Version | Purpose |
|---------|---------|---------|
| URP | 6000.0+ | Rendering pipeline |
| Input System | 1.7.0+ | New input handling |
| NavMesh | 6000.0+ | NPC pathfinding |
| Timeline | 6000.0+ | Cinematics |
| Animation Rigging | 6000.0+ | Character animations |
| ProBuilder | 6.0+ | Prototype level design |
| Addressables | 1.21+ | Asset management |
| Visual Studio Editor | 6000.0+ | IDE integration |

### 6.2 Third-Party (TBD Phase 2-3)
- Cinemachine (camera system)
- FMOD (audio engine)
- Odin Inspector (debug tools)
- Quest Machine (NPC dialogue)

---

## 7. Phase 1 Script Architecture

### 7.1 Core Scripts

```
IPlayerController
├── PlayerController : MonoBehaviour, IPlayerController
├── PlayerInput : MonoBehaviour
└── PlayerCamera : MonoBehaviour

IInteractionSystem
├── InteractionManager : MonoBehaviour, IInteractionSystem
├── Interactable : MonoBehaviour, IInteractable
└── InteractionPromptUI : MonoBehaviour

IGameManager
├── GameManager : Singleton MonoBehaviour
├── GameStateMachine : StateMachine
├── PauseManager : MonoBehaviour
└── SaveManager : MonoBehaviour, ISaveManager
```

### 7.2 Folder Structure for Scripts
```
Assets/_Project/Application/Controllers/Player/
├── PlayerController.cs
├── PlayerInput.cs
├── PlayerCamera.cs
└── PlayerStateMachine.cs

Assets/_Project/Application/GameManagers/
├── GameManager.cs
├── GameStateMachine.cs
├── PauseManager.cs
├── SaveManager.cs
├── AudioManager.cs
└── UIManager.cs

Assets/_Project/Domain/Interfaces/
├── IPlayerController.cs
├── IInteractionSystem.cs
├── IGameManager.cs
├── ISaveManager.cs
└── IAudioManager.cs

Assets/_Project/Domain/Entities/
├── PlayerData.cs
├── PatientData.cs
└── MissionData.cs

Assets/_Project/Domain/ScriptableObjects/
├── GameSettings.cs
├── PlayerSettings.cs
├── InputSettings.cs
└── InteractionSettings.cs
```

---

## 8. Blender Asset Pipeline (Phase 2+)

### 8.1 Naming Conventions
- Models: `{Category}_{SubCategory}_{Name}_{Variant}.fbx`
- Textures: `{ModelName}_{Channel}_{Resolution}.png`
- Materials: `{ModelName}_Mat`
- Prefabs: `{ModelName}_Pf`

### 8.2 Hospital Modular Kit (Phase 2)
| Component | Dimensions | Poly Budget | Purpose |
|-----------|-------------|-------------|---------|
| Wall_Straight_4m | 4m x 3m x 0.2m | 64 tris | Standard wall |
| Wall_Corner | 4m x 3m x 0.2m | 128 tris | Corner connections |
| Floor_Tile_2m | 2m x 2m x 0.05m | 8 tris | Floor tiles |
| Door_Single | 1m x 2.1m x 0.1m | 256 tris | Doorways |
| Door_Double | 2m x 2.1m x 0.1m | 384 tris | Main corridors |

---

## 9. UI/UX Design System

### 9.1 Typography
| Element | Font | Size | Weight |
|---------|------|------|--------|
| HUD Title | Inter | 24px | Bold |
| HUD Body | Inter | 16px | Regular |
| Timer | JetBrains Mono | 32px | Bold |
| Vitals | JetBrains Mono | 20px | Medium |
| Button Text | Inter | 14px | SemiBold |

### 9.2 Spacing System (8pt Grid)
- `xs`: 4px
- `sm`: 8px
- `md`: 16px
- `lg`: 24px
- `xl`: 32px
- `xxl`: 48px

### 9.3 Component States
| State | Background | Border | Text |
|-------|------------|--------|------|
| Default | #1A1D21 | #2C3E50 | #FFFFFF |
| Hover | #2C3E50 | #00D4FF | #FFFFFF |
| Active | #00D4FF | #00D4FF | #0D1117 |
| Disabled | #1A1D21 | #1A1D21 | #4A4A4A |

---

## 10. Risk Assessment

| Risk | Likelihood | Impact | Mitigation |
|------|-------------|--------|------------|
| Performance issues | Medium | High | Early profiling, LOD system, occlusion |
| Scope creep | High | High | Strict phase gates, MVP focus |
| Asset pipeline bottlenecks | Medium | Medium | Clear Blender workflow, import automation |
| Save system complexity | Low | High | Simple JSON initially, expand later |

---

## 11. Next Steps (Upon Approval)

1. Initialize Unity project with URP
2. Configure Git repository
3. Set up folder structure
4. Implement Player Controller
5. Implement Interaction System
6. Build hospital blockout
7. Add placeholder UI
8. Integrate save/load framework
9. **Deliverable:** Playable prototype

---

*Document Version: 1.0*
*Last Updated: 2026-07-02*
*Phase: 1 - Foundation*
