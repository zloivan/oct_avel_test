using UnityEngine;
using Azylon.Currency;
using Azylon.Inventory;
using Azylon.SaveData;
using Azylon.UI;
using Azylon.UI.Presenters;
using Azylon.UI.UIStates;

namespace Azylon.Testing
{
    public class AzylonTestingScript : MonoBehaviour
    {
        [SerializeField] private CurrencyConfigSO _currencyConfig;
        
        private InventoryService _inventoryService;
        private CurrencyService _currencyService;
        private SaveSystem _saveSystem;
        private UIStateMachine _uiStateMachine;

        private void Start()
        {
            // Initialize services
            _saveSystem = new SaveSystem();
            _inventoryService = new InventoryService(_saveSystem);
            _currencyService = new CurrencyService(_currencyConfig, _saveSystem);
            
            // Initialize UI State Machine with real states
            var inventoryPresenter = new InventoryPresenter();
            //var shopPresenter = new ShopPresenter();//TODO ADD ACTUAL TESTING
            var rewardPresenter = new RewardPresenter();

            var uiStates = new IUIState[]
            {
                new InventoryState(inventoryPresenter),
                //new ShopState(shopPresenter),//TODO ADD ACTUAL TESTING
                new RewardState(rewardPresenter)
            };
            _uiStateMachine = new UIStateMachine(uiStates);
            
            // Subscribe to events
            _inventoryService.OnInventoryChanged += () => 
                Debug.Log("[InventoryService] Inventory changed!");
            
            _currencyService.OnAmountChanged += (amount) => 
                Debug.Log($"[CurrencyService] Amount changed to: {amount}");
            
            Debug.Log("=== Testing Script Initialized ===");
            Debug.Log("Keyboard Controls:");
            Debug.Log("--- Inventory ---");
            Debug.Log("1: Add Sword");
            Debug.Log("2: Add Shield");
            Debug.Log("3: Add Potion");
            Debug.Log("4: Remove Sword");
            Debug.Log("5: Remove Shield");
            Debug.Log("6: Check if has Sword");
            Debug.Log("7: List all items");
            Debug.Log("--- Currency ---");
            Debug.Log("Q: Add 50 currency");
            Debug.Log("W: Add 100 currency");
            Debug.Log("E: Spend 25 currency");
            Debug.Log("R: Spend 50 currency");
            Debug.Log("T: Spend 100 currency");
            Debug.Log("Y: Get current amount");
            Debug.Log("--- UI State Machine ---");
            Debug.Log("U: Show Inventory");
            Debug.Log("I: Show Shop");
            Debug.Log("O: Show Reward");
            Debug.Log("P: Show Inventory again (repeat test)");
            Debug.Log("--- Other ---");
            Debug.Log("C: Clear all saves");
            Debug.Log("=================================\n");
        }

        private void Update()
        {
            // Inventory controls
            if (Input.GetKeyDown(KeyCode.Alpha1))
                AddItemTest("sword_001");
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
                AddItemTest("shield_001");
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
                AddItemTest("potion_001");
            
            if (Input.GetKeyDown(KeyCode.Alpha4))
                RemoveItemTest("sword_001");
            
            if (Input.GetKeyDown(KeyCode.Alpha5))
                RemoveItemTest("shield_001");
            
            if (Input.GetKeyDown(KeyCode.Alpha6))
                CheckItemTest("sword_001");
            
            if (Input.GetKeyDown(KeyCode.Alpha7))
                ListAllItemsTest();
            
            // Currency controls
            if (Input.GetKeyDown(KeyCode.Q))
                AddCurrencyTest(50);
            
            if (Input.GetKeyDown(KeyCode.W))
                AddCurrencyTest(100);
            
            if (Input.GetKeyDown(KeyCode.E))
                SpendCurrencyTest(25);
            
            if (Input.GetKeyDown(KeyCode.R))
                SpendCurrencyTest(50);
            
            if (Input.GetKeyDown(KeyCode.T))
                SpendCurrencyTest(100);
            
            if (Input.GetKeyDown(KeyCode.Y))
                GetCurrencyAmountTest();
            
            // UI State Machine controls
            if (Input.GetKeyDown(KeyCode.U))
                SwitchToStateTest<InventoryState>();

            if (Input.GetKeyDown(KeyCode.I))
                SwitchToStateTest<ShopState>();

            if (Input.GetKeyDown(KeyCode.O))
                SwitchToStateTest<RewardState>();

            if (Input.GetKeyDown(KeyCode.P))
                SwitchToStateTest<InventoryState>(); // Repeat test
            
            // Clear saves
            if (Input.GetKeyDown(KeyCode.C))
                ClearSavesTest();
        }

