using ST10261874_CLDV6212_POE.Models;
using ST10261874_CLDV6212_POE.Services;
using System;
using System.Threading.Tasks;

public class OrderService
{
    private readonly TableStorageService _tableStorageService;
    private readonly QueueService _queueService;

    public OrderService(TableStorageService tableStorageService, QueueService queueService)
    {
        _tableStorageService = tableStorageService;
        _queueService = queueService;
    }

    public async Task RegisterOrderAsync(Order order)
    {
        // Set necessary fields for the order
        order.Ordering_Date = DateTime.SpecifyKind(order.Ordering_Date, DateTimeKind.Utc);
        order.PartitionKey = "OrderingsPartition";
        order.RowKey = Guid.NewGuid().ToString();

        // Save the order to table storage
        await _tableStorageService.AddOrderAsync(order);

        // Retrieve the product to get the image name
        var product = await _tableStorageService.GetProductByIdAsync(order.Product_ID);

        // Ensure the product exists
        if (product == null)
        {
            throw new Exception("Product not found.");
        }

        // Get the image name from the product
        string imageName = product.ImageUrl;

        // Create the queue message with the image name
        string message = $"New order by Customer {order.Customer_ID} of Product {order.Product_ID} ({product.Product_Name}) on {order.Ordering_Date}. " +
                         $"Processing order. Uploading image: {product.ImageUrl}";

        // Send the message to the Azure Queue using the QueueService
        await _queueService.SendMessageAsync(message);
    }
}
