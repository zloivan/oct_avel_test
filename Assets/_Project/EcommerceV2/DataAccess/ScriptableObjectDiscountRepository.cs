using System;
using System.Collections.Generic;
using System.Linq;
using EcommerceV2.Domain;

namespace EcommerceV2.DataAccess
{
    public class ScriptableObjectDiscountRepository : DiscountRepository
    {
        private readonly DiscountDatabase _discountDatabase;

        public ScriptableObjectDiscountRepository(DiscountDatabase discountDatabase) =>
            _discountDatabase = discountDatabase ?? throw new ArgumentNullException(nameof(discountDatabase));

        public override IEnumerable<Product> GetDiscountedProducts() =>
            _discountDatabase.DiscountedProducts.Select(p => new Product
            {
                Name = p.Name,
                UnitPrice = p.UnitPrice,
            });
    }
}