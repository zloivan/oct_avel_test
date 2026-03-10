namespace EcommerceV2.Domain
{
    public class DefaultProductDiscountPolicy : BasketDiscountPolicy
    {
        private readonly IUserContext _userContext;

        public DefaultProductDiscountPolicy(IUserContext userContext) =>
            _userContext = userContext;

        public override Basket Apply(Basket basket)
        {
            if (_userContext.IsPreferredCustomer)
                return basket;

            return CreateDiscountedBasket(basket,
                e => new Extent { Product = e.Product.WithDiscount(0.95f), Quantity = e.Quantity });
        }
    }
}