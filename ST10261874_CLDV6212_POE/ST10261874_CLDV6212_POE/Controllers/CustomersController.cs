using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ST10261874_CLDV6212_POE.Services;
using ST10261874_CLDV6212_POE.Models;
using Microsoft.AspNetCore.Authorization;
public class CustomersController : Controller
{
    private readonly TableStorageService _tableStorageService;

    // constructor that initializes the TableStorageService.
    public CustomersController(TableStorageService tableStorageService)
    {
        _tableStorageService = tableStorageService;
    }
    [Authorize]
    // action to display a list of all customers.
    public async Task<IActionResult> Index()
    {
        var customers = await _tableStorageService.GetAllCustomersAsync(); // retrieve all customers from Azure Table Storage.
        return View(customers); 
    }

    // action to display the customer creation form.
    public IActionResult Create()
    {
        return View(); // render the view for creating a new customer.
    }

    [HttpPost]
    public async Task<IActionResult> Create(Customer customer)
    {
        // assigning partition and row keys for the customer entity.
        customer.PartitionKey = "CustomersPartition";
        customer.RowKey = Guid.NewGuid().ToString(); 

        await _tableStorageService.AddCustomerAsync(customer); 
        TempData["Message"] = "Customer added successfully!"; 
        return RedirectToAction("Index"); 
    }

    // action to delete a specific customer identified by partition and row keys.
    public async Task<IActionResult> Delete(string partitionKey, string rowKey)
    {
        await _tableStorageService.DeleteCustomerAsync(partitionKey, rowKey);
        TempData["Message"] = "Customer deleted successfully!";
        return RedirectToAction("Index"); 
    }

    // action to display the details of a specific customer identified by partition and row keys.
    public async Task<IActionResult> Details(string partitionKey, string rowKey)
    {
        var customer = await _tableStorageService.GetCustomerAsync(partitionKey, rowKey);
        if (customer == null)
        {
            return NotFound(); // return a 404 Not Found response if the customer does not exist.
        }
        return View(customer);
    }
}