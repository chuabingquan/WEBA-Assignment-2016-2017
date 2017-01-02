using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePetShopManagementSystem.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        //Navigation Properties
        public List<CategoryProduct> CategoryProducts { get; set; }
        public Brand Brand { get; set; }
        public ProductPhoto ProductPhoto { get; set; }

        //Foreign Key
        public int BrandId { get; set; }

        //Created, Updated and Deleted At & By properties
        public DateTime CreatedAt { get; set; }
        //public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        //public string UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        //public string DeletedBy { get; set; }
    }
}
