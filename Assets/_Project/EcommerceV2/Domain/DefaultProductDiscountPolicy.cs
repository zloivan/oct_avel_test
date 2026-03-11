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
            var evaluatedBasket = new Basket(basket.User);
            
            foreach (var extent in basket.Contents)
            {
                extent.Product = extent.Product.WithDiscount(_userContext);
                evaluatedBasket.Contents.Add(extent);
            }
           
            return evaluatedBasket;
        }
    }
}