using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePetShopManagementSystem.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int NoOfProducts { get; set; }


        //Navigation Property
        public List<CategoryBrand> CategoryBrands { get; set; }
        public List<Product> Products { get; set; }
        public BrandPhoto BrandPhoto { get; set; }

        //Created, Updated and Deleted At & By properties
        public DateTime CreatedAt { get; set; }
        //public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        //public string UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        //public string DeletedBy { get; set; }
    }
}
