using static Zenex.Master.Models.Master;

namespace Zenex.Master.IRespository
{
    public interface ITypeRepository
    {
        List<CBPType> GetAllTypes();
        Task<CBPType> CreateType(CBPType type);
        Task<CBPType> UpdateType(CBPType type);
        Task<CBPType> DeleteType(CBPType type);
    }
}
