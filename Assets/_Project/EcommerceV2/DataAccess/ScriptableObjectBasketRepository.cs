using EcommerceV2.Domain;

namespace EcommerceV2.DataAccess
{
    public class InMemoryBasketRepository : BasketRepository
    {
        private readonly Basket _basket;
        private IUserContext _basketOwner;

        public InMemoryBasketRepository(IUserContext basketOwner)
        {
            _basketOwner = basketOwner;
            _basket = new Basket(basketOwner);
        }

        public override Basket GetBasketFor(IUserContext user) =>
            _basket;

        public override void AddToBasket(Product product, int quantity)
        {
            var existing = _basket.Contents
                .Find(e => e.Product.Name == product.Name);

            if (existing != null)
                existing.Quantity += quantity;
            else
                _basket.Contents.Add(new Extent(product) { Quantity = quantity });
        }

        public override void EmptyBasket(IUserContext user) =>
            _basket.Contents.Clear();
    }
}