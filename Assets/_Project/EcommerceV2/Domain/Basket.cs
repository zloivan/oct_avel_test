using System.Collections.Generic;

namespace EcommerceV2.Domain
{
    public class Basket
    {
        public List<Extent> Contents { get; set; } = new();
    }
}