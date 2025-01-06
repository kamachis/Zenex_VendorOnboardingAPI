

using static Zenex.Registration.Models.Registeration;

namespace Zenex.Registration.IRespository
{
    public interface IContactRepository
    {
        List<BPContact> GetAllContacts();
        List<BPContact> GetContactsByVOB(int TransID);
        Task<BPContact> CreateContact(BPContact Contact);
        Task CreateContacts(List<BPContact> Contacts, int TransID);
        Task<BPContact> UpdateContact(BPContact Contact);
        Task<BPContact> DeleteContact(BPContact Contact);
        Task DeleteContactByTransID(int TransID);
    }
}
