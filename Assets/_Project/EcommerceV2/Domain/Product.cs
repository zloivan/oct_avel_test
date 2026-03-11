namespace EcommerceV2.Domain
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float UnitPrice { get; set; }

        public Product WithDiscount(IUserContext user)
        {
            var policy = new DefaultCustomerDiscountPolicy();
            return policy.Apply(this, user);
        }
        
    }
}