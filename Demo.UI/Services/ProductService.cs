using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Models;
using Demo.UI.Interfaces;
using Demo.UI.ViewModels;
using Microsoft.Extensions.Logging;

namespace Demo.UI.Services
{
    public class ProductService : IProductService
    {
        private readonly Application.Interfaces.IProductService _productAppService;
        private readonly Application.Interfaces.ICategoryService _categoryAppService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(Application.Interfaces.IProductService productAppService, Application.Interfaces.ICategoryService categoryAppService, IMapper mapper, ILogger<ProductService> logger)
        {
            _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
            _categoryAppService = categoryAppService ?? throw new ArgumentNullException(nameof(categoryAppService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ProductViewModel>> GetProducts()
        {
            var list = await _productAppService.GetProducts();
            return _mapper.Map<IEnumerable<ProductViewModel>>(list);
        }

        public async Task<ProductViewModel> GetProductById(int productId)
        {
            var product = await _productAppService.GetProductById(productId);
            var mapped = _mapper.Map<ProductViewModel>(product);
            return mapped;
        }

        public async Task<IEnumerable<ProductViewModel>> GetProductByCategory(int categoryId)
        {
            var list = await _productAppService.GetProductByCategory(categoryId);
            var mapped = _mapper.Map<IEnumerable<ProductViewModel>>(list);
            return mapped;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategories()
        {
            var list = await _categoryAppService.GetCategories();
            var mapped = _mapper.Map<IEnumerable<CategoryViewModel>>(list);
            return mapped;
        }

        public async Task<ProductViewModel> CreateProduct(ProductViewModel productViewModel)
        {
            var mapped = _mapper.Map<ProductModel>(productViewModel);
            if (mapped == null)
                throw new Exception($"Entity could not be mapped.");

            var entityDto = await _productAppService.Create(mapped);
            _logger.LogInformation($"Entity successfully added - HomeService");

            var mappedViewModel = _mapper.Map<ProductViewModel>(entityDto);
            return mappedViewModel;
        }

        public async Task UpdateProduct(ProductViewModel productViewModel)
        {
            var mapped = _mapper.Map<ProductModel>(productViewModel);
            if (mapped == null)
                throw new Exception($"Entity could not be mapped.");

            await _productAppService.Update(mapped);
            _logger.LogInformation($"Entity successfully added - HomeService");
        }

        public async Task DeleteProduct(ProductViewModel productViewModel)
        {
            var mapped = _mapper.Map<ProductModel>(productViewModel);
            if (mapped == null)
                throw new Exception($"Entity could not be mapped.");

            await _productAppService.Delete(mapped);
            _logger.LogInformation($"Entity successfully added - HomeService");
        }
    }
}
