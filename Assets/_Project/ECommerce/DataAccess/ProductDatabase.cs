using System.Collections.Generic;
using UnityEngine;

namespace ECommerce.DataAccess
{
    [CreateAssetMenu(fileName = "ProductDatabase", menuName = "ECommerce/Products", order = 0)]
    public class ProductDatabase : ScriptableObject
    {
        public List<ProductData> Products = new List<ProductData>();
    }
}