        // ===== INVENTORY TESTS =====
        
        [ContextMenu("Inventory/Add Sword")]
        private void AddSword() => AddItemTest("sword_001");
        
        [ContextMenu("Inventory/Add Shield")]
        private void AddShield() => AddItemTest("shield_001");
        
        [ContextMenu("Inventory/Add Potion")]
        private void AddPotion() => AddItemTest("potion_001");
        
        [ContextMenu("Inventory/Add Helmet")]
        private void AddHelmet() => AddItemTest("helmet_001");
        
        [ContextMenu("Inventory/Add Armor")]
        private void AddArmor() => AddItemTest("armor_001");
        
        [ContextMenu("Inventory/Remove Sword")]
        private void RemoveSword() => RemoveItemTest("sword_001");
        
        [ContextMenu("Inventory/Remove Shield")]
        private void RemoveShield() => RemoveItemTest("shield_001");
        
        [ContextMenu("Inventory/Remove Potion")]
        private void RemovePotion() => RemoveItemTest("potion_001");
        
        [ContextMenu("Inventory/Check Has Sword")]
        private void CheckHasSword() => CheckItemTest("sword_001");
        
        [ContextMenu("Inventory/Check Has Shield")]
        private void CheckHasShield() => CheckItemTest("shield_001");
        
        [ContextMenu("Inventory/List All Items")]
        private void ListAllItems() => ListAllItemsTest();
        
        // ===== CURRENCY TESTS =====
        
        [ContextMenu("Currency/Add 10")]
        private void AddCurrency10() => AddCurrencyTest(10);
        
        [ContextMenu("Currency/Add 50")]
        private void AddCurrency50() => AddCurrencyTest(50);
        
        [ContextMenu("Currency/Add 100")]
        private void AddCurrency100() => AddCurrencyTest(100);
        
        [ContextMenu("Currency/Add 500")]
        private void AddCurrency500() => AddCurrencyTest(500);
        
        [ContextMenu("Currency/Spend 10")]
        private void SpendCurrency10() => SpendCurrencyTest(10);
        
        [ContextMenu("Currency/Spend 25")]
        private void SpendCurrency25() => SpendCurrencyTest(25);
        
        [ContextMenu("Currency/Spend 50")]
        private void SpendCurrency50() => SpendCurrencyTest(50);
        
        [ContextMenu("Currency/Spend 100")]
        private void SpendCurrency100() => SpendCurrencyTest(100);
        
        [ContextMenu("Currency/Spend 1000 (Should Fail)")]
        private void SpendCurrency1000() => SpendCurrencyTest(1000);
        
        [ContextMenu("Currency/Get Current Amount")]
        private void GetCurrentAmount() => GetCurrencyAmountTest();
        
        // ===== UI STATE MACHINE TESTS =====

        [ContextMenu("UI State Machine/Show Inventory")]
        private void ShowInventory() => SwitchToStateTest<InventoryState>();

        [ContextMenu("UI State Machine/Show Shop")]
        private void ShowShop() => SwitchToStateTest<ShopState>();

        [ContextMenu("UI State Machine/Show Reward")]
        private void ShowReward() => SwitchToStateTest<RewardState>();
        
        [ContextMenu("UI State Machine/Test State Transitions")]
        private void TestStateTransitions()
        {
            Debug.Log("\n=== TESTING UI STATE MACHINE ===\n");

            Debug.Log("--- Test 1: Show Inventory ---");
            SwitchToStateTest<InventoryState>();

            Debug.Log("\n--- Test 2: Show Shop (should Exit Inventory, Enter Shop) ---");
            SwitchToStateTest<ShopState>();

            Debug.Log("\n--- Test 3: Show Reward (should Exit Shop, Enter Reward) ---");
            SwitchToStateTest<RewardState>();

            Debug.Log("\n--- Test 4: Show Inventory again (should Exit Reward, Enter Inventory) ---");
            SwitchToStateTest<InventoryState>();

            Debug.Log("\n--- Test 5: Repeat Inventory (should Exit Inventory, Enter Inventory again) ---");
            SwitchToStateTest<InventoryState>();

            Debug.Log("\n=== UI STATE MACHINE TEST COMPLETE ===\n");
        }
        
        // ===== GENERAL TESTS =====
        
        [ContextMenu("General/Clear All Saves")]
        private void ClearAllSaves() => ClearSavesTest();
        
