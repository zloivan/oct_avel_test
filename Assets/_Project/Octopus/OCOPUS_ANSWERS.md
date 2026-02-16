# Ocopus Test - Answers

## Question 1: Coding Principles for Unity Projects

### Principle 1: Separation of Concerns via Dependency Injection
**Why it matters:**
In projects mixing 3D gameplay, UI systems, and designer iteration, tight coupling creates bottlenecks. A designer changing UI flow shouldn't break gameplay logic, and vice versa.

**Where I apply it:**
- Use DI containers (VContainer/Zenject) to inject dependencies explicitly
- Separate data (ScriptableObjects), logic (services), and presentation (MonoBehaviours)
- Example: `ShopService` handles purchase logic, `ShopUI` only displays and forwards input

**Designer benefit:**
Designers can modify UI layouts, dialog flows, or ScriptableObject configs without touching code. Systems remain testable in isolation.

---

### Principle 2: Addressables-First Content Management
**Why it matters:**
3D games have heavy assets (models, textures, levels). Resources.Load blocks the main thread and bloats builds. Designers need instant iteration without rebuilding.

**Where I apply it:**
- All runtime content (levels, prefabs, configs) goes through Addressables
- Use async loading with progress callbacks for smooth UX
- Group assets by feature (levels, UI, audio) for granular control

**Designer benefit:**
Designers can add/modify/test new levels and assets without programmer involvement. Hot-reload works in Editor. Remote content can be patched post-release.