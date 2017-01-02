using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OnlinePetShopManagementSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public DateTime JoinDate { get; set; }		      
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        //Foreign Key Properties to support the 
       //CreatedBy, UpdatedBy and Deletedby
        //navigation properties
        public string CreatedById { get; set; }
        public string UpdatedById { get; set; }
        public string DeletedById { get; set; }
        //Navigation Properties to pull in information 
        //regarding the users (e.g. Full Name)
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public ApplicationUser DeletedBy { get; set; }

        //Navigation Property
        public EmployeePhoto EmployeePhoto { get; set; }
    }
}

