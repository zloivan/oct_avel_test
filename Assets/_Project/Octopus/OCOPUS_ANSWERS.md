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

## Question 3.1: Unity Components for Popup Prefab

### Components Used:
1. **Canvas** (root)
    - **Why:** Required for all UI rendering. Set to Screen Space - Overlay for consistent display across resolutions.

2. **Canvas Scaler**
    - **Why:** Ensures UI scales properly on different screen sizes/resolutions. Use "Scale with Screen Size" mode with 1920x1080 reference.

3. **Image** (background panel)
    - **Why:** Provides visual container for popup. Semi-transparent to darken background content, improving readability.

4. **TextMeshProUGUI** (title and body)
    - **Why:** Superior text rendering compared to legacy Unity Text. Supports rich text formatting, better performance, and cleaner appearance.

5. **Button** (for each action)
    - **Why:** Built-in component handles click events, hover states, and accessibility. Each button contains a TextMeshProUGUI child for label.

6. **Horizontal/Vertical Layout Group** (button container)
    - **Why:** Automatically arranges 1-5 buttons with consistent spacing, eliminating manual positioning. Adapts to dynamic button count.

7. **Content Size Fitter** (optional, for dynamic sizing)
    - **Why:** Adjusts popup dimensions based on text length, preventing overflow or excessive whitespace.

### Architecture Decision:
Separation of data (PopupConfig) from presentation (PopupView) allows runtime button generation without designer intervention. Designers can modify popup prefab appearance (colors, fonts, spacing) without code changes.