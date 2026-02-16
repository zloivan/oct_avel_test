using _Project.Octopus.Scripts.Core;
using _Project.Octopus.Scripts.Player;
using UnityEngine;
using VContainer;

namespace _Project.Octopus.Scripts
{
    public class TestingScript : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private PlayerData _loaded;

        private ISaveSystem _saveSystem;

        [Inject]
        public void Construct(ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
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
    }
}