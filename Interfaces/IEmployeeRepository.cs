using lemco_id_core_api.Models;

namespace lemco_id_core_api.Interfaces
{
    public interface IEmployeeRepository
    {
        Employee GetByID(int id);
        ResponseFormat GeToptEmployees(int size, string searchKey);
        bool UpdateEmployee(Employee e);
        bool MarkAsPrinted(int id);
    }
}
