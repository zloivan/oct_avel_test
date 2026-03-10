using EcommerceV2.Domain;

namespace EcommerceV2.DataAccess
{
    public class InMemoryBasketRepository : BasketRepository
    {
        private readonly Basket _basket = new() { };

        public override Basket GetBasketFor() =>
            _basket;

        public override void AddToBasket(Product product, int quantity)
        {
            var existing = _basket.Contents
                .Find(e => e.Product.Name == product.Name);

            if (existing != null)
                existing.Quantity += quantity;
            else
                _basket.Contents.Add(new Extent { Product = product, Quantity = quantity });
        }

        public override void EmptyBasket() =>
            _basket.Contents.Clear();
    }
}