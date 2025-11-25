using lemco_id_core_api.Interfaces;
using lemco_id_core_api.Models;
using ZstdSharp.Unsafe;

namespace lemco_id_core_api.Services
{
    public class EmployeeServices
    {

        IEmployeeRepository _repo;

        public EmployeeServices(IEmployeeRepository repo) { 
            _repo = repo;
        }

        public Employee GetEmployeeById(int id) {
           return _repo.GetByID(id);
        }

        public ResponseFormat GeToptEmployees(int size, string searchKey) {
            return _repo.GeToptEmployees(size, searchKey);
        }

        public bool UpdateEmployee(Employee e) {
            return _repo.UpdateEmployee(e);   
        }

        public bool MarkAsPrinted(int id) {
            return _repo.MarkAsPrinted(id);
        }

        public PrintLogs GetPrintLogs() { 
            return _repo.GetPrintLogs();
        }
    }
}
