using System.Collections.Generic;
using UnityEngine;

namespace EcommerceV2.DataAccess
{
    [CreateAssetMenu(fileName = "ProductDatabase", menuName = "ECommerceV2/Products", order = 0)]
    public class ProductDatabase : ScriptableObject
    {
        public List<ProductData> Products = new();
    }
}