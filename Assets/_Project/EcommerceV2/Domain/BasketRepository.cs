namespace EcommerceV2.Domain
{
    public abstract class BasketRepository
    {
        public abstract Basket GetBasketFor(IUserContext user);
        public abstract void AddToBasket(Product product, int quantity);
        public abstract void EmptyBasket(IUserContext user);
    }
}