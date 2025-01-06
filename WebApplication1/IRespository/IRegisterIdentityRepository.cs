using static Zenex.Master.Models.Master;

namespace Zenex.Master.IRespository
{
    public interface IRegisterIdentityRepository
    {
        List<CBPIdentity> GetAllIdentities();
        List<string> GetAllIdentityTypes();
        CBPIdentity GetIdentityByType(string Type);
        CBPIdentity ValidateIdentityByType(string Type);
        Task<CBPIdentity> CreateIdentity(CBPIdentity Identity);
        Task<CBPIdentity> UpdateIdentity(CBPIdentity Identity);
        Task<CBPIdentity> DeleteIdentity(CBPIdentity Identity);
    }
}
