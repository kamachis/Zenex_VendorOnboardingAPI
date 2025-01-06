using static Zenex.Master.Models.Master;

namespace Zenex.Master.IRespository
{
    public interface IPostalRepository
    {
        List<CBPPostal> GetAllPostals();
        Task<CBPPostal> CreatePostal(CBPPostal Postal);
        Task<CBPPostal> UpdatePostal(CBPPostal Postal);
        Task<CBPPostal> DeletePostal(CBPPostal Postal);
    }
}
