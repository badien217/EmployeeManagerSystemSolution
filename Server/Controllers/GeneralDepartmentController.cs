using BaseLibrary.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Reponsitories.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralDepartmentController(IGenericReponsitoryInterface<GeneralDepartment> genericReponsitoryInterface) 
        : GenericController<GeneralDepartment>(genericReponsitoryInterface)
    {
    }
}
