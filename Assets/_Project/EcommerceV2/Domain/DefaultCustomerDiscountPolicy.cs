namespace EcommerceV2.Domain
{
    public class DefaultCustomerDiscountPolicy
    {
        public Product Apply(Product product, IUserContext user) =>
            user.IsPreferredCustomer
                ? new Product(id: product.Id, name: product.Name, unitPrice: product.UnitPrice * 0.95f)
                : product;
    }
}