using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace ST10261874_CLDV6212_POE.Models
{
    public class Order : ITableEntity
    {
        [Key]
        public int Order_Id { get; set; }   
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
      //  public DateTimeOffset? TimeStamp { get; set; }
        public DateTimeOffset? Timestamp { get ; set ; }
        public ETag ETag { get; set; }

        //introduce validation sample
        [Required(ErrorMessage ="Please select a Customer")]
        public string Customer_ID { get; set; } //foreign key to the customer who made the order

        [Required(ErrorMessage = "Please select a Product")]
      //  public string Customer_Name { get; set; } 
        public string Product_ID { get; set; } //foreign key to the product being ordered

        [Required(ErrorMessage = "Please select the date")]
        public DateTime Ordering_Date { get; set; }



    }
}
