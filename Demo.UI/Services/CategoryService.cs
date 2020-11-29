using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Demo.UI.Interfaces;
using Demo.UI.ViewModels;

namespace Demo.UI.Services
{
    public class CategoryService : ICategoryService
    {        
        private readonly Application.Interfaces.ICategoryService _categoryAppService;
        private readonly IMapper _mapper;

        public CategoryService(Application.Interfaces.ICategoryService categoryAppService, IMapper mapper)
        {
            _categoryAppService = categoryAppService ?? throw new ArgumentNullException(nameof(categoryAppService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategories()
        {
            var list = await _categoryAppService.GetCategories();
            var mapped = _mapper.Map<IEnumerable<CategoryViewModel>>(list);
            return mapped;
        }
    }
}
