using System;
using System.Collections.Generic;
using System.Linq;

namespace EcommerceV2.Domain
{
    public class ProductService
    {
        private readonly ProductRepository _repository;

        public ProductService(ProductRepository repository) =>
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public IEnumerable<Product> GetFeaturedProducts(bool isPreferredCustomer)
        {
            var discount = isPreferredCustomer ? 0.95f : 1f;

            return _repository.GetFeaturedProducts()
                .Select(p => p.WithDiscount(discount));
        }
    }
}