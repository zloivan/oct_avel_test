# Optional Bonus - Scaling, Designer Interaction, Profiling

## Scaling These Systems for Larger Projects

### SaveSystem
- **Current:** Synchronous JSON, single file per save
- **Scale to 100+ save files:**
    - Switch to async I/O (UniTask)
    - Implement save metadata index (avoid loading all saves to list)
    - Add compression (GZip) for large data
- **Scale to multiplayer:**
    - Add encryption (AES)
    - Version management (migrations for schema changes)
    - Cloud sync integration (PlayFab, Firebase)

### Popup System
- **Current:** Single popup, instantiate on demand
- **Scale to 50+ popup types:**
    - Popup pooling (avoid instantiate/destroy spam)
    - Priority queue (critical popups interrupt non-critical)
    - Addressables integration (load popup prefabs on demand)
- **Scale to complex flows:**
    - Popup stack (back button navigates history)
    - Async/await API: `await popupManager.ShowAsync<bool>(config)`

### CharactersView
- **Current:** Manual Update throttling, cached components
- **Scale to 1000+ characters:**
    - Event-driven updates (only update when character state changes)
    - Spatial partitioning (only track visible characters)
    - ECS (Unity DOTS) for massive entity counts
- **Scale to multiple views:**
    - Extract logic to `CharacterStatsCalculator` service
    - Views subscribe to calculator events

### EntityManager
- **Current:** List-based, O(n) active filtering
- **Scale to 10,000+ entities:**
    - Separate active/inactive lists (O(1) filtering)
    - Spatial hash grid for proximity queries
    - Integrate with Unity ECS for massive parallelization
- **Scale to networked:**
    - Add entity ownership tracking (client/server authority)
    - Replicate Register/Unregister via network messages

---

## How Designers Interact with This Code

### SaveSystem
**Designer Impact:** Zero code interaction
- **Config:** Designers create ScriptableObjects for default save data
- **Tools:** Custom editor window: "Save Editor" (view/edit saves in Inspector)
- **Validation:** Define validation rules in ScriptableObjects (e.g., "max level = 100")

### Popup System
**Designer Impact:** UI prefab editing only
- **Flow:** Designers modify `PopupView` prefab (fonts, colors, layouts)
- **Content:** Localization keys in ScriptableObjects (no hardcoded text)
- **Testing:** Custom editor button: "Preview Popup" (shows popup in Scene view)

### CharactersView
**Designer Impact:** Minimal
- **Setup:** Designers drag character transforms into Inspector list
- **Tuning:** `_updateEveryNFrames` exposed in Inspector (adjust performance vs. responsiveness)
- **Debugging:** Add `[ContextMenu("Show Stats")]` for instant feedback

### EntityManager
**Designer Impact:** Indirect (via entity prefabs)
- **Workflow:** Designers create entity prefabs with `GameplayEntity` component
- **Tools:** Custom Gizmos draw entity states (active = green, inactive = red)
- **Debugging:** Inspector shows live entity count

**General Pattern:**
- Code = logic, ScriptableObjects = data
- Designers modify data, not code
- Custom Editor tools bridge gap (preview, validation, debugging)

---

## How to Profile and Debug Performance

### General Unity Profiling
1. **Deep Profile Mode:** Window → Analysis → Profiler → Deep Profile
    - **Use when:** Investigating Update() performance
    - **Focus:** CharactersView.UpdateDisplay(), EntityManager.GetActiveEntities()
2. **Memory Profiler:** Package Manager → Memory Profiler
    - **Track:** GC allocations from SaveSystem, StringBuilder usage
3. **Frame Debugger:** Window → Analysis → Frame Debugger
    - **Check:** PopupView UI batching (should be single draw call per popup)

### System-Specific Profiling

#### SaveSystem
**Issue:** Save lag on large files
- **Tool:** Stopwatch
```csharp
  var sw = System.Diagnostics.Stopwatch.StartNew();
  saveSystem.Save("bigfile", data);
  Debug.Log($"Save took {sw.ElapsedMilliseconds}ms");
```
- **Fix if > 100ms:** Switch to async, compress data

**Issue:** Memory spike on Load
- **Tool:** Memory Profiler (snapshot before/after Load)
- **Fix:** Streaming deserialization (read JSON in chunks)

#### Popup System
**Issue:** Instantiate lag
- **Tool:** Profiler → "Instantiate" markers
- **Fix if > 16ms:** Object pooling

**Issue:** Too many active popups (memory leak)
- **Debug:** `PopupManager._currentPopup` inspector
- **Fix:** Add `[SerializeField] private int _maxConcurrentPopups = 1`

#### CharactersView
**Issue:** Update() taking > 2ms
- **Tool:** Profiler → CPU Usage → CharactersView.Update
- **Isolate:**
```csharp
  UnityEngine.Profiling.Profiler.BeginSample("CharactersView.Update");
  UpdateDisplay();
  UnityEngine.Profiling.Profiler.EndSample();
```
- **Optimize:** Increase `_updateEveryNFrames` or switch to event-driven

**Issue:** GC allocations from StringBuilder
- **Tool:** Profiler → GC Alloc column
- **Fix:** Reuse single StringBuilder instance (cache in class field)

#### EntityManager
**Issue:** GetActiveEntities() slow with 1000+ entities
- **Tool:** Profiler + manual timing
```csharp
  var sw = Stopwatch.StartNew();
  var active = entityManager.GetActiveEntities();
  Debug.Log($"Query took {sw.ElapsedMilliseconds}ms for {active.Count} entities");
```
- **Fix if > 5ms:** Maintain separate active/inactive lists

**Issue:** Memory leak (entities not unregistered)
- **Debug:** `EntityManager._allEntities.Count` after level unload
- **Fix:** Add `EntityManager.Clear()` to scene transition

### Custom Debug Tools

#### SaveSystem Debug Window
```csharp
// EditorWindow showing:
// - All save files (with size, timestamp)
// - "Validate All" button (runs validators)
// - "Delete All" button
```

#### Entity Debug Overlay
```csharp
// OnGUI overlay showing:
// - Active entity count
// - Registered vs. actual count (detect leaks)
// - List of active entities (click to ping in Hierarchy)
```

#### Performance Budget Display
```csharp
// In-game overlay:
// - CharactersView update time
// - EntityManager query time
// - Current FPS
// - GC allocations per frame
```

---

## Key Takeaway
**Production profiling = measure first, optimize second.**
Use Unity's built-in tools (Profiler, Memory Profiler) as primary source of truth, then apply system-specific fixes based on data, not assumptions.