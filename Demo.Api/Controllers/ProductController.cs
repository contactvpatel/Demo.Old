using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Api.Models;
using Demo.Application.Interfaces;
using Demo.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, ILogger<ProductController> logger, IMapper mapper)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<ProductApiModel>>> GetProducts()
        {
            _logger.LogInformation("Api Call: Get Products");
            return Ok(_mapper.Map<IEnumerable<ProductApiModel>>(await _productService.GetProducts()));
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductApiModel>> GetProduct(int productId)
        {
            _logger.LogInformation($"Api Call: Get Product By Id: {productId}");
            var product = await _productService.GetProductById(productId);
            if (product == null)
            {
                _logger.LogInformation($"Api Call: Product Not Found. ProductId : {productId}");
                return NotFound();
            }

            return Ok(_mapper.Map<ProductApiModel>(product));
        }

        [HttpPost]
        public async void Post([FromBody] ProductApiModel productApiModel)
        {
            _logger.LogInformation($"Api Call: Post Product");
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value);
            productApiModel.CreatedBy = userId;
            productApiModel.LastUpdatedBy = userId;
            await _productService.Create(_mapper.Map<ProductModel>(productApiModel));
        }

        [HttpPut]
        public async void Put([FromBody] ProductApiModel productApiModel)
        {
            _logger.LogInformation($"Api Call: Put Product : {productApiModel.ProductId}");
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value);
            productApiModel.CreatedBy = userId;
            productApiModel.LastUpdatedBy = userId;
            await _productService.Update(_mapper.Map<ProductModel>(productApiModel));
        }

        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            _logger.LogInformation($"Api Call: Delete Product : {id}");
            var productModel = await _productService.GetProductById(id);
            await _productService.Delete(productModel);
        }
    }
}
