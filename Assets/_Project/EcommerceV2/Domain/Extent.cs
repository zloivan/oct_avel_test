namespace EcommerceV2.Domain
{
    public class Extent
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public float Total => Product.UnitPrice * Quantity;

        public Extent(Product item)
        {
            Product = item;
            Quantity = 1;
        }

        public Extent WithItem(Product item)
        {
            var oldQuantity = Quantity;
            
            var newExtent = new Extent(item)
            {
                Quantity = oldQuantity,
            };

            return newExtent;
        }
    }
}