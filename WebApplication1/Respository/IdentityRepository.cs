
using static Zenex.Registration.Models.Registeration;
using Zenex.Registration.IRespository;
using Zenex.DBContext;
using Zenex.Registration.Models;
using System.Net;
using System.Net.Http;
using static Zenex.Authentication.Models.AuthMaster;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Newtonsoft.Json;
using System.Xml;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using Microsoft.Extensions.Primitives;
using static Zenex.Master.Models.Master;
using Zenex.Migrations;

namespace Zenex.Registration.Respository
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly ZenexContext _dbContext;
        private readonly IConfiguration _configuration;
        AttachmentRepository attachmentRepository;

        public IdentityRepository(ZenexContext dbContext)
        {
            _dbContext = dbContext;
            
            attachmentRepository = new AttachmentRepository(_dbContext);
        }

        //public List<BPIdentity> GetAllIdentities()
        //{
        //    try
        //    {
        //        return _dbContext.BPIdentities.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        
        public  List<GSTField> GetGstDetails()
        {
            try
            {
                var GstFields =  _dbContext.GSTFields.ToList();
                return GstFields;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong");
                
            }
        }
            
        
        private static bool validateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {

            return true;

        }
        public async Task<IdentityAttachments> GetIdentitiesDeleteByVOB(int TransID, string Doctype)
        {
            var deleteidentity = _dbContext.IdentityAttachments.Where(x => x.TransID == TransID && x.DocType == Doctype).FirstOrDefault();
            if (deleteidentity != null)
            {
                deleteidentity.AttachmentFile = null;
                deleteidentity.AttachmentName = null;
                deleteidentity.ContentType = null;
                deleteidentity.ContentLength = null;
                deleteidentity.Size = null;
                deleteidentity.Date = null;
                deleteidentity.IDNumber = null;
                await _dbContext.SaveChangesAsync();
            }
            return deleteidentity;
        }

        public List<IdentityAttachments> GetIdentitiesByVOB(int TransID)
        {
            try
            {
                  var result = _dbContext.IdentityAttachments.Where(x => x.TransID == TransID).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<BPIdentity> CreateIdentity(BPIdentity Identity)
        //{
        //    try
        //    {
        //        Identity.IsActive = true;
        //        Identity.CreatedOn = DateTime.Now;
        //        var result = _dbContext.BPIdentities.Add(Identity);
        //        await _dbContext.SaveChangesAsync();
        //        return Identity;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}




        //public async Task CreateIdentities(List<BPIdentity> Identities, int TransID)
        //{
        //    try
        //    {
        //        if (Identities != null && Identities.Count > 0)
        //        {
        //            foreach (BPIdentity Identity in Identities)
        //            {
        //                Identity.TransID = TransID;
        //                Identity.IsActive = true;
        //                Identity.CreatedOn = DateTime.Now;
        //                var result = _dbContext.BPIdentities.Add(Identity);
        //                if (!string.IsNullOrEmpty(Identity.AttachmentName))
        //                {
        //                    //BPAttachment BPAttachment = new BPAttachment();
        //                    //BPAttachment.ProjectName = "BPCloud";
        //                    //BPAttachment.AppID = 1;
        //                    //BPAttachment.AppNumber = result.Entity.Type;
        //                    //BPAttachment.IsHeaderExist = true;
        //                    //BPAttachment.HeaderNumber = TransID.ToString();
        //                    //BPAttachment.AttachmentName = Identity.AttachmentName;
        //                    //BPAttachment result1 = await attachmentRepository.AddAttachment(BPAttachment);
        //                    //result.Entity.DocID = result1.AttachmentID.ToString();

        //                    BPAttachment attachment = _dbContext.BPAttachments.Where(x => x.HeaderNumber == TransID.ToString() && x.AttachmentName == Identity.AttachmentName).FirstOrDefault();
        //                    if (attachment != null)
        //                    {
        //                        attachment.ProjectName = "BPCloud";
        //                        attachment.AppID = 1;
        //                        attachment.IsHeaderExist = true;
        //                        attachment.AppNumber = result.Entity.Type;
        //                        attachment.ProjectName = "BPCloud";
        //                        result.Entity.DocID = attachment.AttachmentID.ToString();
        //                    }
        //                }
        //            }
        //            await _dbContext.SaveChangesAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<BPIdentity> UpdateIdentity(BPIdentity Identity)
        //{
        //    try
        //    {
        //        var entity = _dbContext.Set<BPIdentity>().FirstOrDefault(x => x.TransID == Identity.TransID && x.Type == Identity.Type);
        //        if (entity == null)
        //        {
        //            return entity;
        //        }
        //        //_dbContext.Entry(Identity).State = EntityState.Modified;
        //        entity.IDNumber = Identity.IDNumber;
        //        entity.DocID = Identity.DocID;
        //        entity.AttachmentName = Identity.AttachmentName;
        //        //entity.ValidUntil = Identity.ValidUntil;
        //        //entity.IsValid = Identity.IsValid;
        //        entity.ModifiedBy = Identity.ModifiedBy;
        //        entity.ModifiedOn = DateTime.Now;
        //        await _dbContext.SaveChangesAsync();
        //        return Identity;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<BPIdentity> DeleteIdentity(BPIdentity Identity)
        //{
        //    try
        //    {
        //        //var entity = await _dbContext.Set<BPIdentity>().FindAsync(Identity.Identity, Identity.Language);
        //        var entity = _dbContext.Set<BPIdentity>().FirstOrDefault(x => x.TransID == Identity.TransID && x.Type == Identity.Type);
        //        if (entity == null)
        //        {
        //            return entity;
        //        }

        //        _dbContext.Set<BPIdentity>().Remove(entity);
        //        await _dbContext.SaveChangesAsync();
        //        return entity;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task DeleteIdentityByTransID(int TransID)
        //{
        //    try
        //    {
        //        _dbContext.Set<BPIdentity>().Where(x => x.TransID == TransID).ToList().ForEach(x => _dbContext.Set<BPIdentity>().Remove(x));
        //        await _dbContext.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


    }
}
