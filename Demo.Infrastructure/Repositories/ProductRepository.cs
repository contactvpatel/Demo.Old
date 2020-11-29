using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Core.Entities;
using Demo.Core.Repositories;
using Demo.Infrastructure.Data;
using Demo.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DemoContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await DbContext.Products
                .Include(x => x.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string productName)
        {
            return await DbContext.Products
                .Where(x => x.ProductName == productName)
                .Include(x => x.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(int categoryId)
        {
            return await DbContext.Products
                .Where(x => x.CategoryId == categoryId)
                .Include(x => x.Category)
                .ToListAsync();
        }
    }
}
