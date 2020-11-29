using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Core.Entities;
using Demo.Core.Interfaces;
using Demo.Core.Repositories;
using Demo.Application.Models;
using Demo.Application.Mapper;
using Demo.Application.Interfaces;

namespace Demo.Application.Services
{
    // TODO : add validation , authorization, logging, exception handling etc. -- cross cutting activities in here.
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IAppLogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, IAppLogger<ProductService> logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ProductModel>> GetProducts()
        {
            var productList = await _productRepository.GetProducts();
            return ObjectMapper.Mapper.Map<IEnumerable<ProductModel>>(productList);
        }

        public async Task<ProductModel> GetProductById(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            return ObjectMapper.Mapper.Map<ProductModel>(product);
        }

        public async Task<IEnumerable<ProductModel>> GetProductByName(string productName)
        {
            var productList = await _productRepository.GetProductByName(productName);
            return ObjectMapper.Mapper.Map<IEnumerable<ProductModel>>(productList);
        }

        public async Task<IEnumerable<ProductModel>> GetProductByCategory(int categoryId)
        {
            var productList = await _productRepository.GetProductByCategory(categoryId);
            return ObjectMapper.Mapper.Map<IEnumerable<ProductModel>>(productList);
        }

        public async Task<ProductModel> Create(ProductModel productModel)
        {
            await ValidateProductIfExist(productModel);

            productModel.Created = DateTime.Now;
            productModel.LastUpdated = DateTime.Now;

            var mappedEntity = ObjectMapper.Mapper.Map<Product>(productModel);
            if (mappedEntity == null)
                throw new ApplicationException($"Entity could not be mapped.");

            var newEntity = await _productRepository.AddAsync(mappedEntity);
            _logger.LogInformation($"Entity successfully added.");

            return ObjectMapper.Mapper.Map<ProductModel>(newEntity);
        }

        public async Task Update(ProductModel productModel)
        {
            ValidateProductIfNotExist(productModel);

            productModel.LastUpdated = DateTime.Now;

            var editProduct = await _productRepository.GetByIdAsync(productModel.ProductId);
            if (editProduct == null)
                throw new ApplicationException($"Entity could not be loaded.");

            ObjectMapper.Mapper.Map(productModel, editProduct);

            await _productRepository.UpdateAsync(editProduct);
            _logger.LogInformation($"Entity successfully updated.");
        }

        public async Task Delete(ProductModel productModel)
        {
            ValidateProductIfNotExist(productModel);
            var deletedProduct = await _productRepository.GetByIdAsync(productModel.ProductId);
            if (deletedProduct == null)
                throw new ApplicationException($"Entity could not be loaded.");

            await _productRepository.DeleteAsync(deletedProduct);
            _logger.LogInformation($"Entity successfully deleted.");
        }

        private async Task ValidateProductIfExist(ProductModel productModel)
        {
            var existingEntity = await _productRepository.GetByIdAsync(productModel.ProductId);
            if (existingEntity != null)
                throw new ApplicationException($"{productModel} with this id already exists");
        }

        private void ValidateProductIfNotExist(ProductModel productModel)
        {
            var existingEntity = _productRepository.GetByIdAsync(productModel.ProductId);
            if (existingEntity == null)
                throw new ApplicationException($"{productModel} with this id is not exists");
        }
    }
}
