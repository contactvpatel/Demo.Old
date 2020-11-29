using System;
using System.Threading.Tasks;
using Demo.UI.Interfaces;
using Demo.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace Demo.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"Get Products");
            return View(await _productService.GetProducts());
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation($"Get Product By Id: {id}");
            var productViewModel = await _productService.GetProductById(id);
            if (productViewModel == null)
            {
                return NotFound();
            }
            return View(productViewModel);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _productService.GetCategories(), "CategoryId", "CategoryName");
            return View(new ProductViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            await _productService.CreateProduct(productViewModel);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation($"Get Product By Id: {id}");
            var productViewModel = await _productService.GetProductById(id);
            if (productViewModel == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(await _productService.GetCategories(), "CategoryId", "CategoryName");
            return View(productViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateProduct(productViewModel);
                return RedirectToAction("Index");
            }
            return View(productViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var productViewModel = await _productService.GetProductById(id);
            await _productService.DeleteProduct(productViewModel);
            return RedirectToAction("Index");
        }
    }
}
