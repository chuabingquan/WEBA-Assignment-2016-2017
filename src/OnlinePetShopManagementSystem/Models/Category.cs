using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePetShopManagementSystem.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Visibility { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsSpecial { get; set; }


        //Navigation Property
        public List<CategoryBrand> CategoryBrands { get; set; }
        public List<CategoryProduct> CategoryProducts { get; set; }


        //Created, Updated and Deleted At & By properties
        public DateTime CreatedAt { get; set; }
        //public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        //public string UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        //public string DeletedBy { get; set; }
    }
}
