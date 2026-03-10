using System.Text;
using EcommerceV2.PresentationModel;
using UnityEngine;
using UnityEngine.UI;

namespace EcommerceV2.UI
{
    public class BasketView : MonoBehaviour
    {
        [SerializeField] private Text _outputText;

        public void Render(BasketViewModel vm)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Basket\n");

            foreach (var item in vm.Items)
            {
                sb.AppendLine(item.SummaryTest);
            }

            _outputText.text = sb.ToString();
        }
    }
}