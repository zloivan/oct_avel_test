namespace EcommerceV2.Domain
{
    public interface IBasketService
    {
        Basket GetBasket();
        void AddToBasket(Product product, int quantity);
        void EmptyBasket();
    }
    
}