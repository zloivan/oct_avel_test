namespace EcommerceV2.Domain
{
    public class Extent
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public float Total => Product.UnitPrice * Quantity;
    }
}