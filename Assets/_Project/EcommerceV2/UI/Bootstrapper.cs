using System.Linq;
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

        private BasketController _basketController;
        private HomeController _homeController;

        private void Start()
        {
            var userContext = new LocalUserContext();
            
            var repository = new ScriptableObjectProductRepository(_database);
            _homeController = new HomeController(repository);
            
            RenderHome(userContext);

            var discountRepository = new ScriptableObjectDiscountRepository(_discountDatabase);
            var discountPolicy = new RepositoryBasketDiscountPolicy(discountRepository);
            var basketRepository = new InMemoryBasketRepository();
            var basketService = new BasketService(basketRepository, discountPolicy);
            _basketController = new BasketController(basketService);

            RenderBaskets();
        }

        public void OnAddFirstProductToBasket()
        {
            var product = _database.Products.FirstOrDefault();
            if (product == null)
            {
                return;
            }
            
            _basketController.AddToBasket(new Product
            {
                Name = product.Name,
                UnitPrice = product.UnitPrice,
            }, 1);
            
            RenderBaskets();
        }

        public void OnEmptyBasket()
        {
            _basketController.EmptyBasket();
            RenderBaskets();
        }

        private void RenderBaskets() =>
            _basketView.Render(_basketController.GetBasket());

        private void RenderHome(LocalUserContext userContext) =>
            _view.Render(_homeController.Index(userContext.IsPreferredCustomer));
    }
}