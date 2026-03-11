using System;
using System.Linq;

namespace EcommerceV2.Domain
{
    public class RepositoryBasketDiscountPolicy : BasketDiscountPolicy
    {
        private readonly DiscountRepository _discountRepository;

        public RepositoryBasketDiscountPolicy(DiscountRepository discountRepository) =>
            _discountRepository = discountRepository
                                  ?? throw new ArgumentNullException(nameof(discountRepository));

        public override Basket Apply(Basket basket)
        {
            var discounts = _discountRepository.GetDiscountedProducts().ToList();

            var evaluatedBasket = new Basket(basket.User);

            foreach (var extent in basket.Contents)
            {
                var product = discounts.Where(n => n.Id == extent.Product.Id)
                    .DefaultIfEmpty(extent.Product)
                    .SingleOrDefault();

                evaluatedBasket.Contents.Add(extent.WithItem(product));
            }

            return evaluatedBasket;
        }
    }
}