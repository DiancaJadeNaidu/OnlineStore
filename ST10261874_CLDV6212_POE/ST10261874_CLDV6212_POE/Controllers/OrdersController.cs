using ST10261874_CLDV6212_POE.Models;
using Microsoft.AspNetCore.Mvc;
using ST10261874_CLDV6212_POE.Services;

public class OrdersController : Controller
{
    private readonly TableStorageService _tableStorageService;
    private readonly QueueService _queueService;

    public OrdersController(TableStorageService tableStorageService, QueueService queueService)
    {
        _tableStorageService = tableStorageService;
        _queueService = queueService;
    }

    //action to display all orders
    public async Task<IActionResult> Index()
    {
        var orders = await _tableStorageService.GetAllOrdersAsync();
        return View(orders);
    }

    public async Task<IActionResult> Register()
    {
        var customers = await _tableStorageService.GetAllCustomersAsync();
        var products = await _tableStorageService.GetAllProductsAsync();

        // Check for null or empty lists
        if (customers == null || !customers.Any())
        {
            ModelState.AddModelError("", "No customers found. Please add customers first.");
            return View();
        }
        if (products == null || !products.Any())
        {
            ModelState.AddModelError("", "No products found. Please add products first.");
            return View();
        }

        // Pass customers and products to the view
        ViewData["Customers"] = customers;
        ViewData["Products"] = products;

        return View();
    }


    //action to handle the form submission and register the orders
    [HttpPost]
    public async Task<IActionResult> Register(Order order)
    {
        if (ModelState.IsValid)
        {
            order.Ordering_Date = DateTime.SpecifyKind(order.Ordering_Date, DateTimeKind.Utc);
            order.PartitionKey = "OrderingsPartition";
            order.RowKey = Guid.NewGuid().ToString();

            await _tableStorageService.AddOrderAsync(order);

            string message = $"New order by Customer {order.Customer_ID} of Product {order.Product_ID} on {order.Ordering_Date}.";
            await _queueService.SendMessageAsync(message);

            TempData["Message"] = "Order registered successfully!";
            TempData["QueueMessage"] = "Order message sent to the queue.";
            return RedirectToAction("Index");
        }

        // Handle model validation errors
        TempData["ErrorMessage"] = "Failed to register order. Please correct the errors.";
        return View(order);
    }

}

