using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Reponses
{
    public record GeneralResponse(bool Flag, string Message = null!);
    
}
