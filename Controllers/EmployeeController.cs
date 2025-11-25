using lemco_id_core_api.Data;
using lemco_id_core_api.Interfaces;
using lemco_id_core_api.Models;
using lemco_id_core_api.Services;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(empMgr.UpdateEmployee(e));
        }

        [HttpOptions]
        public ActionResult<bool> UpdateEmployee()
        {
            return Ok(true);
        }

        [HttpPost]
        [Route("{id}/mark-as-printed")]
        public ActionResult<int> MarkAsPrinted(int id) {
            return Ok(empMgr.MarkAsPrinted(id));
        }



      
    }
}
