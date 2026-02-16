using System.IO;
using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Octopus.Scripts.Core
{
    public interface ISaveSystem
    {
        /// <summary>
        /// Saves any serializable object to a JSON file
        /// </summary>
        void Save<T>(string fileName, T data) where T : class;

        /// <summary>
        /// Loads an object from a JSON file. Returns default if file doesn't exist.
        /// </summary>
        T Load<T>(string fileName) where T : class, new();

        /// <summary>
        /// Checks if a save file exists
        /// </summary>
        bool SaveExists(string fileName);

        /// <summary>
        /// Deletes a save file if it exists
        /// </summary>
        void DeleteSave(string fileName);
    }

    [UsedImplicitly]
    public class SaveSystem : ISaveSystem
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

        /// <summary>
        /// Saves any serializable object to a JSON file
        /// </summary>
        public void Save<T>(string fileName, T data) where T : class
        {
            var filePath = Path.Combine(_savePath, $"{fileName}.json");
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Loads an object from a JSON file. Returns default if file doesn't exist.
        /// </summary>
        public T Load<T>(string fileName) where T : class, new()
        {
            var filePath = Path.Combine(_savePath, $"{fileName}.json");

            if (!File.Exists(filePath))
            {
                return new T();
            }

            var json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(json);
        }

        /// <summary>
        /// Checks if a save file exists
        /// </summary>
        public bool SaveExists(string fileName)
        {
            var filePath = Path.Combine(_savePath, $"{fileName}.json");
            return File.Exists(filePath);
        }

        /// <summary>
        /// Deletes a save file if it exists
        /// </summary>
        public void DeleteSave(string fileName)
        {
            var filePath = Path.Combine(_savePath, $"{fileName}.json");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}