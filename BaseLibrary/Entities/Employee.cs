using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Employee:BaseEntity
    {
        [Required]
        
        public string? CicilId { get; set; } = string.Empty;
        [Required]
        public string? FileNumber { get; set; } = string.Empty;
        [Required]
        public string? Fullname { get; set; } = string.Empty;
        [Required]
        public string? JobName { get; set; } = string.Empty;
        [Required]
        public string? Address { get; set; } = string.Empty;
        [Required,DataType(DataType.PhoneNumber)]  
        public string? TelephoneNumber { get; set; } = string.Empty;
        [Required]
        public string? Photo { get; set; } = string.Empty;
        public string? Other { get; set; } 


        //Relationship:many to One
        //public GeneralDepartment? generalDepartment { get; set; }
        //public int GeneralDepartmentId { get; set; }
        //public Department? department { get; set; }
        //public int DepartmentId { get; set; }
        public Branch? Branch { get; set; }
        public int BranchId { get; set; }
        public Town? Town { get; set; }
        public int TownId
        {
            get; set;

        }
    }
}
