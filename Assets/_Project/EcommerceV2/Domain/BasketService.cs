using System;

namespace EcommerceV2.Domain
{
    public class BasketService : IBasketService
    {
        private readonly BasketRepository _basketRepository;
        private readonly BasketDiscountPolicy _basketDiscountPolicy;

        public BasketService(BasketRepository basketRepository, BasketDiscountPolicy basketDiscountPolicy)
        {
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            _basketDiscountPolicy =
                basketDiscountPolicy ?? throw new ArgumentNullException(nameof(basketDiscountPolicy));
        }

        public Basket GetBasketFor() =>
            _basketDiscountPolicy.Apply(_basketRepository.GetBasketFor());

        public void AddToBasket(Product product, int quantity) =>
            _basketRepository.AddToBasket(product, quantity);

        public void EmptyBasket() =>
            _basketRepository.EmptyBasket();
    }
}