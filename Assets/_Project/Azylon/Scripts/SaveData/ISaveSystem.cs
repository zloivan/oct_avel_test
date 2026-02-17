using System;

namespace Azylon.SaveData
{
    public interface ISaveSystem
    {
        void Save<T>(string fileName, T data, Func<T, bool> validator = null) where T : class;
        T Load<T>(string fileName, Func<T, bool> validator = null) where T : class, new();
        bool SaveExists(string fileName);
        void DeleteSave(string fileName);
    }
}