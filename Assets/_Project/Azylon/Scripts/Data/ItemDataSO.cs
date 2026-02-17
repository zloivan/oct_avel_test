using System;
using UnityEngine;

namespace Azylon.Data
{
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Azylon/Configs/Item Data", order = 0)]
    public class ItemDataSO : ScriptableObject
    {
        [SerializeField] private string _id;
        public string ItemName;
        public string Description;
        public Sprite Icon;
        public int Price;

        public string GetId() => _id;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_id))
                _id = Guid.NewGuid().ToString();
        }
    }
}