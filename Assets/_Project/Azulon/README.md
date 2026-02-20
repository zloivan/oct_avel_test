# Azulon — Shop & Inventory System

A data-driven Shop + Inventory feature built without third-party DI frameworks, demonstrating clean layered architecture and a gamification mechanic.

---

## Architecture Overview

```
[Bootstrapper]  ← single composition root
      │
      ├── ScriptableObjectItemRepository  (IItemRepository)
      ├── CurrencyService                 (ICurrencyService)
      ├── InventoryService                (IInventoryService)
      ├── ShopService                     (IShopService)
      │
      ├── PopupManager
      │
      ├── UIStateMachine
      │       ├── ShopState      → ShopPresenter
      │       ├── InventoryState → InventoryPresenter
      │       └── RewardState    → RewardPresenter
      │
      ├── ShopPresenter
      ├── InventoryPresenter
      └── RewardPresenter
```

**Dependency rules:**
- **Core** (services) has no knowledge of UI
- **Presenters** talk to Core only through interfaces
- **Views** (MonoBehaviour) know nothing beyond their own display
- **Bootstrapper** is the only class allowed to know everything

---

## Layer Breakdown

### Data Layer

| Class | Type | Responsibility |
|---|---|---|
| `ItemDataSO` | ScriptableObject | Stores a single item: auto-generated GUID, name, description, icon, price |
| `CurrencyConfigSO` | ScriptableObject | Designer-editable starting balance |
| `CurrencyData` / `InventoryData` | Serializable class | DTOs for SaveSystem; inventory stores `List<string>` (GUIDs) because `JsonUtility` can't serialize Unity Object references |

---

### Core Layer (Services)

| Class | Responsibility |
|---|---|
| `IItemRepository` / `ScriptableObjectItemRepository` | Provides items. `GetAll()` returns full list; `GetById()` resolves in O(1) via Dictionary |
| `ICurrencyService` / `CurrencyService` | Manages balance. `TrySpend()` returns bool to prevent partial state. Fires `OnAmountChanged` event. Persists via SaveSystem |
| `IInventoryService` / `InventoryService` | Tracks owned item IDs (`List<string>`). Fires `OnInventoryChanged`. Persists via SaveSystem |
| `IShopService` / `ShopService` | Single place where Currency and Inventory interact. `TryPurchase()` is an atomic transaction: check owned → check funds → add item |
| `PurchaseResult` | Enum (`Success`, `NotEnoughCurrency`, `AlreadyOwned`) — lets Presenter show the correct message |
| `SaveSystem` | Generic `Save<T>` / `Load<T>` over `JsonUtility`. Fail-safe: returns `default` on missing or corrupted file |

---

### UI Layer

| Class | Responsibility |
|---|---|
| `IUIState` / `UIStateMachine` | Controls which screen is active. `SwitchTo<T>()` calls `Exit()` on current, `Enter()` on next. States stored in `Dictionary<Type, IUIState>` — no switch-case, adding a screen = new class only |
| `ShopState` / `InventoryState` / `RewardState` | Thin wrappers: `Enter()` → `presenter.Enable()`, `Exit()` → `presenter.Disable()` |
| `ShopPresenter` | Subscribes to service events, feeds data to `ShopScreenView`, delegates purchase to `ShopService`, shows result via `PopupManager` |
| `InventoryPresenter` | Subscribes to `OnInventoryChanged`, resolves GUIDs → `ItemData` via `IItemRepository`, updates `InventoryScreenView` |
| `RewardPresenter` | Drives the timing-bar mini-game, calls `CurrencyService.Add()` on claim |
| `PopupManager` | Instantiates/destroys `PopupViewUI`. One popup at a time. Accepts `PopupConfig` (title, message, buttons with callbacks) |

---

### Views (MonoBehaviour)

| Class | Responsibility |
|---|---|
| `ShopScreenView` | Renders item list, currency display. Exposes Unity events: `OnPurchaseRequested`, `OnInventoryRequested`, `OnEarnCurrencyRequested` |
| `InventoryScreenView` | Renders owned items in a grid. Exposes `OnShopRequested` |
| `RewardScreenView` | Animates the timing-bar marker. Exposes `OnRewardClaimed(int amount)` |
| `PopupViewUI` | Renders title, message, and a dynamic list of buttons (1–5) |
| `ShopItemView` | Single item card: icon, name, price, Buy button |
| `InventoryItemView` | Single owned-item card: icon, name |

---

## Gamification Mechanic — Timing Bar

The player earns currency by pressing **Claim** while a marker is inside a highlighted green zone:

- **Green zone hit** → full reward (100 coins)
- **Miss** → partial reward (25 coins)

Accessible via the **Earn Coins** button in the Shop. Reward is applied through `CurrencyService.Add()` and immediately reflected in the Shop balance.

---

## Folder Structure

```
Assets/_Project/Azulon/
├── Data/
│   ├── Items/          ← ItemDataSO assets
│   └── Configs/        ← CurrencyConfigSO
├── Scripts/
│   ├── Bootstrap/      ← Bootstrapper
│   ├── Core/
│   │   ├── Currency/
│   │   ├── Inventory/
│   │   ├── Items/
│   │   └── Shop/
│   ├── SaveData/
│   └── UI/
│       ├── Popups/
│       ├── Presenters/
│       ├── UIStates/
│       └── Views/
├── Prefabs/UI/
└── Scenes/
```