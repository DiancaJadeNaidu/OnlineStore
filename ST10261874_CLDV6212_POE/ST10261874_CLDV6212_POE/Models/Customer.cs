using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace ST10261874_CLDV6212_POE.Models
{
    public class Customer : ITableEntity
    {
        [Key]
        public int Customer_Id { get; set; }
        public string? Customer_Name { get; set; }
        public string? Customer_Email { get; set; }
        public string? Customer_Password { get; set; }  

        //ITableEntity implementation
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }

        public ETag ETag { get; set; }
       // public DateTimeOffset? TimeStamp { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}
