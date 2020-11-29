using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Api.Models;
using Demo.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController :ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger, IMapper mapper)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<CategoryApiModel>>> GetCategories()
        {
            _logger.LogInformation("API Call: Get Categories");
            return Ok(_mapper.Map<IEnumerable<CategoryApiModel>>(await _categoryService.GetCategories()));
        }


        [HttpGet("{categoryId}")]
        public async Task<ActionResult<CategoryApiModel>> GetCategory(int categoryId)
        {
            _logger.LogInformation($"API Call: Get Category By Id: {categoryId}");
            var product = await _categoryService.GetCategories();
            if (product == null)
            {
                _logger.LogInformation($"API Call: Category Not Found. CategoryId : {categoryId}");
                return NotFound();
            }

            return Ok(_mapper.Map<ProductApiModel>(product));
        }
    }
}
