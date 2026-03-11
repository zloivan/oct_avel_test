using System;
using System.Collections.Generic;
using System.Linq;
using EcommerceV2.Domain;

namespace EcommerceV2.DataAccess
{
    public class ScriptableObjectProductRepository : ProductRepository
    {
        private readonly ProductDatabase _database;

        public ScriptableObjectProductRepository(ProductDatabase productDatabase) =>
            _database = productDatabase ?? throw new ArgumentNullException(nameof(productDatabase));

        public override IEnumerable<Product> GetFeaturedProducts() =>
            _database.Products
                .Where(p => p.IsFeatured)
                .Select(p => new Product(p.ProductId, p.Name, p.UnitPrice));
    }
}