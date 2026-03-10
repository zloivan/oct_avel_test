using System;

namespace EcommerceV2.DataAccess
{
    [Serializable]
    public class ProductData
    {
        public int ProductId;
        public string Name;
        public string Description;
        public float UnitPrice;
        public bool IsFeatured;
    }
}