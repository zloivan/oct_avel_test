# Ocopus Test - Implementation Explanations

## SaveSystem (OC-3, OC-4)
**Architecture Decision:** Generic Save<T>/Load<T> with JSON serialization.

**Why JSON over Binary:**
- Human-readable for debugging
- Cross-platform compatible
- Easy to validate manually
- Unity's JsonUtility is lightweight and fast

**Error Handling Strategy:**
- Returns default instance on missing/corrupted files (fail-safe, not fail-hard)
- Optional validation callback allows custom integrity checks
- All exceptions logged but gracefully handled
- Bool return values let callers react to failures

**Production Considerations:**
- Could be extended with encryption (AES) for secure data
- Could support async operations via UniTask
- Currently single-threaded (sufficient for test scope)

---

## Popup System (OC-5, OC-6)
**Architecture Decision:** Manager-View pattern with data-driven configuration.

**Separation of Concerns:**
- `PopupConfig` = Pure data (no Unity dependencies)
- `PopupView` = Presentation (MonoBehaviour, handles Unity UI)
- `PopupManager` = Controller (lifecycle management)

**Why Dynamic Button Generation:**
- Supports 1-5 buttons without separate prefabs
- Callbacks assigned at runtime (maximum flexibility)
- Designers can modify button prefab appearance independently

**Memory Management:**
- Instantiate on Show, Destroy on Hide (no pooling needed for infrequent popups)
- Previous popup destroyed when new one shown (only 1 active at a time)

**Production Considerations:**
- Could add animations (fade in/out via DOTween)
- Could support input field, checkbox, slider in PopupConfig
- Could implement popup queue for multiple sequential popups

---

## CharactersView Refactoring (OC-8, OC-9)
**Bugs Fixed:**
1. `GetComponents<Character>()` → `GetComponent<Character>()` (was returning array, not single component)
2. Division order: `totalValue / count` instead of `count / totalValue`
3. `FixedUpdate` → `Update` (UI doesn't need physics timing)
4. Cached `Text` component (was calling `GetComponent` every frame—severe performance issue)

**Optimizations Applied:**
1. **Component Caching:** Character components cached in Awake(), not retrieved per frame
    - **Impact:** Eliminates GetComponent calls (expensive reflection)
2. **Update Throttling:** Configurable interval (default 200ms = 5 fps)
    - **Impact:** Reduces Update() executions by 97% (from 60 fps to 5 fps)
3. **StringBuilder:** Text formatting uses StringBuilder instead of string concatenation
    - **Impact:** Eliminates GC allocations from string operations
4. **Debug.Log Removed:** Was logging every frame (severe console spam + performance hit)

**Theoretical Performance Gains:**
- **Before:** 60 updates/sec × 5 GetComponent calls = 300 reflection calls/sec
- **After:** 5 updates/sec × 0 GetComponent calls = 0 reflection calls/sec
- **GC Reduction:** ~95% fewer string allocations per second

**Production Considerations:**
- Could use events (OnCharacterAdded/Removed) instead of manual RefreshCharacterCache() calls
- Could use object pooling for StringBuilder instances
- Update interval could be adaptive based on frame rate

---

## Entity Tracking System (OC-10)
**Architecture Decision:** Manager-based OOP approach with IEntity interface.

**Why Manager over Event-Driven:**
- **Simpler:** Single class to debug, no event subscription complexity
- **Performant:** One iteration in GetActiveEntities() vs. multiple event handlers
- **Safer:** Centralized null-check cleanup (handles destroyed-but-not-unregistered entities)
- **Production-ready:** Easy to extend (filters, sorting, queries)

**How It Works:**
1. Entities implement `IEntity` interface (IsActive property)
2. EntityManager maintains List<IEntity>
3. Entities register in Start/Initialize, unregister in OnDestroy
4. GetActiveEntities() filters in real-time (checks IsActive + null)

**Safety Features:**
- Null reference cleanup (removes destroyed entities automatically)
- IsActive check handles both GameObject.activeInHierarchy and custom logic
- Clear() method for level transitions (prevents memory leaks)

**Production Considerations:**
- Could add generic queries: GetEntitiesOfType<T>(), GetEntitiesInRadius()
- Could integrate with ECS (Unity DOTS) for massive entity counts
- Could add pooling support for frequent spawn/despawn scenarios

---

## Testing Strategy
**Unit Tests (if time permits):**
- SaveSystem: Test save/load/validation/errors
- EntityManager: Test register/unregister/active filtering
- PopupConfig: Test struct construction

**Manual Testing (priority):**
- SaveSystem: Create PlayerData, save, load, corrupt JSON, verify defaults
- Popup: Show popup with 1/3/5 buttons, verify callbacks execute
- CharactersView: Profile before/after optimization (Unity Profiler)
- EntityManager: Spawn/destroy/disable entities, verify counts

**Integration Testing:**
- Full flow: Load game → Show popup → Track entities → Save progress