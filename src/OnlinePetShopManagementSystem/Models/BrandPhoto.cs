using Cloudinary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePetShopManagementSystem.Models
{
    public class BrandPhoto:ImageFile
    {
        public int BrandPhotoId { get; set; }

        //Navigation Property
        public Brand Brand { get; set; }

        //Foreign Key
        public int BrandId { get; set; }
    }
}
