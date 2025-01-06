using static Zenex.Master.Models.Master;

namespace Zenex.Master.IRespository
{
    public interface ITitleRepository
    {
        List<CBPTitle> GetAllTitles();
        Task<CBPTitle> CreateTitle(CBPTitle Title);
        Task<CBPTitle> UpdateTitle(CBPTitle Title);
        Task<CBPTitle> DeleteTitle(CBPTitle Title);
    }
}
