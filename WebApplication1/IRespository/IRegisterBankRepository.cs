using static Zenex.Master.Models.Master;

namespace Zenex.Master.IRespository
{
    public interface IRegisterBankRepository
    {
        List<CBPBank> GetAllBanks();
        CBPBank GetBankByIFSC(string IFSC);
        Task<CBPBank> CreateBank(CBPBank Bank);
        Task<CBPBank> UpdateBank(CBPBank Bank);
        Task<CBPBank> DeleteBank(CBPBank Bank);
    }
}
