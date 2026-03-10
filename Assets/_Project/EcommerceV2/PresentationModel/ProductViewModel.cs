namespace EcommerceV2.PresentationModel
{
    public class ProductViewModel
    {
        public string Name { get; }
        public float UnitPrice { get; }
        public string SummaryText => $"{Name}\t(${UnitPrice:F2})";

        public ProductViewModel(string name, float unitPrice)
        {
            Name = name;
            UnitPrice = unitPrice;
        }
    }
}