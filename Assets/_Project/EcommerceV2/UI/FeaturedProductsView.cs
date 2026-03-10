using System.Text;
using EcommerceV2.PresentationModel;
using UnityEngine;
using UnityEngine.UI;

namespace EcommerceV2.UI
{
    public class FeaturedProductsView : MonoBehaviour
    {
        [SerializeField] private Text _outputText;

        public void Render(FeaturedProductsViewModel viewModel)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Featured Products\n");

            foreach (var viewModelProduct in viewModel.Products)
            {
                sb.AppendLine(viewModelProduct.SummaryText);
            }

            _outputText.text = sb.ToString();
        }
    }
}