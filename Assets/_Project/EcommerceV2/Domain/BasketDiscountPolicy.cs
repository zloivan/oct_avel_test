using System;

namespace EcommerceV2.Domain
{
    public abstract class BasketDiscountPolicy
    {
        public abstract Basket Apply(Basket basket);
    }
}