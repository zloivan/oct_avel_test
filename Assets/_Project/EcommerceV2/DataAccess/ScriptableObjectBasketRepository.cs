using EcommerceV2.Domain;

namespace EcommerceV2.DataAccess
{
    public class InMemoryBasketRepository : BasketRepository
    {
        private readonly Basket _basket;

        public InMemoryBasketRepository(IUserContext basketOwner) =>
            _basket = new Basket(basketOwner);

        public override Basket GetBasketFor(IUserContext _) =>
            _basket;

        public override void AddToBasket(Product product, int quantity)
        {
            var existing = _basket.Contents
                .Find(e => e.Product.Id == product.Id);

            if (existing != null)
                existing.Quantity += quantity;
            else
                _basket.Contents.Add(new Extent(product) { Quantity = quantity });
        }

        public override void EmptyBasket(IUserContext _) =>
            _basket.Contents.Clear();
    }
}