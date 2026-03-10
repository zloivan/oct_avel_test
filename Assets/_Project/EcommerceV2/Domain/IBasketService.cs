namespace EcommerceV2.Domain
{
    public interface IBasketService
    {
        Basket GetBasketFor();
        void AddToBasket(Product product, int quantity);
        void EmptyBasket();
    }
    
}