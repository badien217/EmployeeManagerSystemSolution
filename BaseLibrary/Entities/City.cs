using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class City:BaseEntity
    {
        public Country? Country { get; set; }
        public int CountryId { get; set; }
        public List<Town>? Towns { get; set; }
    }
}
