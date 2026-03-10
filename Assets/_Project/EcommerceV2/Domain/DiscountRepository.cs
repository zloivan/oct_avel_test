using System.Collections.Generic;

namespace EcommerceV2.Domain
{
    public abstract class DiscountRepository
    {
        public abstract IEnumerable<Product> GetDiscountedProducts();
    }
}