using System.Collections.Generic;

namespace EcommerceV2.PresentationModel
{
    public class BasketViewModel
    {
        public List<ExtentViewModel> Items { get; } = new();
        public bool IsEmpty => Items.Count == 0;
    }
}