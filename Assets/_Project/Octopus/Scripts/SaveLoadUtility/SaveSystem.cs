using System;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

namespace Octopus.SaveLoadUtility
{
    public interface ISaveSystem
    {
        void Save<T>(string fileName, T data) where T : class;
        T Load<T>(string fileName) where T : class, new();
        bool SaveExists(string fileName);
        void DeleteSave(string fileName);
    }

    [UsedImplicitly]
    public class SaveSystem : ISaveSystem
    {
        private readonly string _savePath;
        private readonly bool _debugMode;

        public SaveSystem(bool debugMode = false)
        {
            _debugMode = debugMode;
            _savePath = Path.Combine(Application.persistentDataPath, "Saves");

            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
        }

        public void Save<T>(string fileName, T data) where T : class
        {
            if (_debugMode) Debug.Log($"[SaveSystem] Saving {fileName}...");

            try
            {
                var filePath = Path.Combine(_savePath, $"{fileName}.json");
                var json = JsonUtility.ToJson(data, true);
                File.WriteAllText(filePath, json);

                if (_debugMode) Debug.Log($"[SaveSystem] Saved {fileName} successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Failed to save {fileName}: {e.Message}");
            }
        }

        public T Load<T>(string fileName) where T : class, new()
        {
            if (_debugMode) Debug.Log($"[SaveSystem] Loading {fileName}...");

            try
            {
                var filePath = Path.Combine(_savePath, $"{fileName}.json");

                if (!File.Exists(filePath))
                {
                    if (_debugMode)
                        Debug.LogWarning($"[SaveSystem] Save file {fileName} not found. Returning default.");
                    return new T();
                }

                var json = File.ReadAllText(filePath);
                var data = JsonUtility.FromJson<T>(json);

                if (_debugMode) Debug.Log($"[SaveSystem] Loaded {fileName} successfully");
                return data;
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

            if (_debugMode) Debug.Log($"[SaveSystem] Save {fileName} exists: {exists}");
            return exists;
        }

        public void DeleteSave(string fileName)
        {
            if (_debugMode) Debug.Log($"[SaveSystem] Deleting {fileName}...");

            try
            {
                var filePath = Path.Combine(_savePath, $"{fileName}.json");

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    if (_debugMode) Debug.Log($"[SaveSystem] Deleted {fileName} successfully");
                }
                else
                {
                    if (_debugMode) Debug.LogWarning($"[SaveSystem] Save file {fileName} not found");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Failed to delete {fileName}: {e.Message}");
            }
        }
    }
}