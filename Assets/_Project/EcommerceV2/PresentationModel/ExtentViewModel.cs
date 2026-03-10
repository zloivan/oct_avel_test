using EcommerceV2.Domain;

namespace EcommerceV2.PresentationModel
{
    public class ExtentViewModel
    {
        public string SummaryTest { get; }

        public ExtentViewModel(Extent extent) =>
            SummaryTest = $"{extent.Product.Name} x {extent.Quantity}\t(${extent.Total:F2})";
    }
}