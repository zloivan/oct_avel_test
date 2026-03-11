using System;

namespace EcommerceV2.Domain
{
    public abstract class BasketDiscountPolicy
    {
        public abstract Basket Apply(Basket basket);
        
        protected Basket CreateDiscountedBasket(Basket original, Func<Extent, Extent> transform)
        {
            var result = new Basket();
            foreach (var extent in original.Contents)
                result.Contents.Add(transform(extent));
            
            return result;
        }
    }
}