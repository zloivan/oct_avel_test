namespace EcommerceV2.Domain
{
    public class DefaultCustomerDiscountPolicy
    {
        public Product Apply(Product product, IUserContext user) =>
            user.IsPreferredCustomer ? new Product { Name = product.Name, UnitPrice = product.UnitPrice * 0.95f } : product;
    }
}