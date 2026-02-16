using UnityEngine;

namespace _Project.Octopus.Scripts.Core
{
    public interface ISaveSystem
    {
        void Save<T>(string fileName, T data) where T : class;

        T Load<T>(string fileName) where T : class, new();

        bool SaveExists(string fileName);

        void DeleteSave(string fileName);
    }

    public class SaveProcessor : ISaveSystem
    {
        private readonly SaveSystem _saveSystem = new();

        public void Save<T>(string fileName, T data) where T : class =>
            _saveSystem.Save(fileName, data);

        public T Load<T>(string fileName) where T : class, new() =>
            _saveSystem.Load<T>(fileName, validator: null);

        public bool SaveExists(string fileName) =>
            _saveSystem.SaveExists(fileName);

        public void DeleteSave(string fileName) =>
            _saveSystem.DeleteSave(fileName);
    }

    public class DebugSavingDecorator : ISaveSystem
    {
        private readonly ISaveSystem _original;

        public DebugSavingDecorator(ISaveSystem saveSystem)
        {
            _original = saveSystem;
        }

        public void Save<T>(string fileName, T data) where T : class
        {
            Debug.Log("On Save Started...");
            _original.Save(fileName, data);
            Debug.Log($"On save completed: {fileName}");
        }

        public T Load<T>(string fileName) where T : class, new()
        {
            Debug.Log("On Load started...");
            var load = _original.Load<T>(fileName);
            Debug.Log($"On load completed: {fileName}");

            if (load == null)
            {
                Debug.LogWarning("Load returned null");
            }

            return load;
        }

        public bool SaveExists(string fileName)
        {
            Debug.Log("On SaveExists started...");
            var saveExists = _original.SaveExists(fileName);
            Debug.Log(saveExists ? $"Save for {fileName} - exist" : $"Save for {fileName} - does not exist");
            return saveExists;
        }

        public void DeleteSave(string fileName)
        {
            Debug.Log("On Delete Started...");
            _original.DeleteSave(fileName);
            Debug.Log($"On delete completed: {fileName}");
        }
    }
}