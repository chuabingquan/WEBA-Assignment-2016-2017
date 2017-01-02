using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePetShopManagementSystem.Models
{
    public class CategoryBrand
    {
        public int CategoryBrandId { get; set; }

        //Navigation Properties
        public Brand Brand { get; set; }
        public Category Category { get; set; }

        //Foreign Key Properties
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
    }
}
