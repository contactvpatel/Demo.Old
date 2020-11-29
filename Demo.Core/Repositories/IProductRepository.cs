using Demo.Core.Entities;
using Demo.Core.Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Core.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Product>> GetProductByName(string productName);
        Task<IEnumerable<Product>> GetProductByCategory(int categoryId);
    }
}
