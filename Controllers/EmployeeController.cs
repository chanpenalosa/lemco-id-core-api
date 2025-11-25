using lemco_id_core_api.Data;
using lemco_id_core_api.Interfaces;
using lemco_id_core_api.Models;
using lemco_id_core_api.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;
using System.Diagnostics;

namespace lemco_id_core_api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private IEmployeeRepository _SqlCtx;
        private EmployeeServices empMgr;
        public EmployeeController(EmployeeMySqlDbContext employeeMySqlDbContext)
        {

            _SqlCtx = employeeMySqlDbContext;
            empMgr = new EmployeeServices(_SqlCtx);
        }

        [HttpGet]
        public ActionResult<ResponseFormat> GetEmployees([FromQuery] int size = 20, [FromQuery] string searchKey = "")
        {
            ResponseFormat res = empMgr.GeToptEmployees(size, searchKey);
            return Ok(res);
        }

        // This action handles GET requests to api/add/employee/{id}
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployeeByID(int id) {
            return Ok(empMgr.GetEmployeeById(id));
        }

        [HttpPatch]
        public ActionResult<bool> UpdateEmployee([FromBody] Employee e) {
            if (empMgr.UpdateEmployee(e)) return Json(new { success = true });
            return StatusCode(500, Json(new { success=false}));
        }

        [HttpPost]
        [Route("{id}/mark-as-printed")]
        public ActionResult<int> MarkAsPrinted(int id) {
            if (empMgr.MarkAsPrinted(id)) return Json(new { success = true });
            return StatusCode(500, Json(new { success = false }));
        }

        [HttpGet("{id}/image")]
        public IActionResult GetImage(int id) {

            string path = "D:\\dev\\lemco\\Photo\\";
            var imgPath = Path.Combine(path, id.ToString()+ ".JPG");
;
            if (!System.IO.File.Exists(imgPath)) imgPath = Path.Combine(path,"1.JPG");
            
            var imageFile = new FileStream(imgPath, FileMode.Open, FileAccess.Read);   
            return File(imageFile,"image/JPG");
        }
      
    }
}
