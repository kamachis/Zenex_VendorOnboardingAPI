using static Zenex.Master.Models.Master;
using static Zenex.Registration.Models.Registeration;

namespace Zenex.Registration.IRespository
{
    public interface IAttachmentRepository
    {
        //Task DeleteAttachment(string AppNumber, string HeaderNumber);
        //Task<BPAttachment> AddAttachment(BPAttachment BPAttachment);
        //Task<BPAttachment> UpdateAttachment(BPAttachment BPAttachment);
        Task<IdentityAttachments> UploadUserAttachment(IdentityAttachments BPAttachment);
        //List<BPAttachment> FilterAttachments(string ProjectName, int AppID = 0, string AppNumber = null);
        //BPAttachment FilterAttachment(string ProjectName, string AttchmentName, int AppID = 0, string AppNumber = null, string HeaderNumber = null);
        //BPAttachment GetAttachmentByName(string AttachmentName);
        //BPAttachment GetAttachmentByID(int AttachmentID);

        //Task<BPAttachment> AddDeclerationAttachment(BPAttachment attachmentData, string TransID, string Name);
        //BPAttachment GetIdentityAttachment(string AppNumber, string HeaderNumber, string AttachmentName);

        //void UpdateDecelrationId(BPAttachment attachmentData, string TransID, string Name);
        //BPAttachment GetAttachmentByIDAndName(string HeaderNumber, string AttachmentName);
    }
}
