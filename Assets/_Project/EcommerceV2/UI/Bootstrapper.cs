using EcommerceV2.DataAccess;
using EcommerceV2.Domain;
using EcommerceV2.PresentationModel;
using UnityEngine;

namespace EcommerceV2.UI
{
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Featured Products")]
        [SerializeField] private ProductDatabase _database;

        [SerializeField] private FeaturedProductsView _view;

        [Header("Basket")]
        [SerializeField] private BasketView _basketView;

        [SerializeField] private DiscountDatabase _discountDatabase;

        [Header("User")]
        [SerializeField] private string _playerName = "Player";

        [SerializeField] private bool _isPreferredCustomer;
        private BasketController _basketController;

        private void Start()
        {
            var repository = new ScriptableObjectProductRepository(_database);
            var homeController = new HomeController(repository);

            _view.Render(homeController.Index(_isPreferredCustomer));

            var discountRepository = new ScriptableObjectDiscountRepository(_discountDatabase);
            var discountPolicy = new RepositoryBasketDiscountPolicy(discountRepository);
            var basketRepository = new InMemoryBasketRepository();
            var basketService = new BasketService(basketRepository, discountPolicy);
            _basketController = new BasketController(basketService);


            RenderBaskets();
        }

        public void OnAddFirstProductToBasket()
        {
            var product = _database.Products.Find(p=>p.IsFeatured);
            if (product == null)
            {
                return;
            }
            
            _basketController.AddToBasket(_playerName, new Product
            {
                Name = product.Name,
                UnitPrice = product.UnitPrice,
            }, 1);
            
            RenderBaskets();
        }

        public void OnEmptyBasket()
        {
            _basketController.EmptyBasket(_playerName);
            RenderBaskets();
        }

        private void RenderBaskets()
        {
            _basketView.Render(_basketController.GetBasket(_playerName));
        }
    }
}