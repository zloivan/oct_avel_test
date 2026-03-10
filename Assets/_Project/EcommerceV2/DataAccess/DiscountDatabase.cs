using System.Collections.Generic;
using UnityEngine;

namespace EcommerceV2.DataAccess
{
    [CreateAssetMenu(fileName = "DiscountDatabase", menuName = "EcommerceV2/DiscountDatabase", order = 0)]
    public class DiscountDatabase : ScriptableObject
    {
        public List<ProductData> DiscountedProducts = new();
    }
}