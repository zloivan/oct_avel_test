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

        public IEnumerable<Product> GetFeaturedProducts(IUserContext user)
        {
            return _repository.GetFeaturedProducts()
                .Select(p => p.WithDiscount(user));
        }
    }
}