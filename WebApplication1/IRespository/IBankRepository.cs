

using static Zenex.Registration.Models.Registeration;

namespace Zenex.Registration.IRespository
{
    public interface IBankRepository
    {
        List<BPBank> GetAllBanks();
        List<BPBank> GetBanksByVOB(int TransID);
        Task<BPBank> CreateBank(BPBank Bank);
        Task<bool> CreateBanks(BPBank Banks);
        Task<BPBank> UpdateBank(BPBank Bank);
        Task<BPBank> DeleteBank(BPBank Bank);
        Task DeleteBankByTransID(int TransID);
    }
}
