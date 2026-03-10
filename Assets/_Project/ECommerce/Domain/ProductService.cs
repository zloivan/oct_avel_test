using System.Collections.Generic;
using System.Linq;
using ECommerce.DataAccess;

namespace ECommerce.Domain
{
    public class ProductService
    {
        private readonly ProductObjectContext _objectContext;

        public ProductService()
        {
            _objectContext = new ProductObjectContext();
        }

        public IEnumerable<ProductData> GetFeaturedProducts(bool isCustomerPreferred)
        {
            var discount = isCustomerPreferred ? 0.95f : 1f;

            var products = _objectContext.Products.
                Where(p => p.IsFeatured);
            
            return products.Select(p => new ProductData
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                UnitPrice = p.UnitPrice * discount
            });
        }
    }
}