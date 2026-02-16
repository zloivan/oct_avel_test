using _Project.Octopus.Scripts.Core;
using _Project.Octopus.Scripts.Player;
using _Project.Octopus.Scripts.UI;
using UnityEngine;
using VContainer;

namespace _Project.Octopus.Scripts
{
    public class TestingScript : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private PlayerData _loaded;

        private ISaveSystem _saveSystem;
        private PopupManager _popupManager;


        [Inject]
        public void Construct(ISaveSystem saveSystem, PopupManager popupManager)
        {
            _saveSystem = saveSystem;
            _popupManager = popupManager;
        }

        [ContextMenu("Save")]
        public void SaveData()
        {
            _loaded = null;
            _saveSystem.Save("PlayerData", _playerData);
        }

        [ContextMenu("Load")]
        public void LoadData()
        {
            _loaded = _saveSystem.Load<PlayerData>("PlayerData");
        }

        [ContextMenu("Save Exsist")]
        public void SaveExist()
        {
            Debug.Log($"Save Exists: {_saveSystem.SaveExists("PlayerData")}");
        }

        [ContextMenu("Delete Save")]
        public void DeleteSave()
        {
            _saveSystem.DeleteSave("PlayerData");
        }

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
    }
}