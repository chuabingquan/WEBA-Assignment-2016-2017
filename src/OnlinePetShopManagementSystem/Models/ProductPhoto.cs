using Cloudinary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePetShopManagementSystem.Models
{
    public class ProductPhoto:ImageFile
    {
        public int ProductPhotoId { get; set; }

        //Navigation Property
        public Product Product { get; set; }

        //Foreign Key
        public int ProductId { get; set; }
    }
}
