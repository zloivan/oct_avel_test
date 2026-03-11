using System;

namespace EcommerceV2.Domain
{
    public class DefaultProductDiscountPolicy : BasketDiscountPolicy
    {
        private readonly IUserContext _userContext;

        public DefaultProductDiscountPolicy(IUserContext userContext) =>
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));

        public override Basket Apply(Basket basket)
        {
            if (!_userContext.IsPreferredCustomer)
                return basket;

            return CreateDiscountedBasket(basket, extent => new Extent
            {
                Product = extent.Product.WithDiscount(0.95f),
                Quantity = extent.Quantity,
            });
        }
    }
}