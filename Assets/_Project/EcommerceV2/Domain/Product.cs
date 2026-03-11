namespace EcommerceV2.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float UnitPrice { get; set; }

        public Product(int id, string name, float unitPrice)
        {
            Id = id;
            Name = name;
            UnitPrice = unitPrice;
        }

        public Product WithDiscount(IUserContext user)
        {
            var policy = new DefaultCustomerDiscountPolicy();
            return policy.Apply(this, user);
        }
        
    }
}