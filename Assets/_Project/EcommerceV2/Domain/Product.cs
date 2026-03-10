namespace EcommerceV2.Domain
{
    public class Product
    {
        public string Name { get; set; }
        public float UnitPrice { get; set; }

        public Product WithDiscount(float factor) =>
            new() { Name = Name, UnitPrice = UnitPrice * factor };
    }
}