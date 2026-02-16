using Octopus.Entities;
using Octopus.SaveLoadUtility;
using Octopus.Testing.SavingTestData;
using Octopus.UI.Popups;
using UnityEngine;
using VContainer;

namespace Octopus.Testing
{
    public class TestingScript : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private PlayerData _loaded;

        private ISaveSystem _saveSystem;
        private PopupManager _popupManager;
        private EntityManager _entityManager;
        private const string SAVE_FILE_NAME = "PlayerData";

        [Inject]
        public void Construct(ISaveSystem saveSystem, PopupManager popupManager, EntityManager entityManager)
        {
            _saveSystem = saveSystem;
            _popupManager = popupManager;
            _entityManager = entityManager;
        }

        [ContextMenu("Save")]
        public void SaveData()
        {
            _loaded = null;

            _saveSystem.Save(SAVE_FILE_NAME, _playerData);
        }

        [ContextMenu("Load")]
        public void LoadData() =>
            _loaded = _saveSystem.Load<PlayerData>(SAVE_FILE_NAME);

        [ContextMenu("Save Exist")]
        public void SaveExist() =>
            Debug.Log($"Save Exists: {_saveSystem.SaveExists(SAVE_FILE_NAME)}");

        [ContextMenu("Delete Save")]
        public void DeleteSave() =>
            _saveSystem.DeleteSave(SAVE_FILE_NAME);

        [ContextMenu("Create Popup")]
        public void CreatePopup()
        {
            var popupConfig = new PopupConfig("Test",
                "Some super cool message",
                new PopupButton("Accept", () => { Debug.Log("Accept pressed"); }),
                new PopupButton("Cancel", () => { Debug.Log("Cancel pressed"); }),
                new PopupButton("Buy", () => { Debug.Log("Buy pressed"); }),
                new PopupButton("Sell", () => { Debug.Log("Sell pressed"); })
            );

            _popupManager.ShowPopup(popupConfig);
        }

        [ContextMenu("Close Popup")]
        public void ClosePopup() =>
            _popupManager.HidePopup();

        [ContextMenu("Log Active Entities")]
        public void LogActiveEntities()
        {
            var count = _entityManager.GetActiveEntityCount();
            Debug.Log($"[Test] Active entities: {count}");

            var entities = _entityManager.GetActiveEntities();
            foreach (var entity in entities)
            {
                if (entity is GameplayEntity ge)
                {
                    Debug.Log($"  - {ge.name} (Active: {ge.IsActive})");
                }
            }
        }

        [ContextMenu("Test Clear")]
        private void TestClear()
        {
            _entityManager.Clear();
            Debug.Log("[Test] EntityManager cleared");
            LogActiveEntities();
        }
    }
}