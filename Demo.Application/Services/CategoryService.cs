using Demo.Application.Mapper;
using Demo.Application.Interfaces;
using Demo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Core.Repositories;
using Demo.Application.Models;

namespace Demo.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAppLogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, IAppLogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<CategoryModel>> GetCategories()
        {
            _logger.LogInformation("Get Categories");
            var category = await _categoryRepository.GetAllAsync();
            return ObjectMapper.Mapper.Map<IEnumerable<CategoryModel>>(category);
        }

        public async Task<CategoryModel> GetCategoryById(int categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            return ObjectMapper.Mapper.Map<CategoryModel>(category);
        }
    }
}
