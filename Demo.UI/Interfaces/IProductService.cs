using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.UI.ViewModels;

namespace Demo.UI.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetProducts();
        Task<ProductViewModel> GetProductById(int productId);
        Task<IEnumerable<ProductViewModel>> GetProductByCategory(int categoryId);
        Task<IEnumerable<CategoryViewModel>> GetCategories();
        Task<ProductViewModel> CreateProduct(ProductViewModel productViewModel);
        Task UpdateProduct(ProductViewModel productViewModel);
        Task DeleteProduct(ProductViewModel productViewModel);
    }
}
