using System;
using System.Text;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.ECommerce.Presentation
{
    public class HomeController : MonoBehaviour
    {
        [SerializeField] private Text _outputText;

        [SerializeField] private bool _isCustomerPreferred;

        private void Start()
        {
            var service = new ProductService();
            var products = service.GetFeaturedProducts(_isCustomerPreferred);

            var sb = new StringBuilder();
            sb.AppendLine("Featured Products\n");

            foreach (var productData in products)
            {
                sb.AppendLine($"{productData.Name} - ${productData.UnitPrice:F2}");
            }

            _outputText.text = sb.ToString();
        }
    }
}