        [ContextMenu("General/Run Full Test Suite")]
        private void RunFullTestSuite()
        {
            Debug.Log("\n=== RUNNING FULL TEST SUITE ===\n");
            
            // Inventory tests
            Debug.Log("--- Testing Inventory Service ---");
            AddItemTest("sword_001");
            AddItemTest("shield_001");
            AddItemTest("potion_001");
            AddItemTest("sword_001"); // Duplicate test
            CheckItemTest("sword_001");
            CheckItemTest("nonexistent_item");
            ListAllItemsTest();
            RemoveItemTest("shield_001");
            RemoveItemTest("nonexistent_item"); // Remove non-existent
            ListAllItemsTest();
            
            Debug.Log("\n--- Testing Currency Service ---");
            GetCurrencyAmountTest();
            AddCurrencyTest(100);
            AddCurrencyTest(50);
            SpendCurrencyTest(75);
            SpendCurrencyTest(1000); // Should fail
            GetCurrencyAmountTest();
            
            Debug.Log("\n--- Testing UI State Machine ---");
            TestStateTransitions();
            
            Debug.Log("\n=== TEST SUITE COMPLETE ===\n");
        }
        
        // ===== HELPER METHODS =====
        
        private void AddItemTest(string itemId)
        {
            Debug.Log($"[TEST] Adding item: {itemId}");
            _inventoryService.AddItem(itemId);
            Debug.Log($"[TEST] Item added. Total items: {_inventoryService.OwnedItemsIdList().Count}");
        }
        
        private void RemoveItemTest(string itemId)
        {
            Debug.Log($"[TEST] Removing item: {itemId}");
            bool hadItem = _inventoryService.HasItem(itemId);
            _inventoryService.RemoveItem(itemId);
            Debug.Log($"[TEST] Item removed. Had item before: {hadItem}. Total items: {_inventoryService.OwnedItemsIdList().Count}");
        }
        
        private void CheckItemTest(string itemId)
        {
            bool hasItem = _inventoryService.HasItem(itemId);
            Debug.Log($"[TEST] Has item '{itemId}': {hasItem}");
        }
        
        private void ListAllItemsTest()
        {
            var items = _inventoryService.OwnedItemsIdList();
            Debug.Log($"[TEST] Listing all items (Total: {items.Count}):");
            if (items.Count == 0)
            {
                Debug.Log("[TEST] - Inventory is empty");
            }
            else
            {
                foreach (var itemId in items)
                {
                    Debug.Log($"[TEST] - {itemId}");
                }
            }
        }
        
        private void AddCurrencyTest(int amount)
        {
            Debug.Log($"[TEST] Adding {amount} currency");
            int beforeAmount = _currencyService.GetAmount();
            _currencyService.Add(amount);
            int afterAmount = _currencyService.GetAmount();
            Debug.Log($"[TEST] Currency added. Before: {beforeAmount}, After: {afterAmount}");
        }
        
        private void SpendCurrencyTest(int amount)
        {
            Debug.Log($"[TEST] Attempting to spend {amount} currency");
            int beforeAmount = _currencyService.GetAmount();
            bool success = _currencyService.TrySpend(amount);
            int afterAmount = _currencyService.GetAmount();
            Debug.Log($"[TEST] Spend result: {(success ? "SUCCESS" : "FAILED")}. Before: {beforeAmount}, After: {afterAmount}");
        }
        
        private void GetCurrencyAmountTest()
        {
            int amount = _currencyService.GetAmount();
            Debug.Log($"[TEST] Current currency amount: {amount}");
        }
        
        private void SwitchToStateTest<T>() where T : IUIState
        {
            Debug.Log($"[TEST] Switching to state: {typeof(T).Name}");
            _uiStateMachine.SwitchTo<T>();
            Debug.Log($"[TEST] State switch complete");
        }
        
        private void ClearSavesTest()
        {
            Debug.Log("[TEST] Clearing all saves...");
            _saveSystem.DeleteSave("InventoryItems");
            _saveSystem.DeleteSave("CurrencyAmount");
            
            // Reinitialize services
            _inventoryService = new InventoryService(_saveSystem);
            _currencyService = new CurrencyService(_currencyConfig, _saveSystem);
            
            // Resubscribe to events
            _inventoryService.OnInventoryChanged += () => 
                Debug.Log("[InventoryService] Inventory changed!");
            
            _currencyService.OnAmountChanged += (amount) => 
                Debug.Log($"[CurrencyService] Amount changed to: {amount}");
            
            Debug.Log("[TEST] All saves cleared and services reinitialized!");
        }
    }
}