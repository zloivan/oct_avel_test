using System;
using System.IO;
using UnityEngine;

namespace Octopus.Services
{
    public class SaveSystem
    {
        private readonly string _savePath;

        public SaveSystem()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "Saves");

            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
        }

        public bool Save<T>(string fileName, T data) where T : class
        {
            try
            {
                var filePath = Path.Combine(_savePath, $"{fileName}.json");
                var json = JsonUtility.ToJson(data, true);
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Failed to save {fileName}: {e.Message}");
                return false;
            }
        }


        public T Load<T>(string fileName, Func<T, bool> validator = null) where T : class, new()
        {
            try
            {
                var filePath = Path.Combine(_savePath, $"{fileName}.json");

                if (!File.Exists(filePath))
                {
                    Debug.Log($"[SaveSystem] Save file {fileName} not found. Returning default.");
                    return new T();
                }

                var json = File.ReadAllText(filePath);
                var data = JsonUtility.FromJson<T>(json);

                if (validator == null || validator(data)) 
                    return data;
                
                Debug.LogWarning($"[SaveSystem] Save file {fileName} failed validation. Returning default.");
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
            return File.Exists(filePath);
        }


        public bool DeleteSave(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_savePath, $"{fileName}.json");
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Failed to delete {fileName}: {e.Message}");
                return false;
            }
        }
    }
}