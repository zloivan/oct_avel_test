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
        private LocalUserContext _userContext;

        private void Start()
        {
            _userContext = new LocalUserContext();

            var repository = new ScriptableObjectProductRepository(_database);
            _homeController = new HomeController(repository);

            RenderHome(_userContext);

            var discountRepository = new ScriptableObjectDiscountRepository(_discountDatabase);
            var discountPolicy = new RepositoryBasketDiscountPolicy(discountRepository);
            var basketRepository = new InMemoryBasketRepository(_userContext);
            var basketService = new BasketService(basketRepository, discountPolicy);
            _basketController = new BasketController(basketService);

            RenderBaskets(_userContext);
        }

        public void OnAddFirstProductToBasket()
        {
            var product = _database.Products.FirstOrDefault();
            if (product == null)
            {
                return;
            }

            _basketController.AddToBasket(
                new Product(product.ProductId, product.Name, product.UnitPrice), 1);

            RenderBaskets(_userContext);
        }

        public void OnEmptyBasket()
        {
            _basketController.EmptyBasket(_userContext);
            RenderBaskets(_userContext);
        }

        private void RenderBaskets(LocalUserContext userContext) =>
            _basketView.Render(_basketController.GetBasket(userContext));

        private void RenderHome(LocalUserContext userContext) =>
            _view.Render(_homeController.Index(userContext));
    }
}