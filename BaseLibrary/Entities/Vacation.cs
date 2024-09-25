using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Vacation : OtherBaseEntity
    {
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public int NumberOfDay { get; set; }
        public DateTime EndDate => StartTime.AddDays(NumberOfDay);
        public VacationType? VacationType { get; set; }

        [Required]
        public int VacationTypeId { get; set; }
    }
}
