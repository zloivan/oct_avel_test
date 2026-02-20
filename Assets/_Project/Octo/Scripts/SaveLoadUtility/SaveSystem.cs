using System;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

namespace Octo.SaveLoadUtility
{
    public interface ISaveSystem
    {
        void Save<T>(string fileName, T data, Func<T, bool> validator = null) where T : class;
        T Load<T>(string fileName, Func<T, bool> validator = null) where T : class, new();
        bool SaveExists(string fileName);
        void DeleteSave(string fileName);
    }

    [UsedImplicitly]
    public class SaveSystem : ISaveSystem
    {
        private const string SAVES_FOLDER_NAME = "Saves";
        private readonly string _savePath;

        public SaveSystem(bool debugMode = false)
        {
            _savePath = Path.Combine(Application.persistentDataPath, SAVES_FOLDER_NAME);

            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
        }

        public void Save<T>(string fileName, T data, Func<T, bool> validator = null) where T : class
        {
            try
            {
                if (validator != null && !validator(data))
                {
                    Debug.LogError($"[SaveSystem] Validation failed for {fileName}. Save aborted.");
                    return;
                }

                var filePath = Path.Combine(_savePath, $"{fileName}.json");
                var json = JsonUtility.ToJson(data, true);
                File.WriteAllText(filePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Failed to save {fileName}: {e.Message}");
            }
        }

        public T Load<T>(string fileName, Func<T, bool> validator = null) where T : class, new()
        {
            try
            {
                var filePath = Path.Combine(_savePath, $"{fileName}.json");

                if (!File.Exists(filePath))
                    return new T();

                var json = File.ReadAllText(filePath);
                var data = JsonUtility.FromJson<T>(json);

                if (validator == null || validator(data))
                    return data;

                Debug.LogError($"[SaveSystem] Validation failed for {fileName}. Returning default.");
                return new T();
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Failed to load {fileName}: {e.Message}");
                return new T();
            }
        }

        public bool SaveExists(string fileName)
        {
            var filePath = Path.Combine(_savePath, $"{fileName}.json");
            var exists = File.Exists(filePath);

            return exists;
        }

        public void DeleteSave(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_savePath, $"{fileName}.json");

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Failed to delete {fileName}: {e.Message}");
            }
        }
    }
}