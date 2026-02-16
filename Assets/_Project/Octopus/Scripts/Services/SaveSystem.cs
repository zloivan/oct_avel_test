using System;
using System.IO;
using UnityEngine;

namespace _Project.Octopus.Scripts.Core
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
                string filePath = Path.Combine(_savePath, $"{fileName}.json");
                string json = JsonUtility.ToJson(data, true);
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
                string filePath = Path.Combine(_savePath, $"{fileName}.json");

                if (!File.Exists(filePath))
                {
                    Debug.Log($"[SaveSystem] Save file {fileName} not found. Returning default.");
                    return new T();
                }

                string json = File.ReadAllText(filePath);
                T data = JsonUtility.FromJson<T>(json);

                // Optional validation
                if (validator != null && !validator(data))
                {
                    Debug.LogWarning($"[SaveSystem] Save file {fileName} failed validation. Returning default.");
                    return new T();
                }

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
            string filePath = Path.Combine(_savePath, $"{fileName}.json");
            return File.Exists(filePath);
        }


        public bool DeleteSave(string fileName)
        {
            try
            {
                string filePath = Path.Combine(_savePath, $"{fileName}.json");
                
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