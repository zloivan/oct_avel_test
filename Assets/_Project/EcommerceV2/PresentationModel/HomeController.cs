
using EcommerceV2.Domain;

namespace EcommerceV2.PresentationModel
{
    public class HomeController
    {
        private readonly ProductRepository _repository;

        public HomeController(ProductRepository repository) =>
            _repository = repository;

        public FeaturedProductsViewModel Index(IUserContext user)
        {
            var vm = new FeaturedProductsViewModel();
            var productService = new ProductService(_repository);

            var products = productService.GetFeaturedProducts(user);

            foreach (var product in products)
            {
                vm.Products.Add(new ProductViewModel(product.Name, product.UnitPrice));
            }

            return vm;
        }
    }
}