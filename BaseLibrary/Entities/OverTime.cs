﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class OverTime : OtherBaseEntity
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public int NumberOfDays => (EndDate- StartDate).Days;
        public OverTimeType? OverTimeType { get; set; }
        [Required]
        public int OverTimeTypeId {  get; set; }

    }
}
