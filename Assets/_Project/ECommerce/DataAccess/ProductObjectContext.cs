using System.Collections.Generic;
using UnityEngine;

namespace ECommerce.DataAccess
{
    public class ProductObjectContext
    {
        private readonly ProductDatabase _productDatabase;

        public ProductObjectContext() =>
            _productDatabase = Resources.Load<ProductDatabase>("ProductDatabase");

        public List<ProductData> Products => _productDatabase.Products;
    }
}