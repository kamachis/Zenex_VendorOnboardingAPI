using static Zenex.Master.Models.Master;

namespace Zenex.Master.IRespository
{
    public interface IAppRepository
    {
        List<CBPApp> GetAllApps();
        Task<CBPApp> CreateApp(CBPApp App);
        Task<CBPApp> UpdateApp(CBPApp App);
        Task<CBPApp> DeleteApp(CBPApp App);
    }
}
