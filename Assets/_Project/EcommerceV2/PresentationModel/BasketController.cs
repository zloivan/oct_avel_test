using System;
using EcommerceV2.Domain;

namespace EcommerceV2.PresentationModel
{
    public class BasketController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService) =>
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));

        public BasketViewModel GetBasket(string owner)
        {
            var basket = _basketService.GetBasketFor();

            var vm = new BasketViewModel();
            foreach (var extent in basket.Contents)
            {
                vm.Items.Add(new ExtentViewModel(extent));
            }

            return vm;
        }

        public void AddToBasket(string owner, Product product, int quantity) =>
            _basketService.AddToBasket(product, quantity);

        public void EmptyBasket(string owner) =>
            _basketService.EmptyBasket();
    }
}