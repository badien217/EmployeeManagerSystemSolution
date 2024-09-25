using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Reponsitories.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<T>(IGenericReponsitoryInterface<T> genericReponsitoryInterface) : Controller where T : class 
    {
        [HttpGet("All")]
        public async Task<IActionResult> GetAll() => Ok(await genericReponsitoryInterface.GetAll());
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid request send");
            return Ok(await genericReponsitoryInterface.DeleteById(id));
        }
        [HttpGet("signle/{id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            if (Id <= 0) return BadRequest("Invalid request send");
            return Ok(await genericReponsitoryInterface.GetById(Id));
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add(T model)
        {
            if (model is null) return BadRequest("Invalid request send");
            return Ok(await genericReponsitoryInterface.Insert(model));
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update(T model)
        {
            if (model is null) return BadRequest("Invalid request send");
            return Ok(await genericReponsitoryInterface.Update(model));
        }

    }
}
