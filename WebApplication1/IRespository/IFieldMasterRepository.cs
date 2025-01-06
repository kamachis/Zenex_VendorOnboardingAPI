using static Zenex.Master.Models.Master;

namespace Zenex.Master.IRespository
{
    public interface IFieldMasterRepository
    {
        List<CBPFieldMaster> GetAllOnBoardingFieldMaster();
        List<CBPIdentity> GetAllIdentityFields();
        Task<CBPFieldMaster> UpdateOnBoardingFieldMaster(CBPFieldMaster OnBoardingFieldMaster);
    }
}
