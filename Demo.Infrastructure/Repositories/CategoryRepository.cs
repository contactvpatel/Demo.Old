using Demo.Core.Entities;
using Demo.Core.Repositories;
using Demo.Infrastructure.Data;
using Demo.Infrastructure.Repositories.Base;

namespace Demo.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DemoContext dbContext) : base(dbContext)
        {            
        }
    }
}
