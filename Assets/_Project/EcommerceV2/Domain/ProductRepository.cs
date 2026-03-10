using System.Collections.Generic;

namespace EcommerceV2.Domain
{
    public abstract class ProductRepository
    {
        public abstract IEnumerable<Product> GetFeaturedProducts();
    }
}