using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10261874_CLDV6212_POE.Models
{
    public class Product : ITableEntity
    {
        [Key]
        public int Product_Id { get; set; }
        public string? Product_Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }


        //ITableEntity implementation

        public string? RowKey { get; set; }

       // public ETag ETag { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string? PartitionKey { get; set ; }

        // string ITableEntity.PartitionKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        // string ITableEntity.RowKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        DateTimeOffset? ITableEntity.Timestamp { get; set; }
        ETag ITableEntity.ETag { get; set; }
        //  public DateTimeOffset? Timestamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
