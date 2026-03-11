namespace EcommerceV2.Domain
{
    public interface IBasketService
    {
        Basket GetBasket(IUserContext user);
        void AddToBasket(Product product, int quantity);
        void EmptyBasket(IUserContext user);
    }
    
}