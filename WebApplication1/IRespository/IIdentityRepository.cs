

using static Zenex.Master.Models.Master;
using static Zenex.Registration.Models.Registeration;

namespace Zenex.Registration.IRespository
{
    public interface IIdentityRepository
    {
      //  List<BPIdentity> GetAllIdentities();
        
        List<GSTField> GetGstDetails();
        
        List<IdentityAttachments> GetIdentitiesByVOB(int TransID);
        Task<IdentityAttachments> GetIdentitiesDeleteByVOB(int TransID, string Doctype);
        //Task<BPIdentity> CreateIdentity(BPIdentity Identity);
        ////Task CreateIdentities(List<BPIdentity> Identities, int TransID);
        //Task<BPIdentity> UpdateIdentity(BPIdentity Identity);
        //Task<BPIdentity> DeleteIdentity(BPIdentity Identity);
        // Task DeleteIdentityByTransID(int TransID);
    }
}
