using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.UI.ViewModels;

namespace Demo.UI.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetCategories();
    }
}
