using System;
using System.Collections.Generic;
using System.Linq;

namespace EcommerceV2.Domain
{
    public class RepositoryBasketDiscountPolicy : BasketDiscountPolicy
    {
        private readonly DiscountRepository _discountRepository;

        public RepositoryBasketDiscountPolicy(DiscountRepository discountRepository) =>
            _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));

        public override Basket Apply(Basket basket)
        {
            var discountedNames = new HashSet<string>(
                _discountRepository.GetDiscountedProducts()
                    .Select(p => p.Name));
            
            return CreateDiscountedBasket(basket, extent => new Extent
            {
                Product  = discountedNames.Contains(extent.Product.Name)
                    ? extent.Product.WithDiscount(0.95f)
                    : extent.Product,
                Quantity = extent.Quantity
            });
        }
    }
}