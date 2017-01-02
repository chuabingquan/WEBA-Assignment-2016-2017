using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cloudinary;
namespace OnlinePetShopManagementSystem.Models
{
    public class EmployeePhoto:ImageFile 
    {
				/// <summary>
				/// Class used to represent employee image uploaded to Cloudinary.
				/// This will also be saved to the WEBA_OnlinePetShopManagementSystemDB database
				/// </summary>

				/// <summary>
				/// Primary key of the database table
				/// </summary>
				public int EmployeePhotoId { get; set; }
        /// <summary>
        /// Navigation Property
        /// </summary>
        public Employee Employee { get; set; }
        /// <summary>
        /// Foreign Key
        /// </summary>
        public int EmployeeId { get; set; }


		}//End of EmployeePhoto class
}//End of namespace
