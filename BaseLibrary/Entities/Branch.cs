﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Branch :BaseEntity
    {
        public GeneralDepartment? Department { get; set; }
        public int DepartmentId { get; set; }
    }
}
