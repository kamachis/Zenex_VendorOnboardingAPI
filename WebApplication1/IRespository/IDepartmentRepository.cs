using static Zenex.Master.Models.Master;

namespace Zenex.Master.IRespository
{
    public interface IDepartmentRepository
    {
        List<CBPDepartment> GetAllDepartments();
        Task<CBPDepartment> CreateDepartment(CBPDepartment Department);
        Task<CBPDepartment> UpdateDepartment(CBPDepartment Department);
        Task<CBPDepartment> DeleteDepartment(CBPDepartment Department);
    }
}
