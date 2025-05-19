using Microsoft.AspNetCore.Mvc;
using ST10261874_CLDV6212_POE.Models;
using ST10261874_CLDV6212_POE.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FilesController : Controller
{
    private readonly AzureFileShareService _fileShareService;
    //private readonly AzureFunctionService _azureFunctionService; // For calling Azure Functions

    // Constructor to initialize AzureFileShareService and AzureFunctionService
    public FilesController(AzureFileShareService fileShareService)
    {
        _fileShareService = fileShareService;
    
    }

    // Action to display a list of files in the "uploads" directory
    public async Task<IActionResult> Index()
    {
        List<FileModel> files;
        try
        {
            files = await _fileShareService.ListFilesAsync("uploads"); // Get the list of files from Azure File Share
        }
        catch (Exception ex)
        {
            ViewBag.Message = $"Failed to load files: {ex.Message}"; // Handle any errors that occur while fetching files
            files = new List<FileModel>();
        }
        return View(files); // Return the list of files to the view
    }

    // Action to handle file upload
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["ErrorMessage"] = "Please select a file to upload."; // Error if no file is selected
            return RedirectToAction("Index");
        }

        try
        {
            using (var stream = file.OpenReadStream()) // Open a stream to read the file
            {
                string directoryName = "uploads";
                string fileName = file.FileName;

                // Upload the file to Azure File Share
                await _fileShareService.UploadFileAsync(directoryName, fileName, stream);

              
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"File upload failed: {ex.Message}"; // Handle any errors during upload
        }

        return RedirectToAction("Index");
    }

    // Action to handle file download
    [HttpGet]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return BadRequest("File name cannot be null or empty"); // Validate that file name is provided
        }

        try
        {
            var fileStream = await _fileShareService.DownloadFileAsync("uploads", fileName); // Download the file from Azure File Share

            if (fileStream == null)
            {
                return NotFound($"File '{fileName}' not found"); // Return 404 if file not found
            }

            return File(fileStream, "application/octet-stream", fileName); // Return file as a downloadable response
        }
        catch (Exception ex)
        {
            return BadRequest($"Error downloading file: {ex.Message}"); // Handle any errors during download
        }
    }

    // New Action to log file metadata to Azure Table via an Azure Function
    public async Task<IActionResult> LogFileMetadata(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return BadRequest("File name cannot be null or empty");
        }

        //try
        //{
        //    // Call Azure Function to log metadata in Azure Table
        //  //  var functionResponse = await _azureFunctionService.CallTableOperationsAsync(new
        //    {
        //        fileName = fileName,
        //        timestamp = DateTime.UtcNow
        //    });

        //    if (functionResponse.IsSuccessStatusCode)
        //    {
        //        TempData["Message"] = "File metadata logged successfully!";
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = "Error logging file metadata.";
        //    }
        //}
        //catch (Exception ex)
        //{
        //    TempData["ErrorMessage"] = $"Failed to log metadata: {ex.Message}";
        //}

        return RedirectToAction("Index");
    }
}
