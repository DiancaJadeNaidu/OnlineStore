using ST10261874_CLDV6212_POE.Models;
using Microsoft.AspNetCore.Mvc;
using ST10261874_CLDV6212_POE.Services;

public class ProductsController : Controller
    {
    private readonly BlobService _blobService;
    private readonly TableStorageService _tableStorageService;

    public ProductsController(BlobService blobService, TableStorageService tableStorageService)
    {
        _blobService = blobService;
        _tableStorageService = tableStorageService;
    }

    [HttpGet]
    public IActionResult AddProduct()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(Product product, IFormFile file)
    {
        if (file != null)
        {
            using var stream = file.OpenReadStream();
            var imageUrl = await _blobService.UploadAsync(stream, file.FileName);
            product.ImageUrl = imageUrl;
        }

        if (ModelState.IsValid)
        {
            product.PartitionKey = "ProductsPartition";
            product.RowKey = Guid.NewGuid().ToString();
            await _tableStorageService.AddProductAsync(product);
            TempData["Message"] = "Product added successfully!";
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = "Failed to add product. Please correct the errors.";
        return View(product);
    }


    [HttpGet]
    public IActionResult Edit()
    {
       
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Product product, IFormFile file)
    {
        if (file != null)
        {
            using var stream = file.OpenReadStream();
            var imageUrl = await _blobService.UploadAsync(stream, file.FileName);
            product.ImageUrl = imageUrl;
        }

        if (ModelState.IsValid)
        {
            await _tableStorageService.UpdateProductAsync(product);
            TempData["Message"] = "Product updated successfully!";
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = "Failed to update product. Please correct the errors.";
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string partitionKey, string rowKey)
    {
        var product = await _tableStorageService.GetProductAsync(partitionKey, rowKey);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProduct(string partitionKey, string rowKey)
    {
        await _tableStorageService.DeleteProductAsync(partitionKey, rowKey);
        TempData["Message"] = "Product deleted successfully!";
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Index()
    {
        var products = await _tableStorageService.GetAllProductsAsync();
        return View(products);
    }

}
