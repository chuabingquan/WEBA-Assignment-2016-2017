using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePetShopManagementSystem.Models
{
    public class CategoryProduct
    {
        public int CategoryProductId { get; set; }

        //Foreign Key Properties
        public int CategoryId { get; set; }
        public int ProductId { get; set; }

        //Navigation Properties
        public Category Category { get; set; }
        public Product Product { get; set; }

    }
}
