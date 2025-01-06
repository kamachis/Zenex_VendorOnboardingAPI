using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using Zenex.DBContext;
using System.Collections.Generic;
using static Zenex.Registration.Models.Registeration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Zenex.Registration.IRespository;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using System.Diagnostics.Metrics;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.ComponentModel;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using static Zenex.Master.Models.Master;
using Microsoft.EntityFrameworkCore.Internal;
using AutoMapper;

namespace Zenex.Registration.Respository
{
   
        public class VendorOnBoardingRepository : IVendorOnBoardingRepository
    {
            private readonly ZenexContext _dbContext;
            IConfiguration _configuration;
            private object user;
            private int _tokenTimespan = 30;

            public VendorOnBoardingRepository(ZenexContext dbContext, IConfiguration configuration)
            {
                _dbContext = dbContext;
                _configuration = configuration;
                try
                {
                    if (_configuration["TokenTimeSpan"] != "")
                        _tokenTimespan = Convert.ToInt32(_configuration["TokenTimeSpan"].ToString());
                    if (_tokenTimespan <= 0)
                    {
                        _tokenTimespan = 30;
                    }
                }
                catch
                {
                    _tokenTimespan = 30;
                }
            }

            public List<BPVendorOnBoarding> GetAllVendorOnBoardings()
            {
                try
                {
                    return _dbContext.BPVendorOnBoardings.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardings()
            {
                try
                {
                    return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "registered").ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<BPVendorOnBoardingWithSapTransID> GetAllApprovedVendorOnBoardings()
            {
                try
                {
                /*var vendorOnboardings = _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "approved").ToList();
                var vendorDetails = _dbContext.VendorDetails.ToList();
                List<BPVendorOnBoardingWithSapTransID> res = new List<BPVendorOnBoardingWithSapTransID>();
                foreach (var vob in vendorOnboardings)
                {
                    var vd = vendorDetails.FirstOrDefault(x => int.Parse(x.TransID) == vob.TransID);
                    if (vd != null)
                    {
                        res.Add(new BPVendorOnBoardingWithSapTransID { vob = vob, TransID_SAP = vd.TransID_SAP });
                    }
                    else
                    {
                        res.Add(new BPVendorOnBoardingWithSapTransID { vob = vob, TransID_SAP = "0" });
                    }
                }*/

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<BPVendorOnBoarding, BPVendorOnBoardingWithSapTransID>();
                });
                var mapper = config.CreateMapper();

                var vendorDetails = _dbContext.VendorDetails
                    .ToList(); // Bring the data into memory

                var res = _dbContext.BPVendorOnBoardings
                    .Where(x => x.Status.ToLower() == "approved")
                    .ToList() // Bring the data into memory
                    .Select(vob =>
                    {
                        var derivedObj = mapper.Map<BPVendorOnBoardingWithSapTransID>(vob);
                        derivedObj.TransID_SAP = vendorDetails.FirstOrDefault(v => int.Parse(v.TransID) == vob.TransID)?.TransID_SAP ?? "0";
                        return derivedObj;
                    })
                    .ToList();

                //return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "approved").ToList();
                return res.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardings()
            {
                try
                {
                    return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "rejected").ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //public string GetVirtualPath(string physicalPath)
            //{
            //    try
            //    {
            //        if (!physicalPath.StartsWith(HttpContext.))
            //        {
            //            return null;
            //        }
            //        return "~/" + physicalPath.Substring(HttpContext.Current.Request.PhysicalApplicationPath.Length)
            //              .Replace("\\", "/");
            //        return physicalPath.Substring(HttpContext.Current.Request.PhysicalApplicationPath.Length)
            //             .Replace("\\", "/");
            //    }
            //    catch (Exception ex)
            //    {
            //        WriteLog.WriteToFile("AuthorizationServerProvider/GetVirtualPath/Exception:- " + ex.Message);
            //        return null;
            //    }
            //}
            //public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsByPlant(List<string> Plants)
            //{
            //    try
            //    {
            //        TimeSpan ts = new TimeSpan(30, 0, 0, 0);
            //        var subractedTime = DateTime.Now.Subtract(ts);
            //        var result = _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "registered" && Plants.Any(y => y == x.Plant) && x.ModifiedOn >= subractedTime).ToList();
            //        //var result = _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "registered" && Plants.Any(y => y == x.Plant)).ToList();
            //        return result;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsByApprover(string Approver)
            {
                try
                {
                    var result = _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "Initialized").ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsByPending(string Approver)
        {
            try
            {
                var result = _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "Registered");
                var res1 = result.ToList();
                return res1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardingsByPlant(List<string> Plants)
        //{
        //    try
        //    {

        //        return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "approved" && Plants.Any(y => y == x.Plant)).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardingsByApprover(string Approver)
        //{
        //    try
        //    {

        //        return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "approved" && x.EmamiContactPerson == Approver).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardingsByPlant(List<string> Plants)
        //{
        //    try
        //    {
        //        return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "rejected" && Plants.Any(y => y == x.Plant)).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardingsByApprover(string Approver)
        //{
        //    try
        //    {
        //        return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "rejected" && x.EmamiContactPerson == Approver).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsCount()
            {
                try
                {
                    return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "Initialized").ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsCountByApprover(string Approver)
            //{
            //    try
            //    {
            //        return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "registered" && x.EmamiContactPerson == Approver).ToList();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            public List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardingsCount()
            {
                try
                {
                    return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "approved").ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardingsCount()
            {
                try
                {
                    return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "rejected").ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsCountByPlant(List<string> Plants)
            //{
            //    try
            //    {
            //        return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "registered" && Plants.Any(y => y == x.Plant)).ToList();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            //public List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardingsCountByPlant(List<string> Plants)
            //{
            //    try
            //    {
            //        return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "approved" && Plants.Any(y => y == x.Plant)).ToList();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            //public List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardingsCountByPlant(List<string> Plants)
            //{
            //    try
            //    {
            //        return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "rejected" && Plants.Any(y => y == x.Plant)).ToList();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            public BPVendorOnBoarding GetVendorOnBoardingsByID(int TransID)
            {
                try
                {
                    return _dbContext.BPVendorOnBoardings.Where(x => x.TransID == TransID).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //public List<BPIdentity> GetAllIdentity(int TransID)
            //{
            //    try
            //    {
            //        return _dbContext.BPIdentities.Where(x => x.TransID == TransID).ToList();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
           
        //public BPAttachment GetAttachmentFile(int TransID, string attachmentname)
            //public async Task<BPVendorOnBoarding> InitializeVendorRegistration(VendorInitialzationClass vendorInitialzationClass)
            //public List<BPAttachment> GetAttachmentFile(int TransID, string attachmentname)
            //{
            //    //TransID = 3027;
            //    //attachmentname = "BP.jpg";

            //    var result = (from tb in _dbContext.BPAttachments
            //                      //join tb1 in _dbContext.BPIdentity on tb.AttachmentName equals tb1.AttachmentName
            //                  where TransID.ToString() == tb.HeaderNumber
            //                      &&
            //                   attachmentname == tb.AttachmentName
            //                  select new BPAttachment()
            //                  {
            //                      AttachmentFile = tb.AttachmentFile,
            //                      AttachmentName = tb.AttachmentName
            //                  }).ToList();
            //    return result;
            //}
          
        //public List<BPAttachment1> GetAttachmentforXML(int transid)
            //{
            //    try
            //    {
            //        var Identitys = (from tb in _dbContext.BPAttachments
            //                         join tb1 in _dbContext.BPIdentities
            //                         on tb.AttachmentName equals tb1.AttachmentName
            //                         where tb.AppNumber == tb1.Type
            //                         && tb1.TransID.ToString() == tb.HeaderNumber
            //                         && tb1.TransID == transid
            //                         select new BPAttachment1()
            //                         {
            //                             AttachmentFile = tb.AttachmentFile,
            //                             AttachmentName = tb.AttachmentName,
            //                             Type = tb1.Type
            //                         }).ToList();
            //        return Identitys;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            //public List<FTPAttachment> GetAllAttachmentsToFTP(BPVendorOnBoarding bPVendorOnBoarding)
            //{
            //    List<FTPAttachment> allAttachments = new List<FTPAttachment>();
            //    try
            //    {
            //        List<FTPAttachment> IndentityFTPAttachments = GetIdentityFTPAttachment(bPVendorOnBoarding.TransID);
            //        allAttachments.AddRange(IndentityFTPAttachments);
            //        //List<FTPAttachment> BankFTPAttachments = GetBankFTPAttachment(bPVendorOnBoarding.TransID);
            //        //allAttachments.AddRange(BankFTPAttachments);
            //        if (!string.IsNullOrEmpty(bPVendorOnBoarding.MSME_Att_ID))
            //        {
            //            List<FTPAttachment> MSMEFTPAttachments = GetMSMEFTPAttachment(bPVendorOnBoarding.TransID, bPVendorOnBoarding.MSME_Att_ID);
            //            allAttachments.AddRange(MSMEFTPAttachments);
            //        }
            //        //if (!string.IsNullOrEmpty(bPVendorOnBoarding.RP_Att_ID))
            //        //{
            //        //    List<FTPAttachment> RPFTPAttachments = GetRPFTPAttachment(bPVendorOnBoarding.TransID, bPVendorOnBoarding.RP_Att_ID);
            //        //    allAttachments.AddRange(RPFTPAttachments);
            //        //}
            //        //if (!string.IsNullOrEmpty(bPVendorOnBoarding.TDS_Att_ID))
            //        //{
            //        //    List<FTPAttachment> TDSFTPAttachments = GetTDSFTPAttachment(bPVendorOnBoarding.TransID, bPVendorOnBoarding.TDS_Att_ID);
            //        //    allAttachments.AddRange(TDSFTPAttachments);
            //        //}
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }

            //    return allAttachments;
            //}

            //public List<FTPAttachment> GetIdentityFTPAttachment(int TransID)
            //{
            //    try
            //    {
            //        var Identitys = (from tb in _dbContext.BPAttachments
            //                         join tb1 in _dbContext.BPIdentities
            //                         on tb.AttachmentName equals tb1.AttachmentName
            //                         where tb.AppNumber == tb1.Type
            //                         && tb1.TransID.ToString() == tb.HeaderNumber
            //                         && tb1.TransID == TransID
            //                         select new FTPAttachment()
            //                         {
            //                             AttachmentID = tb.AttachmentID,
            //                             TransID = TransID.ToString(),
            //                             AttachmentFile = tb.AttachmentFile,
            //                             AttachmentName = tb.AttachmentName,
            //                             Type = tb1.Type
            //                         }).ToList();
            //        return Identitys;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            //public List<FTPAttachment> GetBankFTPAttachment(int TransID)
            //{
            //    try
            //    {
            //        var Banks = (from tb in _dbContext.BPAttachments
            //                     join tb1 in _dbContext.BPBanks
            //                     on tb.AttachmentName equals tb1.AttachmentName
            //                     where tb.AppNumber == tb1.AccountNo
            //                     && tb1.TransID.ToString() == tb.HeaderNumber
            //                     && tb1.TransID == TransID
            //                     select new FTPAttachment()
            //                     {
            //                         AttachmentID = tb.AttachmentID,
            //                         TransID = TransID.ToString(),
            //                         AttachmentFile = tb.AttachmentFile,
            //                         AttachmentName = tb.AttachmentName,
            //                         Type = "Bank"
            //                     }).ToList();
            //        int i = 1;
            //        foreach (var bank in Banks)
            //        {
            //            bank.Type = bank.Type + i;
            //            i++;
            //        }
            //        return Banks;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            //public List<FTPAttachment> GetMSMEFTPAttachment(int TransID, string MSME_Att_ID)
            //{
            //    try
            //    {
            //        var MSME = (from tb in _dbContext.BPAttachments
            //                    where tb.AttachmentID.ToString() == MSME_Att_ID
            //                    select new FTPAttachment()
            //                    {
            //                        AttachmentID = tb.AttachmentID,
            //                        TransID = TransID.ToString(),
            //                        AttachmentFile = tb.AttachmentFile,
            //                        AttachmentName = tb.AttachmentName,
            //                        Type = "MSME"
            //                    }).ToList();

            //        return MSME;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            //public List<FTPAttachment> GetRPFTPAttachment(int TransID, string RP_Att_ID)
            //{
            //    try
            //    {
            //        var MSME = (from tb in _dbContext.BPAttachments
            //                    where tb.AttachmentID.ToString() == RP_Att_ID
            //                    select new FTPAttachment()
            //                    {
            //                        AttachmentID = tb.AttachmentID,
            //                        TransID = TransID.ToString(),
            //                        AttachmentFile = tb.AttachmentFile,
            //                        AttachmentName = tb.AttachmentName,
            //                        Type = "RP"
            //                    }).ToList();

            //        return MSME;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            //public List<FTPAttachment> GetTDSFTPAttachment(int TransID, string TDS_Att_ID)
            //{
            //    try
            //    {
            //        var MSME = (from tb in _dbContext.BPAttachments
            //                    where tb.AttachmentID.ToString() == TDS_Att_ID
            //                    select new FTPAttachment()
            //                    {
            //                        AttachmentID = tb.AttachmentID,
            //                        TransID = TransID.ToString(),
            //                        AttachmentFile = tb.AttachmentFile,
            //                        AttachmentName = tb.AttachmentName,
            //                        Type = "TDS"
            //                    }).ToList();

            //        return MSME;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            //public List<BPAttachment1> GetAttachmentforXML_MSME(int MSME_ID)
            //{
            //    try
            //    {
            //        var MSME = (from tb in _dbContext.BPAttachments
            //                    where tb.AttachmentID == MSME_ID
            //                    select new BPAttachment1()
            //                    {
            //                        AttachmentFile = tb.AttachmentFile,
            //                        AttachmentName = tb.AttachmentName
            //                    }).ToList();

            //        return MSME;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            //public List<BPAttachment1> GetAttachmentforXML_RP_ID(int RP_ID)
            //{
            //    try
            //    {
            //        var MSME = (from tb in _dbContext.BPAttachments
            //                    where tb.AttachmentID == RP_ID
            //                    select new BPAttachment1()
            //                    {
            //                        AttachmentFile = tb.AttachmentFile,
            //                        AttachmentName = tb.AttachmentName
            //                    }).ToList();

            //        return MSME;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            //public List<BPAttachment1> GetAttachmentforXML_TDS(int TDS)
            //{
            //    try
            //    {
            //        var MSME = (from tb in _dbContext.BPAttachments
            //                    where tb.AttachmentID == TDS
            //                    select new BPAttachment1()
            //                    {
            //                        AttachmentFile = tb.AttachmentFile,
            //                        AttachmentName = tb.AttachmentName
            //                    }).ToList();

            //        return MSME;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            
        
        public List<BPBank> bank(int TransID)
            {
                try
                {
                    var Bank = (from tb in _dbContext.BPBanks
                                where tb.TransID == TransID
                                select new BPBank()
                                {
                                    AccountNo = tb.AccountNo,
                                    //AttachmentName = tb.AttachmentName
                                }).ToList();

                    return Bank;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //public List<BPAttachment1> bank_doc(int TransID, string Accono)
            //{
            //    try
            //    {
            //        var Bank = (from tb in _dbContext.BPAttachments
            //                    where tb.HeaderNumber == TransID.ToString() && tb.AppNumber == Accono.ToString()
            //                    select new BPAttachment1()
            //                    {
            //                        AttachmentFile = tb.AttachmentFile,
            //                        //AttachmentName = tb.AttachmentName

            //                    }).ToList();

            //        return Bank;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
           
        public BPVendorOnBoarding GetVendorOnBoardingsByEmailID(string EmailID)
            {
                try
                {
                    return _dbContext.BPVendorOnBoardings.Where(x => x.Primarymail.ToLower() == EmailID.ToLower()).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public List<BPVendorOnBoarding> GetRegisteredVendorOnBoardings()
            {
                try
                {
                    return _dbContext.BPVendorOnBoardings.Where(x => string.IsNullOrEmpty(x.Status)).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        public async Task<BPVendorOnBoarding> InitializeVendorRegistration(VendorInitialzationClass vendorInitialzationClass)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var userdetails = _dbContext.Users.Where(x => x.Email == vendorInitialzationClass.CreatedMailID && x.UserName == vendorInitialzationClass.CreatedUsername).FirstOrDefault();
                if (userdetails != null)
                {
                    try
                    {
                        ///  WriteLog.WriteToFile("VendorOnBoardingRepository/CreateVendorInitilizationToken:- Token already present, updating new token to the user whose mail id is " );

                        //BPVendorOnBoarding history1 = (from tb in _dbContext.BPVendorOnBoardings
                        //                               where tb.Email1 == vendorInitialzationClass.Email
                        //                               select tb).FirstOrDefault();
                        //if (history1 == null)
                        //{

                        BPVendorOnBoarding VendorOnBoarding = new BPVendorOnBoarding();
                        VendorOnBoarding.Name = vendorInitialzationClass.Name;
                        VendorOnBoarding.Primarymail = vendorInitialzationClass.Email;
                        VendorOnBoarding.AccountGroup = vendorInitialzationClass.AccountGroup;
                        VendorOnBoarding.PurchaseOrg = vendorInitialzationClass.PurchaseOrg;
                        VendorOnBoarding.CompanyCode = vendorInitialzationClass.CompanyCode;
                        VendorOnBoarding.GSTValue = vendorInitialzationClass.GSTValue;
                        VendorOnBoarding.Plant = vendorInitialzationClass.Plant;
                        //VendorOnBoarding.G
                        VendorOnBoarding.Status = "Initialized";
                        VendorOnBoarding.IsActive = true;
                        VendorOnBoarding.CreatedOn = DateTime.Now;
                        VendorOnBoarding.CreatedBy = vendorInitialzationClass.CreatedMailID;
                        VendorOnBoarding.IsDocRequired = vendorInitialzationClass.IsDocRequired;
                        VendorOnBoarding.TypeOfService = vendorInitialzationClass.TypeOfService;
                        VendorOnBoarding.BankDetailsRequired = vendorInitialzationClass.BankDetailsRequired;
                        var result = _dbContext.BPVendorOnBoardings.Add(VendorOnBoarding);
                        //string username = 00000 + int.Parse(VendorOnBoarding.TransID);

                        await _dbContext.SaveChangesAsync();

                        bool result1 = false;
                        var createvendor = await CreateVendorLogin(vendorInitialzationClass.Name, vendorInitialzationClass.Email, VendorOnBoarding.TransID);


                        if (createvendor)
                        {
                            result1 = await CreateVendorInitilizationToken(result.Entity, VendorOnBoarding.TransID.ToString(), userdetails.Email);
                        }

                        ApproverUser approverUser = new ApproverUser();
                        approverUser.UserName = vendorInitialzationClass.Name;
                        approverUser.Email = vendorInitialzationClass.Email;
                        bool result2 = await CreateApproverUser(approverUser);
                        if (result1)
                        {
                            transaction.Commit();
                            transaction.Dispose();
                            return VendorOnBoarding;
                        }
                        transaction.Rollback();
                        transaction.Dispose();

                        throw new Exception("Unable to generate token");
                        //}
                        //else
                        //{
                        //    throw new Exception("Vendor with same email address has existed");
                        //}
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                throw new Exception("Initiator Mail not registered");

            }
            }
       

            private async Task<bool> CreateVendorInitilizationToken(BPVendorOnBoarding VendorOnBoarding,string vendorname,string initiatormail)
            {
                try
                {
                string attachmenttype= "CANCEL CHEQUE,GSTIN,PAN,MSME Certificate,Others";
                    DateTime ExpireDateTime = DateTime.Now.AddDays(_tokenTimespan);
                    string code = Encrypt(VendorOnBoarding.TransID.ToString() + '|' + "2001"+ '|' + ExpireDateTime, true);
                    string PortalAddress = _configuration["PortalAddress"];
                    bool sendresult = await SendMail(HttpUtility.UrlEncode(code), VendorOnBoarding.Name, VendorOnBoarding.Primarymail, VendorOnBoarding.TransID.ToString(), PortalAddress, vendorname, initiatormail);
                    WriteLog.WriteToFile(" sendmailstatus" + sendresult);

                    if (sendresult)
                    {
                    try
                    {
                        TokenHistory history1 = (from tb in _dbContext.RegisterTokenHistories
                                                 where tb.TransID == VendorOnBoarding.TransID && !tb.IsUsed
                                                 select tb).FirstOrDefault();
                        if (history1 == null)
                        {
                            TokenHistory history = new TokenHistory()
                            {
                                TransID = VendorOnBoarding.TransID,
                                Token = code,
                                EmailAddress = VendorOnBoarding.Primarymail,
                                CreatedOn = DateTime.Now,
                                ExpireOn = ExpireDateTime,
                                IsUsed = false,
                                Comment = "Registration link has been sent successfully"
                            };
                            var result = _dbContext.RegisterTokenHistories.Add(history);
                        }
                        else
                        {
                            WriteLog.WriteToFile("VendorOnBoardingRepository/CreateVendorInitilizationToken:- Token already present, updating new token to the user whose mail id is " + VendorOnBoarding.Primarymail);
                            history1.Token = code;
                            history1.CreatedOn = DateTime.Now;
                            history1.ExpireOn = ExpireDateTime;
                        }
                        await _dbContext.SaveChangesAsync();
                        string[] splittedtype = attachmenttype.Split(",");
                        var resultattachment = _dbContext.IdentityAttachments.Where(x => x.TransID == VendorOnBoarding.TransID);
                        if (resultattachment != null)
                        {
                            foreach (var attachment in splittedtype)
                            {
                                IdentityAttachments IdentityAttachments = new IdentityAttachments();
                                IdentityAttachments.TransID = VendorOnBoarding.TransID;
                                IdentityAttachments.DocType = attachment;
                                 _dbContext.IdentityAttachments.Add(IdentityAttachments);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        WriteLog.WriteToFile("Master/SendLinkToMail/Exception:- Add record to TokenHistories - " + ex.Message, ex);
                    }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        
            public async Task<bool> CreateApproverUser(ApproverUser approverUser)
            {
                try
                {
                    string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                    string HostURI = BaseAddress + "/authenticationapi/Master/CreateApproverUser";
                    var uri = new Uri(HostURI);
                    var request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    var SerializedObject = JsonConvert.SerializeObject(approverUser);
                    byte[] requestBody = Encoding.UTF8.GetBytes(SerializedObject);

                    using (var postStream = await request.GetRequestStreamAsync())
                    {
                        await postStream.WriteAsync(requestBody, 0, requestBody.Length);
                    }

                    try
                    {
                        using (var response = (HttpWebResponse)await request.GetResponseAsync())
                        {
                            if (response != null && response.StatusCode == HttpStatusCode.OK)
                            {
                                var reader = new StreamReader(response.GetResponseStream());
                                string responseString = await reader.ReadToEndAsync();
                                reader.Close();
                                return true;
                            }
                            else
                            {
                                var reader = new StreamReader(response.GetResponseStream());
                                string responseString = await reader.ReadToEndAsync();
                                reader.Close();
                                return false;
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        using (var stream = ex.Response.GetResponseStream())
                        using (var reader = new StreamReader(stream))
                        {
                            var errorMessage = reader.ReadToEnd();
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                //throw new Exception(errorMessage);
                                WriteLog.WriteToFile($"FactRepository/CreateVendorUser:- Error {errorMessage} for {approverUser.UserName}");
                                return false;
                            }
                            //throw ex;
                            WriteLog.WriteToFile($"FactRepository/CreateVendorUser:- Error {ex.Message} for {approverUser.UserName}");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                        WriteLog.WriteToFile($"FactRepository/CreateVendorUser:- Error {ex.Message} for {approverUser.UserName}");
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    //throw ex; 
                    WriteLog.WriteToFile($"FactRepository/CreateVendorUser:- Error {ex.Message} for {approverUser.UserName}");
                    return false;
                }
            }


            public bool ChectTokenValidity(VendorTokenCheck tokenCheck)
            {
                string[] decryptedArray = new string[3];
                string result = string.Empty;
                try
                {
                    try
                    {
                        result = Decrypt(tokenCheck.Token, true);
                    }
                    catch
                    {
                        throw new Exception("Invalid token!");
                    }
                    if (!string.IsNullOrEmpty(result) && result.Contains('|') && result.Split('|').Length == 3)
                    {
                        decryptedArray = result.Split('|');
                    }
                    else
                    {
                        throw new Exception("Invalid token!");
                    }

                    if (decryptedArray.Length == 3)
                    {
                        DateTime date = DateTime.Parse(decryptedArray[2].Replace('+', ' '));
                        if (DateTime.Now > date)// Convert.ToDateTime(decryptedarray[2]))
                        {
                            throw new Exception("token expired!");
                        }
                        var DecryptedUserID = decryptedArray[0];

                        var user = (from tb in _dbContext.BPVendorOnBoardings
                                    where tb.TransID.ToString() == DecryptedUserID && tb.IsActive
                                    select tb).FirstOrDefault();

                        if (tokenCheck.TransID == user.TransID)
                        {
                            try
                            {
                                TokenHistory history = _dbContext.RegisterTokenHistories.Where(x => x.TransID == user.TransID && !x.IsUsed && x.Token == tokenCheck.Token).Select(r => r).FirstOrDefault();
                                if (history != null)
                                {
                                    return true;
                                }
                                else
                                {
                                    throw new Exception("Token might have already used or wrong token");

                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLog.WriteToFile("Master/ChectTokenValidity/Exception:- Getting TokenHistory - " + ex.Message, ex);
                                throw new Exception("Token might have already used or wrong token");
                            }

                        }
                        else
                        {
                            throw new Exception("Invalid token!");
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid token!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public async Task<BPVendorOnBoarding> CreateVendorOnBoarding(BPVendorOnBoardingView VendorOnBoardingView)
            {

                string[] decryptedArray = new string[3];
                string result2 = string.Empty;
                try
                {
                    try
                    {
                        result2 = Decrypt(VendorOnBoardingView.token, true);
                    }
                    catch
                    {
                        throw new Exception("Invalid token!");
                    }
                    if (result2.Contains('|') && result2.Split('|').Length == 3)
                    {
                        decryptedArray = result2.Split('|');
                    }
                    else
                    {
                        throw new Exception("Invalid token!");
                    }

                    if (decryptedArray.Length == 3)
                    {
                        DateTime date = DateTime.Parse(decryptedArray[2].Replace('+', ' '));
                        if (DateTime.Now > date)// Convert.ToDateTime(decryptedarray[2]))
                        {
                            throw new Exception("token expired!");
                        }
                        var DecryptedUserID = decryptedArray[0];

                        var user = (from tb in _dbContext.BPVendorOnBoardings
                                    where tb.TransID.ToString() == DecryptedUserID && tb.IsActive
                                    select tb).FirstOrDefault();

                        if (VendorOnBoardingView.transID == user.TransID)
                        {
                            try
                            {
                                TokenHistory history = _dbContext.RegisterTokenHistories.Where(x => x.TransID == user.TransID && !x.IsUsed && x.Token == VendorOnBoardingView.token).Select(r => r).FirstOrDefault();
                                if (history != null)
                                {
                                    //BPVendorOnBoarding vendorOnBoarding = new BPVendorOnBoarding();
                                    //var strategy = _dbContext.Database.CreateExecutionStrategy();
                                    //await strategy.ExecuteAsync(async () =>
                                    //{
                                    using (var transaction = _dbContext.Database.BeginTransaction())
                                    {
                                        try
                                        {
                                            var bPVendorOnBoarding = _dbContext.BPVendorOnBoardings.Where(x => x.Primarymail == VendorOnBoardingView.primarymail && x.IsActive).FirstOrDefault();
                                            if (bPVendorOnBoarding == null)
                                            {
                                                BPVendorOnBoarding VendorOnBoarding = new BPVendorOnBoarding();
                                                VendorOnBoarding.Name = VendorOnBoardingView.name;
                                                VendorOnBoarding.StateCode = VendorOnBoardingView.stateCode;
                                                VendorOnBoarding.AddressLine1 = VendorOnBoardingView.AddressLine1;
                                                VendorOnBoarding.AddressLine2 = VendorOnBoardingView.AddressLine2;
                                                VendorOnBoarding.AddressLine3 = VendorOnBoardingView.AddressLine3;
                                                VendorOnBoarding.AddressLine4 = VendorOnBoardingView.AddressLine4;
                                            VendorOnBoarding.AddressLine5 = VendorOnBoardingView.AddressLine5;
                                            VendorOnBoarding.City = VendorOnBoardingView.city;
                                                //VendorOnBoarding.State = VendorOnBoardingView.state;
                                                VendorOnBoarding.Country = VendorOnBoardingView.country;
                                                VendorOnBoarding.PinCode = VendorOnBoardingView.pinCode;
                                                VendorOnBoarding.GSTNumber = VendorOnBoardingView.gstNumber;
                                                VendorOnBoarding.GSTStatus = VendorOnBoardingView.gstStatus;
                                                VendorOnBoarding.PANNumber = VendorOnBoardingView.panNumber;
                                                VendorOnBoarding.PrimaryContact = VendorOnBoardingView.primaryContact;
                                                VendorOnBoarding.SecondaryContact = VendorOnBoardingView.secondaryContact;
                                                VendorOnBoarding.Primarymail = VendorOnBoardingView.primarymail;
                                                VendorOnBoarding.Secondarymail = VendorOnBoardingView.secondarymail;
                                                VendorOnBoarding.Status = VendorOnBoardingView.status;
                                                VendorOnBoarding.MSME_TYPE = VendorOnBoardingView.msmE_TYPE;
                                                VendorOnBoarding.MSME_Att_ID = VendorOnBoardingView.msmE_Att_ID;
                                                VendorOnBoarding.IsActive = true;
                                                VendorOnBoarding.CreatedOn = DateTime.Now;
                                                VendorOnBoarding.CertificateNo = VendorOnBoardingView.certificateNo;
                                                //VendorOnBoarding.CertificateStatus = VendorOnBoardingView.certificateStatus;
                                                VendorOnBoarding.RegisteredCity = VendorOnBoardingView.registeredCity;
                                                VendorOnBoarding.ValidFrom = VendorOnBoardingView.validFrom;
                                                VendorOnBoarding.ValidFrom = VendorOnBoardingView.validTo;
                                                var result = _dbContext.BPVendorOnBoardings.Add(VendorOnBoarding);
                                                await _dbContext.SaveChangesAsync();

                                                //IdentityRepository identityRepository = new IdentityRepository(_dbContext);
                                                //await identityRepository.CreateIdentities(VendorOnBoardingView.bPIdentities, result.Entity.TransID);

                                                //BankRepository BankRepository = new BankRepository(_dbContext);
                                                //await BankRepository.CreateBanks(VendorOnBoardingView.bPBanks, result.Entity.TransID);

                                                //ContactRepository ContactRepository = new ContactRepository(_dbContext);
                                                //await ContactRepository.CreateContacts(VendorOnBoardingView.bPContacts, result.Entity.TransID);

                                                //ActivityLogRepository ActivityLogRepository = new ActivityLogRepository(_dbContext);
                                                //await ActivityLogRepository.CreateActivityLogs(VendorOnBoardingView.bPActivityLogs, result.Entity.TransID);

                                                //VendorOnBoardingView.QuestionAnswers.ForEach(x => { x.AppUID = result.Entity.TransID; });
                                                //QuestionnaireRepository questionnaireRepository = new QuestionnaireRepository(_dbContext, _configuration);
                                                //await questionnaireRepository.SaveAnswers(VendorOnBoardingView.QuestionAnswers);

                                                // Updating TokenHistory
                                                history.UsedOn = DateTime.Now;
                                                history.IsUsed = true;
                                                history.Comment = "Token Used successfully";
                                                await _dbContext.SaveChangesAsync();

                                                transaction.Commit();
                                                transaction.Dispose();
                                                return result.Entity;
                                            }
                                            else
                                            {
                                                throw new Exception("Vendor with same email address has already exist.");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            transaction.Rollback();
                                            transaction.Dispose();
                                            throw ex;
                                        }

                                    }
                                    //});
                                    //return vendorOnBoarding;
                                }
                                else
                                {
                                    throw new Exception("Token might have already used or wrong token");
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLog.WriteToFile("Master/ChectTokenValidity/Exception:- Getting TokenHistory - " + ex.Message, ex);
                                throw new Exception("Token might have already used or wrong token");
                            }

                        }
                        else
                        {
                            throw new Exception("Invalid token!");
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid token!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }


            //public async Task<BPVendorOnBoarding> UpdateVendorOnBoarding(BPVendorOnBoarding VendorOnBoarding)
            //{
            //    try
            //    {
            //        var entity = _dbContext.Set<BPVendorOnBoarding>().FirstOrDefault(x => x.TransID == VendorOnBoarding.TransID);
            //        if (entity == null)
            //        {
            //            return entity;
            //        }
            //        //_dbContext.Entry(VendorOnBoarding).State = EntityState.Modified;
            //        entity.Name = VendorOnBoarding.Name;
            //        entity.Role = VendorOnBoarding.Role;
            //        entity.LegalName = VendorOnBoarding.LegalName;
            //        entity.AddressLine1 = VendorOnBoarding.AddressLine1;
            //        entity.AddressLine2 = VendorOnBoarding.AddressLine2;
            //        entity.City = VendorOnBoarding.City;
            //        entity.State = VendorOnBoarding.State;
            //        entity.Country = VendorOnBoarding.Country;
            //        entity.PinCode = VendorOnBoarding.PinCode;
            //        entity.Type = VendorOnBoarding.Type;
            //        entity.Phone1 = VendorOnBoarding.Phone1;
            //        entity.Phone2 = VendorOnBoarding.Phone2;
            //        entity.Email1 = VendorOnBoarding.Email1;
            //        entity.Email2 = VendorOnBoarding.Email2;
            //        entity.VendorCode = VendorOnBoarding.VendorCode;
            //        entity.ParentVendor = VendorOnBoarding.ParentVendor;
            //        entity.Status = VendorOnBoarding.Status;
            //        entity.ModifiedBy = VendorOnBoarding.ModifiedBy;
            //        entity.ModifiedOn = DateTime.Now;
            //        await _dbContext.SaveChangesAsync();
            //        return VendorOnBoarding;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            public async Task<bool> UpdateVendorOnBoarding(BPVendorOnBoardingView VendorOnBoardingView)
            {

                string[] decryptedArray = new string[3];
            bool status = false;
                string result2 = string.Empty;
                try
                {
                    try
                    {
                        result2 = Decrypt(VendorOnBoardingView.token, true);
                    }
                    catch
                    {
                        throw new Exception("Invalid token!");
                    }
                    if (result2.Contains('|') && result2.Split('|').Length == 3)
                    {
                        decryptedArray = result2.Split('|');
                    }
                    else
                    {
                        throw new Exception("Invalid token!");
                    }

                    if (decryptedArray.Length == 3)
                    {
                        DateTime date = DateTime.Parse(decryptedArray[2].Replace('+', ' '));
                        if (DateTime.Now > date)// Convert.ToDateTime(decryptedarray[2]))
                        {
                            throw new Exception("token expired!");
                        }
                        var DecryptedUserID = decryptedArray[0];

                        var user = (from tb in _dbContext.BPVendorOnBoardings
                                    where tb.TransID.ToString() == DecryptedUserID && tb.IsActive
                                    select tb).FirstOrDefault();

                        if (VendorOnBoardingView.transID == user.TransID)
                        {
                            try
                            {
                                TokenHistory history = _dbContext.RegisterTokenHistories.Where(x => x.TransID == user.TransID && !x.IsUsed && x.Token == VendorOnBoardingView.token).Select(r => r).FirstOrDefault();
                                if (history != null)
                                {
                                    //BPVendorOnBoarding vendorOnBoarding = new BPVendorOnBoarding();
                                    //var strategy = _dbContext.Database.CreateExecutionStrategy();
                                    //await strategy.ExecuteAsync(async () =>
                                    //{
                                    using (var transaction = _dbContext.Database.BeginTransaction())
                                    {
                                        try
                                        {
                                            var VendorOnBoarding = _dbContext.Set<BPVendorOnBoarding>().FirstOrDefault(x => x.TransID == VendorOnBoardingView.transID);
                                            if (VendorOnBoarding == null)
                                            {
                                            status =  false;
                                            }
                                            VendorOnBoarding.Name = VendorOnBoardingView.name;
                                            VendorOnBoarding.StateCode = VendorOnBoardingView.stateCode;
                                            VendorOnBoarding.AddressLine1 = VendorOnBoardingView.AddressLine1;
                                            VendorOnBoarding.AddressLine2 = VendorOnBoardingView.AddressLine2;
                                            VendorOnBoarding.AddressLine3 = VendorOnBoardingView.AddressLine3;
                                            VendorOnBoarding.AddressLine4 = VendorOnBoardingView.AddressLine4;
                                        VendorOnBoarding.AddressLine5 = VendorOnBoardingView.AddressLine5;
                                        VendorOnBoarding.City = VendorOnBoardingView.city;
                                            //VendorOnBoarding.State = VendorOnBoardingView.state;
                                            VendorOnBoarding.Country = VendorOnBoardingView.country;
                                            VendorOnBoarding.PinCode = VendorOnBoardingView.pinCode;
                                            VendorOnBoarding.GSTNumber = VendorOnBoardingView.gstNumber;
                                            VendorOnBoarding.GSTStatus = VendorOnBoardingView.gstStatus;
                                            VendorOnBoarding.PANNumber = VendorOnBoardingView.panNumber;
                                            VendorOnBoarding.TypeOfService = VendorOnBoardingView.TypeOfService;
                                            VendorOnBoarding.PrimaryContact = VendorOnBoardingView.primaryContact;
                                            VendorOnBoarding.SecondaryContact = VendorOnBoardingView.secondaryContact;
                                            VendorOnBoarding.Primarymail = VendorOnBoardingView.primarymail;
                                            VendorOnBoarding.Secondarymail = VendorOnBoardingView.secondarymail;
                                            VendorOnBoarding.Status = VendorOnBoardingView.status;
                                            VendorOnBoarding.Department = VendorOnBoardingView.department;
                                            VendorOnBoarding.AccountGroup = VendorOnBoardingView.accountGroup;
                                        VendorOnBoarding.Plant = VendorOnBoardingView.Plant;
                                        VendorOnBoarding.PurchaseOrg = VendorOnBoardingView.purchaseOrg;
                                            VendorOnBoarding.CompanyCode = VendorOnBoardingView.companyCode;
                                            VendorOnBoarding.MSME_TYPE = VendorOnBoardingView.msmE_TYPE;
                                            VendorOnBoarding.MSME_Att_ID = VendorOnBoardingView.msmE_Att_ID;
                                            VendorOnBoarding.IsActive = true;
                                            VendorOnBoarding.ModifiedBy = VendorOnBoardingView.ModifiedBy;
                                            VendorOnBoarding.ModifiedOn = DateTime.Now;
                                        VendorOnBoarding.GSTValue = VendorOnBoardingView.GSTValue;
                                            VendorOnBoarding.CertificateNo = VendorOnBoardingView.certificateNo;
                                            //VendorOnBoarding.CertificateStatus = VendorOnBoardingView.certificateStatus;
                                            VendorOnBoarding.RegisteredCity = VendorOnBoardingView.registeredCity;
                                            VendorOnBoarding.ValidFrom = VendorOnBoardingView.validFrom;
                                            VendorOnBoarding.ValidTo= VendorOnBoardingView.validTo;
                                            await _dbContext.SaveChangesAsync();

                                            //IdentityRepository identityRepository = new IdentityRepository(_dbContext);
                                            //await identityRepository.DeleteIdentityByTransID(VendorOnBoardingView.transID);
                                            //await identityRepository.CreateIdentities(VendorOnBoardingView.bPIdentities, VendorOnBoardingView.transID);

                                            //BankRepository BankRepository = new BankRepository(_dbContext);
                                            //await BankRepository.DeleteBankByTransID(VendorOnBoardingView.transID);
                                            //await BankRepository.CreateBanks(VendorOnBoardingView.bPBanks, VendorOnBoardingView.transID);

                                            //ContactRepository ContactRepository = new ContactRepository(_dbContext);
                                            //await ContactRepository.DeleteContactByTransID(VendorOnBoardingView.transID);
                                            //await ContactRepository.CreateContacts(VendorOnBoardingView.bPContacts, VendorOnBoardingView.transID); 
                                            if (VendorOnBoarding.Status.ToLower() == "registered")
                                            {
                                                // Updating TokenHistory
                                                history.UsedOn = DateTime.Now;
                                                history.IsUsed = true;
                                                history.Comment = "Token Used successfully";
                                                await _dbContext.SaveChangesAsync();
                                          
                                                var result = await CreateVendorUser(VendorOnBoardingView, VendorOnBoarding.CreatedBy);
                                                if (result)
                                                {
                                                    transaction.Commit();
                                                    transaction.Dispose();
                                                status = true;
                                                
                                                }
                                                else
                                                {
                                                    transaction.Rollback();
                                                    transaction.Dispose();
                                                status =  false;
                                                }
                                            }
                                        return status;
                                            
                                        }
                                        catch (Exception ex)
                                        {
                                            transaction.Rollback();
                                            transaction.Dispose();
                                            throw ex;
                                        }
                                    }
                                    //});
                                    //return vendorOnBoarding;
                                }
                                else
                                {
                                    throw new Exception("Token might have already used or wrong token");
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLog.WriteToFile("Master/ChectTokenValidity/Exception:- Getting TokenHistory - " + ex.Message, ex);
                                throw new Exception("Token might have already used or wrong token");
                            }

                        }
                        else
                        {
                            throw new Exception("Invalid token!");
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid token!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

        }
        public async Task<bool> CreateVendorLogin(string name,string vendormail, int TransID)
        {
            try
            {
                WriteLog.WriteToFile("VendorOnBoardingRepository/VendorCreate ");
                Random random = new Random();
                const int letterMinAscii = 65;
                const int letterMaxAscii = 91;
                char[] specialCharacters = { '!', '@', '#', '$', '%', '^', '&', '*' };
                string randomString = "";
                for (int i = 0; i < 7; i++)
                {
                    bool isLetter = random.Next(2) == 0;

                    char randomChar;

                    if (isLetter)
                    {
                        int randomAscii = random.Next(letterMinAscii, letterMaxAscii);
                        randomChar = (char)randomAscii;
                    }
                    else
                    {
                        randomChar = specialCharacters[random.Next(specialCharacters.Length)];
                    }

                    randomString += randomChar;
                }
                VendorRegisterLogin vendorRegisterLogin = new VendorRegisterLogin();
                int transID = TransID;
                string formattedTransID = transID.ToString("D5");
                if (name.Length > 4)
                {
                    vendorRegisterLogin.UserId = name.Substring(0, 4) + formattedTransID;
                }
                else
                {
                    vendorRegisterLogin.UserId = name + formattedTransID;
                }
                //vendorRegisterLogin.UserId = TransID.ToString(); ;
                vendorRegisterLogin.VendorMail = vendormail;
                vendorRegisterLogin.Password1 = Encryptpassword(randomString, true);
                vendorRegisterLogin.Password2 = Encryptpassword("Zenex@123", true);
                vendorRegisterLogin.CreatedOn = DateTime.Now;
                vendorRegisterLogin.IsActive = true;
                var savevendor = _dbContext.VendorRegisterLogin.Add(vendorRegisterLogin);
                await _dbContext.SaveChangesAsync();
                return true;

            }
          catch(Exception ex)
            {
                WriteLog.WriteToFile("VendorOnBoardingRepository/VendorCreate " + ex.Message);
                return false;

            }

           
        }
        public async Task<bool> CreateVendorUser(BPVendorOnBoardingView VendorOnBoardingView,string CreatedBy)
        {
            try
            {
                WriteLog.WriteToFile("WriteToFile", CreatedBy);
                string xmlContent = "";
                bool sendreq = false;
                string filecontent = "";
                bool attachmentpost = false;
                string validfrom = "";
                string validTo = "";


                string[] regioncode = VendorOnBoardingView.stateCode.Split(" ");

                var cancelcheque = _dbContext.IdentityAttachments.Where(x => x.TransID == VendorOnBoardingView.transID && x.DocType== "CANCEL CHEQUE" && x.AttachmentName != null).FirstOrDefault();
                var pan = _dbContext.IdentityAttachments.Where(x => x.TransID == VendorOnBoardingView.transID && x.DocType == "PAN" && x.AttachmentName != null).FirstOrDefault();
                var other = _dbContext.IdentityAttachments.Where(x => x.TransID == VendorOnBoardingView.transID && x.DocType == "OTHERS" && x.AttachmentName != null).FirstOrDefault();
                var gst = _dbContext.IdentityAttachments.Where(x => x.TransID == VendorOnBoardingView.transID && x.DocType == "GSTIN" && x.AttachmentName != null).FirstOrDefault();
                var msme = _dbContext.IdentityAttachments.Where(x => x.TransID == VendorOnBoardingView.transID && x.DocType == "MSME Certificate" && x.AttachmentName != null).FirstOrDefault();
                if(VendorOnBoardingView.validFrom != null && VendorOnBoardingView.validTo != null)
                {
                     validfrom = VendorOnBoardingView.validFrom?.ToString("dd-MM-yyyy");
                     validTo = VendorOnBoardingView.validTo?.ToString("dd-MM-yyyy");
                }
                
                XDocument addressdoc = new XDocument(
                    new XElement("ADDRESS",
                    new XElement("item",
                       new XElement("COUNTRY", VendorOnBoardingView.country.Substring(0,2)),
                        new XElement("PINCODE", VendorOnBoardingView.pinCode),
                        new XElement("CITY", VendorOnBoardingView.city),
                        //new XElement("STATE", VendorOnBoardingView.state),
                        new XElement("REGION", regioncode[0]),
                        new XElement("ADDRESSLINE1", VendorOnBoardingView.AddressLine1),
                        new XElement("ADDRESSLINE2", VendorOnBoardingView.AddressLine2),
                        new XElement("ADDRESSLINE3", VendorOnBoardingView.AddressLine3),
                        new XElement("ADDRESSLINE4", VendorOnBoardingView.AddressLine4),
                        new XElement("ADDRESSLINE5", VendorOnBoardingView.AddressLine5),
                        new XElement("MSME_TYPE", VendorOnBoardingView.msmE_TYPE),
                        //new XElement("TYPE_OF_INDUSTRY", VendorOnBoardingView.typeofIndustry),
                        new XElement("CERTIFICATE_NUMBER", VendorOnBoardingView.certificateNo),
                        new XElement("REGISTERED_CITY", VendorOnBoardingView.registeredCity),
                        new XElement("VALID_FROM", validfrom ),
                        new XElement("VALID_TO", validTo)
                        //new XElement("STATUS", "Register")
                    )
                    )
                 );
                WriteLog.WriteToFile("addressdoc", addressdoc.ToString());
                xmlContent = addressdoc.ToString();
                XDocument BANKdoc = new XDocument(new XElement("BANK"));
                for (int i = 0; i < VendorOnBoardingView.bPBanks.Count; i++)
                {
                    XElement itemElementbank = new XElement("item",
                        new XElement("IFSC_NUMBER", VendorOnBoardingView.bPBanks[i].IFSC),
                        new XElement("ACCOUNT_NUMBER", VendorOnBoardingView.bPBanks[i].AccountNo),
                        new XElement("BRANCH", VendorOnBoardingView.bPBanks[i].Branch),
                        new XElement("BANK_NAME", VendorOnBoardingView.bPBanks[i].BankName),
                        new XElement("CITY", VendorOnBoardingView.bPBanks[i].City)
                
                    );
                    BANKdoc.Root.Add(itemElementbank);
                }
                xmlContent += "\r\n" + BANKdoc.ToString();
                //for (int i = 0; i < attachment.Count; i++)
                //{
                   if (cancelcheque != null)
                    {

                        string base64String = Convert.ToBase64String(cancelcheque.AttachmentFile);
                        filecontent += "<CANCLECHEQUE>" + base64String + "\r\n</CANCLECHEQUE>\r\n<CANCLECHEQUENAME>" + cancelcheque.AttachmentName + "</CANCLECHEQUENAME>";

                    }
                    xmlContent += "\r\n"+filecontent;
                    filecontent = "";
                //}

                    XDocument CONTDETAILdoc = new XDocument(new XElement("CONTDETAIL"));
                
                    XElement itemElement = new XElement("item",
                        new XElement("NAME", ""),
                        new XElement("DEPARTMENT", ""),
                        new XElement("TITLE", ""),
                        new XElement("CONTACT_NUMBER", ""),
                        new XElement("MAIL_ID", "")

                    );
                    CONTDETAILdoc.Root.Add(itemElement);
                //}
                xmlContent += "\r\n" + CONTDETAILdoc.ToString();
                XDocument CONTINFOdoc = new XDocument(
                    new XElement("CONTINFO",
                        new XElement("item",
                            new XElement("PRIMARY_CONTACT_NUMBER", VendorOnBoardingView.primaryContact),
                            new XElement("SECONDARY_CONTACT_NUMBER", VendorOnBoardingView.secondaryContact),
                            new XElement("PRIMARY_MAIL", VendorOnBoardingView.primarymail),
                            new XElement("SECONDARY_MAIL", VendorOnBoardingView.secondarymail)
                           
                        )

                    )
                 );
                xmlContent += "\r\n" + CONTINFOdoc.ToString();
                XDocument GENERALdoc = new XDocument(
                   new XElement("GENERAL",
                       new XElement("item",
                           new XElement("GST", VendorOnBoardingView.gstNumber),
                           new XElement("PAN", VendorOnBoardingView.panNumber),
                           new XElement("NAME", VendorOnBoardingView.name)
                          
                       )
                   )
                );
                xmlContent += "\r\n" + GENERALdoc.ToString();
                //for (int i = 0; i < attachment.Count; i++)
                //{

                    if (gst != null )
                    {

                        string base64String = Convert.ToBase64String(gst.AttachmentFile);
                        filecontent += "<GST>" + base64String + "\r\n</GST>\r\n<GSTFILENAME>" + gst.AttachmentName + "</GSTFILENAME>";

                    }
                    xmlContent += filecontent;
                if(CreatedBy != null)
                {
                    xmlContent += "\r\n<GV_USERID>" + CreatedBy + "</GV_USERID>";
                }
                
                    filecontent = "";

                //}

                XDocument initialvaluedoc = new XDocument(
                    new XElement("KEY",
                    new XElement("item",
                       new XElement("ACCOUNTGROUP", VendorOnBoardingView.accountGroup.Substring(0, 4)),
                        new XElement("COMPANYCODE", VendorOnBoardingView.companyCode.Substring(0, 4)),
                        new XElement("PRUCHASEORGANISATION", VendorOnBoardingView.purchaseOrg.Substring(0, 4)),
                         new XElement("PLANT", VendorOnBoardingView.Plant.Substring(0,4)),
                          new XElement("TYP_OF_SERVICE", VendorOnBoardingView.TypeOfService),
                        new XElement("GST", VendorOnBoardingView.GSTValue.Substring(0, 1)),
                        new XElement("NAME", VendorOnBoardingView.name),
                        new XElement("EMAIL", VendorOnBoardingView.primarymail),
                        new XElement("TRANS_O", VendorOnBoardingView.transID)

                    )
                    )
                 ); 
                xmlContent += "\r\n" + initialvaluedoc.ToString();
                //for (int i = 0; i < attachment.Count; i++).BPVendorOnBoardingView
                //{
                if(msme != null)
                {
                    string base64String = Convert.ToBase64String(msme.AttachmentFile);
                    filecontent += "<MSME>" + base64String + "\r\n</MSME>\r\n<MSMEFILENAME>" + msme.AttachmentName + "</MSMEFILENAME>";
                }
                xmlContent += filecontent;
                filecontent = "";
                if (other != null )
                {

                    string base64String = Convert.ToBase64String(other.AttachmentFile);
                    filecontent += "<OTHERS>" + base64String + "\r\n</OTHERS>\r\n<OTHERSFILENAME>" + other.AttachmentName + "</OTHERSFILENAME>";

                }
                xmlContent += filecontent;
                filecontent = "";
                if (pan != null)
                    {

                        string base64String = Convert.ToBase64String(pan.AttachmentFile);
                        filecontent += "<PAN>" + base64String + "\r\n</PAN>\r\n<PANFILENAME>" + pan.AttachmentName + "</PANFILENAME>";

                    }

               
                    xmlContent += filecontent;
                    filecontent = "";

                //}
                string RemoteUrl = _configuration["SendVendorDetailsLink"]; 
                string username = _configuration["WebServiceUserName"];
                string password = _configuration["Password"];
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
                ServicePointManager.ServerCertificateValidationCallback += validateCertificate;
                using (HttpClient postclients = new HttpClient(clientHandler))
                {
                    
                    string credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username} :{password}"));
                    postclients.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                    string contextstring = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:sap-com:document:sap:rfc:functions\">\r\n   <soapenv:Header/>\r\n   <soapenv:Body>\r\n      <urn:ZMDM_WS_VENDOR>" + xmlContent + "</urn:ZMDM_WS_VENDOR>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>";
                    var contents = new StringContent(contextstring, Encoding.UTF8, "text/xml");
                    try
                    {
                        HttpResponseMessage response = await postclients.PostAsync(new Uri(RemoteUrl), contents);
                        if (response.IsSuccessStatusCode)
                        {
                            //var attachmentsend = await PostAttachmentToSAP(VendorOnBoardingView.transID.ToString());
                            sendreq = true;
                            

                        }
                        else
                        {
                            sendreq = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteToFile("PostDataToSAP/Exception:-  " + ex.Message, ex);
                        sendreq = false;
                    }

                    
                }
                return sendreq;
            }

            catch (Exception ex)
            {

                WriteLog.WriteToFile("PostDataToSAP/Exception:-  " + ex.Message, ex);
                return false;
            }

        }
        public async Task<bool> PostAttachmentToSAP(string TransID)
        {
            string filecontent = "";
            bool attachmentpost = false;
            //var attachment = _dbContext.BPAttachments.Where(x => x.HeaderNumber == TransID.ToString()).ToList();
            //for (int i = 0; i < attachment.Count; i++)
            //{
            //    //if (attachment[i].filetype == "CANCEL CHEQUE")
            //    //{
            //    //    //string result = Encoding.UTF8.GetString(attachment[i].AttachmentFile);
            //    //    //string result = BitConverter.ToString(attachment[i].AttachmentFile);
            //    //    string base64String = Convert.ToBase64String(attachment[i].AttachmentFile);
            //    //    XDocument CancelChequedoc = new XDocument(
            //    //         new XElement("CANCELCHEQUEFILENAME", attachment[i].AttachmentName),
            //    //         new XElement("CANCELCHEQUE", base64String)

            //    //    );
            //    //    //filecontent += CancelChequedoc.ToString();
            //    //}
            //    //else if (attachment[i].filetype == "GSTIN")
            //    //{
            //    //    string attachmentContent = File.ReadAllText(attachment[i].AttachmentFile.ToString());
            //    //    XDocument GSTINdoc = new XDocument(
            //    //         new XElement("GSTINFILENAME", attachment[i].AttachmentName),
            //    //         new XElement("GSTIN", attachmentContent)

            //    //    );
            //    //    filecontent += GSTINdoc.ToString();
            //    //}
            //    if (attachment[i].filetype == "PAN")
            //    {

            //        string base64String = Convert.ToBase64String(attachment[i].AttachmentFile);
            //        filecontent = "<PAN>" + base64String + "\r\n</PAN>\r\n<PANFILENAME>" + attachment[i].AttachmentName + "</PANFILENAME>";

            //    }
            //    //else if(attachment[i].filetype == "Others")
            //    //{
            //    //    string attachmentContent = File.ReadAllText(attachment[i].AttachmentFile.ToString());
            //    //    XDocument Othersdoc = new XDocument(
            //    //        new XElement("OTHERSFILENAME", attachment[i].AttachmentName),
            //    //    new XElement("OTHERS", attachmentContent)
            //    //    );
            //    //    filecontent += Othersdoc.ToString();
            //    //}

            //    //CONTDETAILdoc.Root.Add(itemElement);
            //}
          
            //var WebsericeConfig = _configuration.GetSection("WebService");
            string attachementUrl = _configuration["SendAttachementDetailsLink"];
            string username = _configuration["WebServiceUserName"];
            string password = _configuration["Password"];
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
            ServicePointManager.ServerCertificateValidationCallback += validateCertificate;
            using (HttpClient attachclients = new HttpClient(clientHandler))
            {
                string attachmentcredentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username} :{password}"));
                attachclients.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", attachmentcredentials);
                string attachmentcontextstring = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"\r\nxmlns:urn=\"urn:sap-com:document:sap:rfc:functions\">\r\n<soapenv:Header/>\r\n<soapenv:Body>\r\n<urn:ZMDM_WS_VEN_ATTACH>\r\n" + filecontent + "\r\n</urn:ZMDM_WS_VEN_ATTACH>\r\n</soapenv:Body>\r\n</soapenv:Envelope>";
                var attachmentcontents = new StringContent(attachmentcontextstring, Encoding.UTF8, "text/xml");
                try
                {
                    HttpResponseMessage response1 = await attachclients.PostAsync(new Uri(attachementUrl), attachmentcontents);
                    if (response1.IsSuccessStatusCode)
                    {
                        attachmentpost = true;
                    }
                }
                catch (Exception ex)
                {
                    attachmentpost = false;
                    WriteLog.WriteToFile("PostAttachemntToSAP/Exception:-  " + ex.Message, ex);
                    
                }
            }
            return attachmentpost;
        }
        //public async bool SendRequest(string xmlContent)
        //{
        //    try
        //    {
        //        string RemoteUrl = "http://vhzahds4ci.sap.zenexah.com:8000/sap/bc/srt/rfc/sap/zmdm_ws_vendor/110/zmdm_ws_vendor/zmdm_ws_vendor";
        //        string username = "X101MDMG001";
        //        string password = "Exalca@123";
        //        HttpClientHandler clientHandler = new HttpClientHandler();
        //        clientHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
        //        ServicePointManager.ServerCertificateValidationCallback += validateCertificate;
        //        using (HttpClient client = new HttpClient(clientHandler))
        //        {
        //            client.BaseAddress = new Uri(RemoteUrl);
        //            string credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username} :{password}"));
        //            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
        //            string contextstring = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:sap-com:document:sap:rfc:functions\"><soapenv:Header/><soapenv:Body><urn:ZMDM_WS_VENDOR>" + xmlContent + "</urn:ZMDM_WS_VENDOR></soapenv:Body></soapenv:Envelope>";
        //            var contents = new StringContent(contextstring, Encoding.UTF8, "text/xml");
        //            try
        //            {
        //                HttpResponseMessage response = await client.PostAsync("", contents);

        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception ex )
        //    {
        //        return false;

        //    }
        //}
        private static bool validateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {

            return true;

        }
        public async Task<BPVendorOnBoarding> DeleteVendorOnBoarding(BPVendorOnBoarding VendorOnBoarding)
            {
                try
                {
                    //var entity = await _dbContext.Set<BPVendorOnBoarding>().FindAsync(VendorOnBoarding.VendorOnBoarding, VendorOnBoarding.Language);
                    var entity = _dbContext.Set<BPVendorOnBoarding>().FirstOrDefault(x => x.TransID == VendorOnBoarding.TransID);
                    if (entity == null)
                    {
                        return entity;
                    }

                    _dbContext.Set<BPVendorOnBoarding>().Remove(entity);
                    await _dbContext.SaveChangesAsync();
                    return entity;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public async Task<BPVendorOnBoarding> ApproveVendor(BPVendorOnBoarding VendorOnBoarding)
            {
                try
                {
                    var entity = _dbContext.Set<BPVendorOnBoarding>().FirstOrDefault(x => x.TransID == VendorOnBoarding.TransID);
                    if (entity == null)
                    {
                        return entity;
                    }
                    entity.Status = "Approved";
                    entity.ModifiedBy = VendorOnBoarding.ModifiedBy;
                    entity.ModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                    //await SendMailToApprovalVendor(entity.Email1, entity.Phone1);
                    return entity;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //public List<BPVendorOnBoarding> GetDeclarationID(int TransID)
            //{
            //    var result = (from tb in _dbContext.BPVendorOnBoardings
            //                  where tb.TransID == TransID
            //                  select new BPVendorOnBoarding()
            //                  {
            //                      MSME_Att_ID = tb.MSME_Att_ID,
            //                      TDS_Att_ID = tb.TDS_Att_ID,
            //                      RP_Att_ID = tb.RP_Att_ID
            //                  }).ToList();
            //    return result;
            //}
            //public BPVendorOnBoarding GetAttachmentId(string arg1)
            //{
            //    //_dbContext.Set<BPVendorOnBoarding>().FirstOrDefault(x => x.TransID == VendorOnBoarding.TransID);
            //    var result = (from tb in _dbContext.BPAttachments
            //                  where tb.AttachmentID.ToString() == arg1
            //                  select new BPVendorOnBoarding()
            //                  {
            //                      MSME_Att_ID = tb.AttachmentID.ToString()
            //                  }).FirstOrDefault();
            //    return result;
            //}
           
        //public BPAttachment GetBPAttachmentByAttachmentId(int attachAttachmentId)
            //{
            //    try
            //    {
            //        var result = _dbContext.BPAttachments.FirstOrDefault(x => x.AttachmentID == attachAttachmentId);
            //        return result;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
           
        //public async Task DeleteVendorOnboardingById(string Transid)
            //{
            //    try
            //    {
            //        var VOBresult = _dbContext.BPVendorOnBoardings.Where(x => x.TransID == int.Parse(Transid)).FirstOrDefault();
            //        if (VOBresult != null)
            //        {
            //            _dbContext.BPVendorOnBoardings.Remove(VOBresult);
            //        }
            //        var BANKresult = _dbContext.BPBanks.Where(x => x.TransID == int.Parse(Transid)).FirstOrDefault();
            //        if (BANKresult != null)
            //        {
            //            _dbContext.BPBanks.Remove(BANKresult);
            //        }
            //        var Contactresult = _dbContext.BPContacts.Where(x => x.TransID == int.Parse(Transid)).FirstOrDefault();
            //        if (Contactresult != null)
            //        {
            //            _dbContext.BPContacts.Remove(Contactresult);
            //        }
            //        var IDresult = _dbContext.BPIdentities.Where(x => x.TransID == int.Parse(Transid)).FirstOrDefault();
            //        if (IDresult != null)
            //        {
            //            _dbContext.BPIdentities.Remove(IDresult);
            //        }
            //        var DocResult = _dbContext.BPAttachments.Where(x => x.HeaderNumber == Transid).ToList();
            //        if (DocResult != null)
            //        {
            //            _dbContext.BPAttachments.RemoveRange(DocResult);
            //        }
            //        await _dbContext.SaveChangesAsync();
            //        return;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
           
        //public List<BPVendorOnBoarding> GetDeclaration_toogle(int TransID)
            //{
            //    var result = (from tb in _dbContext.BPVendorOnBoardings
            //                  where tb.TransID == TransID
            //                  select new BPVendorOnBoarding()
            //                  {
            //                      MSME = tb.MSME,
            //                      RP = tb.RP,
            //                      Reduced_TDS = tb.Reduced_TDS
            //                  }).ToList();
            //    return result;
            //}

            //public List<BPVendorOnBoarding> GetDeclarationIDWithAttachment(int DeclarationID)
            //{
            //    int MSME_Att_ID=DeclarationID[0].
            //    //var result =(from tb1  in _dbContext.BPAttachments where tb1.AttachmentID==DeclarationID. )
            //}




            //public List<BPVendorOnBoarding> GetAllVendorOnBoardings()
            //{
            //    try
            //    {
            //        return _dbContext.BPVendorOnBoardings.ToList();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            //public async Task<BPVendorOnBoarding> RejectVendor(BPVendorOnBoarding VendorOnBoarding)
            //{
            //    try
            //    {
            //        var entity = _dbContext.Set<BPVendorOnBoarding>().FirstOrDefault(x => x.TransID == VendorOnBoarding.TransID);
            //        if (entity == null)
            //        {
            //            return entity;
            //        }
            //        entity.Status = "Rejected";
            //        entity.ModifiedBy = VendorOnBoarding.ModifiedBy;
            //        entity.ModifiedOn = DateTime.Now;

            //        entity.Remarks = VendorOnBoarding.Remarks;
            //        var Remark = VendorOnBoarding.Remarks;
            //        await _dbContext.SaveChangesAsync();
            //        //await SendMailToRejectVendor(entity.Email1, entity.Phone1, Remark);
            //        return VendorOnBoarding;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}



        //private Task SendMailToRejectVendor(object email, object phone)
        //{
        //    throw new NotImplementedException();
        //}

        //#region Question 

        //public QuestionnaireResultSet GetQuestionnaireResultSetByQRID()
        //{
        //    try
        //    {
        //        string BaseAddress = _configuration.GetValue<string>("QuestionnaireBaseAddress");
        //        string QRID = "1";
        //        Guid userId = default(Guid);
        //        QuestionnaireResultSet questionnaireResultSet = null;
        //        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(BaseAddress + "api/Questionnaire/GetQuestionnaireResultSetByQRID?QRID=" + QRID + "&" + userId);
        //        request.Method = "GET";
        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        {
        //            Stream dataStream = response.GetResponseStream();
        //            StreamReader reader = new StreamReader(dataStream);
        //            string jsonRes = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //            questionnaireResultSet = JsonConvert.DeserializeObject<QuestionnaireResultSet>(jsonRes);
        //            reader.Close();
        //            dataStream.Close();
        //        }
        //        return questionnaireResultSet;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}


        ////public static bool InsertInvoiceData(InsertInvoiceDetail insertInvoiceDetail)
        ////{
        ////    bool status = false;
        ////    try
        ////    {
        ////        using (var client = new WebClient())
        ////        {
        ////            client.Headers.Add("Content-Type:application/json");
        ////            client.Headers.Add("Accept:application/json");
        ////            var result1 = client.UploadString(BaseAddress + "api/Invoice/InsertInvoiceDetails/", "Post", JsonConvert.SerializeObject(insertInvoiceDetail));
        ////            Log.WriteLog("InsertInvoiceData:- Invoice Data is inserted into Db from Xml");
        ////            InsertInvoiceResponse insertInvoiceResponse = JsonConvert.DeserializeObject<InsertInvoiceResponse>(result1);
        ////            if (insertInvoiceResponse.Status)
        ////            {
        ////                status = true;
        ////            }
        ////            else
        ////            {
        ////                status = false;
        ////            }
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        status = false;
        ////        Log.WriteLog("InsertInvoiceData/Exception:- " + ex.Message);
        ////    }
        ////    return status;
        ////}

        //#endregion

        //ExalcaSMTP
        //public async Task<bool> SendMail(string code, string UserName, string toEmail, string TransID, string siteURL)
        //{
        //    try
        //    {
        //        var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
        //        string hostName = STMPDetailsConfig["Host"];
        //        string SMTPEmail = STMPDetailsConfig["Email"];
        //        string SMTPEmailPassword = STMPDetailsConfig["Password"];
        //        string SMTPPort = STMPDetailsConfig["Port"];
        //        var message = new MailMessage();
        //        string subject = "";
        //        StringBuilder sb = new StringBuilder();
        //        //string UserName = _ctx.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
        //        //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
        //        sb.Append(string.Format("Dear {0},<br/>", UserName));
        //        sb.Append("You have invited to register in our BPCloud by Emami Limited, Request you to proceed with registration");
        //        sb.Append("<p><a href=\"" + siteURL + "/#/register/vendor?token=" + code + "&Id=" + TransID + "&Email=" + toEmail + "\"" + ">Register</a></p>");
        //        sb.Append($"<i>Note: The verification link will expire in {_tokenTimespan} days.<i>");
        //        sb.Append("<p>Regards,</p><p>Admin</p>");
        //        subject = "Vendor Registration Initialization";
        //        SmtpClient client = new SmtpClient();
        //        client.Port = Convert.ToInt32(SMTPPort);
        //        client.Host = hostName;
        //        client.EnableSsl = true;
        //        client.Timeout = 60000;
        //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        client.UseDefaultCredentials = false;
        //        client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
        //        MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
        //        reportEmail.BodyEncoding = UTF8Encoding.UTF8;
        //        reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        //        reportEmail.IsBodyHtml = true;
        //        //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        //        await client.SendMailAsync(reportEmail);
        //        WriteLog.WriteToFile($"Registration link has been sent successfully to {toEmail}");
        //        return true;
        //    }
        //    catch (SmtpFailedRecipientsException ex)
        //    {
        //        for (int i = 0; i < ex.InnerExceptions.Length; i++)
        //        {
        //            SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
        //            if (status == SmtpStatusCode.MailboxBusy ||
        //                status == SmtpStatusCode.MailboxUnavailable)
        //            {
        //                WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
        //            }
        //            else
        //            {
        //                WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
        //            }
        //        }
        //        WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
        //        return false;
        //    }
        //    catch (SmtpException ex)
        //    {
        //        WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpException:- " + ex.Message, ex);
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/Exception:- " + ex.Message, ex);
        //        return false;
        //    }
        //}

        //Emami SMTP
        public async Task<bool> SendMail(string code, string UserName, string toEmail, string TransID, string siteURL, string vendorname,string initiatormail)
        {
            try
            {
                int transID = int.Parse(TransID);
                string formattedTransID = transID.ToString("D5");
                
                if (UserName.Length >= 4)
                {
                    UserName = UserName.Substring(0, 4) + formattedTransID;
                }
                else
                {
                    UserName = UserName + formattedTransID;
                }
                var vendordetails = _dbContext.VendorRegisterLogin.Where(x => x.UserId == UserName).FirstOrDefault();
                string vendorpassword = Decryptpassword(vendordetails.Password1, true);
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                WriteLog.WriteToFile($"Registration link has been sent successfully from {SMTPEmail}");
                //string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                string SMTPPAssword = STMPDetailsConfig["Password"];
                string SiteURL = _configuration.GetValue<string>("SiteURL");
                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                sb.Append(@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> <h2>Zenex Vendor Onboarding</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> <p>Dear concern,</p> <p>You have invited to register in our Vendor OnBoarding, Request you to proceed with registration.</p> <div style='text-align: end;'>" + "<a href=\"" + siteURL + "/#/pages/Vendorlogin?token=" + code + "&Id=" + TransID + "&Email=" + toEmail + "\"" + "><button style='width: 90px;height: 28px; background-color: #039be5;color: white'>Register</button></a></div><p>Use below crendentials for login </p> <p>UserId:- " + UserName + "</p> <p> Password:- " + vendorpassword + "</p> <p>Note: The verification link will expire in " + _tokenTimespan + " days.</p> <p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>"); subject = "Vendor Registration Initialization";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = true;
                
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPPAssword);
                client.UseDefaultCredentials = false;
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                reportEmail.CC.Add(initiatormail);
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                await client.SendMailAsync(reportEmail);
                WriteLog.WriteToFile($"Registration link has been sent successfully to {toEmail}");
                return true;




            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/Exception:- " + ex.Message, ex);
                return false;
            }
        }

        //public async Task<bool> SendMailToInitializedVendor(string toEmail, string password)
        //    {
        //        try
        //        {
        //            //string hostName = ConfigurationManager.AppSettings["HostName"];
        //            //string SMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"];
        //            ////string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
        //            //string SMTPEmailPassword = ConfigurationManager.AppSettings["SMTPEmailPassword"];
        //            //string SMTPPort = ConfigurationManager.AppSettings["SMTPPort"];
        //            var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
        //            string hostName = STMPDetailsConfig["Host"];
        //            string SMTPEmail = STMPDetailsConfig["Email"];
        //            string siteURL = _configuration["PortalAddress"];
        //            //string SMTPEmailPassword = STMPDetailsConfig["Password"];
        //            string SMTPPort = STMPDetailsConfig["Port"];
        //            var message = new MailMessage();
        //            string subject = "";
        //            StringBuilder sb = new StringBuilder();
        //            //string UserName = _dbContext.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
        //            //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
        //            sb.Append(string.Format("Dear {0},<br/>", toEmail));
        //            sb.Append("<p>Your  Registration is Approved.</p>");
        //            sb.Append("<p>Please Login by clicking <a href=\"" + siteURL + "/#/auth/login\">here</a></p>");
        //            sb.Append(string.Format("<p>User name: {0}</p>", toEmail));
        //            sb.Append(string.Format("<p>Password: {0}</p>", password));
        //            sb.Append("<p>Regards,</p><p>Admin</p>");
        //            subject = "BP Cloud Vendor Registration";
        //            SmtpClient client = new SmtpClient();
        //            client.Port = Convert.ToInt32(SMTPPort);
        //            client.Host = hostName;
        //            client.EnableSsl = false;
        //            client.Timeout = 60000;
        //            client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //            //client.UseDefaultCredentials = false;
        //            //client.Credentials = new System.Net.NetworkCredential(SMTPEmail.Trim(), SMTPEmailPassword.Trim());
        //            //client.Credentials = new System.Net.NetworkCredential(SMTPEmail.Trim(), SMTPEmailPassword.Trim());
        //            MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
        //            reportEmail.BodyEncoding = UTF8Encoding.UTF8;
        //            reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        //            reportEmail.IsBodyHtml = true;
        //            await client.SendMailAsync(reportEmail);
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorLog.WriteToFile("Master/SendMail : - ", ex);
        //            throw ex;
        //        }
        //    }
        //    public async Task<bool> SendMailToApprovalVendor(string toEmail, string password)
        //    {
        //        try
        //        {
        //            //string hostName = ConfigurationManager.AppSettings["HostName"];
        //            //string SMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"];
        //            ////string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
        //            //string SMTPEmailPassword = ConfigurationManager.AppSettings["SMTPEmailPassword"];
        //            //string SMTPPort = ConfigurationManager.AppSettings["SMTPPort"];
        //            var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
        //            string hostName = STMPDetailsConfig["Host"];
        //            string SMTPEmail = STMPDetailsConfig["Email"];
        //            string siteURL = _configuration["PortalAddress"];
        //            //string SMTPEmailPassword = STMPDetailsConfig["Password"];
        //            string SMTPPort = STMPDetailsConfig["Port"];
        //            var message = new MailMessage();
        //            string subject = "";
        //            StringBuilder sb = new StringBuilder();
        //            //string UserName = _dbContext.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
        //            //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
        //            sb.Append(string.Format("Dear {0},<br/>", toEmail));
        //            sb.Append("<p>Vendor Registred.</p>");
        //            //sb.Append("<p>Please Login by clicking <a href=\"" + siteURL + "/#/auth/login\">here</a></p>");
        //            //sb.Append(string.Format("<p>User name: {0}</p>", toEmail));
        //            //sb.Append(string.Format("<p>Password: {0}</p>", password));
        //            sb.Append("<p>Regards,</p><p>Admin</p>");
        //            subject = "BP Cloud Vendor Registration";
        //            SmtpClient client = new SmtpClient();
        //            client.Port = Convert.ToInt32(SMTPPort);
        //            client.Host = hostName;
        //            client.EnableSsl = false;
        //            client.Timeout = 60000;
        //            client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //            //client.UseDefaultCredentials = false;
        //            //client.Credentials = new System.Net.NetworkCredential(SMTPEmail.Trim(), SMTPEmailPassword.Trim());
        //            MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
        //            reportEmail.BodyEncoding = UTF8Encoding.UTF8;
        //            reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        //            reportEmail.IsBodyHtml = true;
        //            await client.SendMailAsync(reportEmail);
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorLog.WriteToFile("Master/SendMail : - ", ex);
        //            throw ex;
        //        }
        //    }

        //    public async Task<bool> SendMailToRejectVendor(string toEmail, string password, string Remarks)
        //    {
        //        try
        //        {
        //            //string hostName = ConfigurationManager.AppSettings["HostName"];
        //            //string SMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"];
        //            ////string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
        //            //string SMTPEmailPassword = ConfigurationManager.AppSettings["SMTPEmailPassword"];
        //            //string SMTPPort = ConfigurationManager.AppSettings["SMTPPort"];
        //            var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
        //            string hostName = STMPDetailsConfig["Host"];
        //            string SMTPEmail = STMPDetailsConfig["Email"];
        //            string siteURL = _configuration["PortalAddress"];
        //            //string SMTPEmailPassword = STMPDetailsConfig["Password"];
        //            string SMTPPort = STMPDetailsConfig["Port"];
        //            var message = new MailMessage();
        //            string subject = "";
        //            StringBuilder sb = new StringBuilder();
        //            //string UserName = _dbContext.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
        //            //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
        //            sb.Append(string.Format("Dear {0},<br/>", toEmail));
        //            sb.Append("<p>Your Registration is Rejected.</p>");
        //            sb.Append(string.Format("<p>Remarks:{0}<p>", Remarks));
        //            //sb.Append("<p>Please Login by clicking <a href=\"" + siteURL + "/#/auth/login\">here</a></p>");
        //            //sb.Append(string.Format("<p>User name: {0}</p>", toEmail));
        //            //sb.Append(string.Format("<p>Password: {0}</p>", password));
        //            sb.Append("<p>Regards,</p><p>Admin</p>");
        //            subject = "BP Cloud Vendor Registration";
        //            SmtpClient client = new SmtpClient();
        //            client.Port = Convert.ToInt32(SMTPPort);
        //            client.Host = hostName;
        //            client.EnableSsl = false;
        //            client.Timeout = 60000;
        //            client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //            //client.UseDefaultCredentials = false;
        //            //client.Credentials = new System.Net.NetworkCredential(SMTPEmail.Trim(), SMTPEmailPassword.Trim());
        //            MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
        //            reportEmail.BodyEncoding = UTF8Encoding.UTF8;
        //            reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        //            reportEmail.IsBodyHtml = true;
        //            await client.SendMailAsync(reportEmail);
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorLog.WriteToFile("Master/SendMail : - ", ex);
        //            throw ex;
        //        }
        //    }

            private class ErrorLog
            {
                internal static void WriteToFile(string v, Exception ex)
                {
                    throw new NotImplementedException();
                }
            }

            #region EncryptAndDecrypt

            public string Encrypt(string Password, bool useHashing)
            {
                try
                {
                    string EncryptionKey = _configuration["EncryptionKey"];
                    byte[] KeyArray;
                    byte[] ToEncryptArray = UTF8Encoding.UTF8.GetBytes(Password);
                    if (useHashing)
                    {
                        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                        KeyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(EncryptionKey));
                        hashmd5.Clear();
                    }
                    else
                        KeyArray = UTF8Encoding.UTF8.GetBytes(EncryptionKey);

                    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                    tdes.Key = KeyArray;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;
                    ICryptoTransform cTransform = tdes.CreateEncryptor();
                    byte[] resultArray =
                      cTransform.TransformFinalBlock(ToEncryptArray, 0,
                      ToEncryptArray.Length);

                    tdes.Clear();
                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
                catch (Exception ex)
                {
                    WriteLog.WriteToFile("Master/Encrypt/Exception:- " + ex.Message, ex);
                    return null;
                }
            }

            public string Decrypt(string Password, bool UseHashing)
            {
                try
                {
                    string EncryptionKey = _configuration["EncryptionKey"];
                    byte[] KeyArray;
                    byte[] ToEncryptArray = Convert.FromBase64String(Password);
                    if (UseHashing)
                    {
                        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                        KeyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(EncryptionKey));
                        hashmd5.Clear();
                    }
                    else
                    {
                        KeyArray = UTF8Encoding.UTF8.GetBytes(EncryptionKey);
                    }

                    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                    tdes.Key = KeyArray;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;
                    ICryptoTransform cTransform = tdes.CreateDecryptor();
                    byte[] resultArray = cTransform.TransformFinalBlock(
                                         ToEncryptArray, 0, ToEncryptArray.Length);
                    tdes.Clear();
                    return UTF8Encoding.UTF8.GetString(resultArray);
                }
                catch (Exception ex)
                {
                    WriteLog.WriteToFile("Master/Decrypt/Exception:- " + ex.Message, ex);
                    return null;
                }
            }
        public async Task<AccountCode> GetCompanyAndAccount()
        {
            AccountCode accountcode = new AccountCode();

            try
            {
                //IConfiguration WebsericeConfig = _configuration.GetSection("WebService");
                string RemoteUrl = _configuration["GetInitiatorDetailsLink"];
                string username = _configuration["WebServiceUserName"];
                string password = _configuration["Password"];
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
                ServicePointManager.ServerCertificateValidationCallback += validateCertificate;
                using (HttpClient client = new HttpClient(clientHandler))
                {
                    client.BaseAddress = new Uri(RemoteUrl);
                    string credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username} :{password}"));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                    //string contextstring = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"xmlns:urn=\"urn:sap-com:document:sap:soap:functions:mc-style\"><soapenv:Header/><soapenv:Body><urn:ZmdmWsVen><ItAccount><item><Ktokk></Ktokk><Txt30></Txt30></item></ItAccount><ItCompany><item><Bukrs></Bukrs><Butxt></Butxt></item></ItCompany><ItCounty><item><Land1></Land1><Landx></Landx><Lnplz></Lnplz></item></ItCounty><ItCredit><item><CertId></CertId></item></ItCredit><ItGstclass><item><Gstclass></Gstclass><Text></Text></item></ItGstclass><ItPlant><item><Werks></Werks><Name1></Name1></item></ItPlant><ItPurchasing><item><Ekorg></Ekorg><Ekotx></Ekotx></item></ItPurchasing><ItRegion><item><Land1></Land1><Bland></Bland><Bezei></Bezei></item></ItRegion></urn:ZmdmWsVen></soapenv:Body></soapenv:Envelope>";
                      string contextstring = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"\r\nxmlns:urn=\"urn:sap-com:document:sap:rfc:functions\"><soapenv:Header/><soapenv:Body><urn:ZMDM_WS_VEN><IT_ACCOUNT><item><KTOKK></KTOKK><TXT30></TXT30></item></IT_ACCOUNT><IT_COMPANY><item><BUKRS></BUKRS><BUTXT></BUTXT></item></IT_COMPANY><IT_COUNTY><item><LAND1></LAND1><LANDX></LANDX><LNPLZ></LNPLZ></item></IT_COUNTY><IT_CREDIT><item><CERT_ID></CERT_ID></item></IT_CREDIT><IT_GSTCLASS><item><GSTCLASS></GSTCLASS><TEXT></TEXT></item></IT_GSTCLASS><IT_PLANT><item><VKORG></VKORG><VTWEG>?</VTWEG><WERKS></WERKS><NAME1></NAME1></item></IT_PLANT><IT_PURCHASING><item><EKORG></EKORG><EKOTX></EKOTX></item></IT_PURCHASING><IT_REGION><item><LAND1></LAND1><BLAND></BLAND><BEZEI></BEZEI></item></IT_REGION><IT_TYP_OF_SERVICE><item><TYP_OF_SERVICE></TYP_OF_SERVICE></item></IT_TYP_OF_SERVICE></urn:ZMDM_WS_VEN></soapenv:Body></soapenv:Envelope>";
                    //byte[] contentBytes = Encoding.UTF8.GetBytes(contextstring);
                    //ByteArrayContent content = new ByteArrayContent(contentBytes);
                    //content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml");
                    //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, RemoteUrl);
                    //request.Content = content;
                    var contents = new StringContent(contextstring, Encoding.UTF8, "text/xml");
                    try
                    {
                        //HttpResponseMessage response = await client.SendAsync(request);
                        HttpResponseMessage response = await client.PostAsync("", contents);
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(responseBody);
                            XmlNodeList accountNodes = xmlDoc.SelectNodes("//IT_ACCOUNT/item");
                            foreach (XmlNode node in accountNodes)
                            {

                                if (accountcode.ACC_Key == null)
                                {
                                    accountcode.ACC_Key = new List<string>();
                                }

                                if (accountcode.ACC_Value == null)
                                {
                                    accountcode.ACC_Value = new List<string>();
                                }
                                string KTOKD = node.SelectSingleNode("KTOKK")?.InnerText;
                                string TXT30 = node.SelectSingleNode("TXT30")?.InnerText;
                                accountcode.ACC_Key.Add(KTOKD);
                                accountcode.ACC_Value.Add(TXT30);
                            }
                            XmlNodeList ComapnyNodes = xmlDoc.SelectNodes("//IT_COMPANY/item");
                            foreach (XmlNode node in ComapnyNodes)
                            {
                                if (accountcode.Company_Key == null)
                                {
                                    accountcode.Company_Key = new List<string>();
                                }

                                if (accountcode.Company_Value == null)
                                {
                                    accountcode.Company_Value = new List<string>();
                                }
                                string BUKRS = node.SelectSingleNode("BUKRS")?.InnerText;
                                string BUTXT = node.SelectSingleNode("BUTXT")?.InnerText;
                                accountcode.Company_Key.Add(BUKRS);
                                accountcode.Company_Value.Add(BUTXT);

                                //Console.WriteLine($"accountcode: {accountcode}");
                            }
                            XmlNodeList PurchaseNodes = xmlDoc.SelectNodes("//IT_PURCHASING/item");
                            foreach (XmlNode node in PurchaseNodes)
                            {
                                if (accountcode.PurchaseGrp_Key == null)
                                {
                                    accountcode.PurchaseGrp_Key = new List<string>();
                                }

                                if (accountcode.PurchaseGrp_Value == null)
                                {
                                    accountcode.PurchaseGrp_Value = new List<string>();
                                }
                                string EKORG = node.SelectSingleNode("EKORG")?.InnerText;
                                string EKOTX = node.SelectSingleNode("EKOTX")?.InnerText;
                                accountcode.PurchaseGrp_Key.Add(EKORG);
                                accountcode.PurchaseGrp_Value.Add(EKOTX);
                                //Console.WriteLine($"accountcode: {accountcode}");
                            }
                            XmlNodeList GSTNodes = xmlDoc.SelectNodes("//IT_GSTCLASS/item");
                            foreach (XmlNode node in GSTNodes)
                            {
                                if (accountcode.Gst_Key == null)
                                {
                                    accountcode.Gst_Key = new List<string>();
                                }

                                if (accountcode.Gst_Value == null)
                                {
                                    accountcode.Gst_Value = new List<string>();
                                }
                                string GSTCLASS = node.SelectSingleNode("GSTCLASS")?.InnerText;
                                string TEXT = node.SelectSingleNode("TEXT")?.InnerText;
                                accountcode.Gst_Key.Add(GSTCLASS);
                                accountcode.Gst_Value.Add(TEXT);
                                //Console.WriteLine($"accountcode: {accountcode}");
                            }
                            XmlNodeList CountryNodes = xmlDoc.SelectNodes("//IT_COUNTY/item");
                            foreach (XmlNode node in CountryNodes)
                            {
                                if (accountcode.Country_code == null)
                                {
                                    accountcode.Country_code = new List<string>();
                                }
                                if (accountcode.Country_desc == null)
                                {
                                    accountcode.Country_desc = new List<string>();
                                }
                                if (accountcode.country_code_length == null)
                                {
                                    accountcode.country_code_length = new List<string>();
                                }

                                string C_Code = node.SelectSingleNode("LAND1")?.InnerText;
                                string C_Desc = node.SelectSingleNode("LANDX")?.InnerText;
                                string C_Code_length = node.SelectSingleNode("LNPLZ")?.InnerText;
                                accountcode.Country_code.Add(C_Code);
                                accountcode.Country_desc.Add(C_Desc);
                                accountcode.country_code_length.Add(C_Code_length);

                                //Console.WriteLine($"accountcode: {accountcode}");
                            }
                            XmlNodeList regionNodes = xmlDoc.SelectNodes("//IT_REGION/item");
                            foreach (XmlNode node in regionNodes)
                            {

                                if (accountcode.region_code == null)
                                {
                                    accountcode.region_code = new List<string>();
                                }

                                if (accountcode.region_desc == null)
                                {
                                    accountcode.region_desc = new List<string>();
                                }

                                if (accountcode.region_country == null)
                                {
                                    accountcode.region_country = new List<string>();
                                }
                                string r_country = node.SelectSingleNode("LAND1")?.InnerText;
                                string r_code = node.SelectSingleNode("BLAND")?.InnerText;
                                string r_desc = node.SelectSingleNode("BEZEI")?.InnerText;
                                accountcode.region_country.Add(r_country);
                                accountcode.region_code.Add(r_code);
                                accountcode.region_desc.Add(r_desc);
                            }
                            XmlNodeList plant = xmlDoc.SelectNodes("//IT_PLANT/item");
                            foreach (XmlNode node in plant)
                            {

                                if (accountcode.Plant_Code == null)
                                {
                                    accountcode.Plant_Code = new List<string>();
                                }
                                if (accountcode.Plant_desc == null)
                                {
                                    accountcode.Plant_desc = new List<string>();
                                }
                                string plantcode = node.SelectSingleNode("WERKS")?.InnerText;
                                string plantdesc = node.SelectSingleNode("NAME1")?.InnerText;
                                accountcode.Plant_Code.Add(plantcode);
                                accountcode.Plant_desc.Add(plantdesc);
                            }
                            XmlNodeList typeOfServiceNodes = xmlDoc.SelectNodes("//IT_TYP_OF_SERVICE/item");
                            foreach (XmlNode node in typeOfServiceNodes)
                            {

                                if (accountcode.typeOfService == null)
                                {
                                    accountcode.typeOfService = new List<string>();
                                }
                               
                                string typeOfService = node.SelectSingleNode("TYP_OF_SERVICE")?.InnerText;
                                //string plantdesc = node.SelectSingleNode("NAME1")?.InnerText;
                                accountcode.typeOfService.Add(typeOfService);
                                
                            }
                        }
                        client.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return accountcode;
        }

        //public async Task<BPVendorOnBoarding> UpdateVendorOnBoardingStatus(BPVendorOnBoarding vendor, string status)
        //    {
        //        try
        //        {
        //            var entity = _dbContext.Set<BPVendorOnBoarding>().FirstOrDefault(x => x.TransID == vendor.TransID);
        //            if (entity == null)
        //            {
        //                return entity;
        //            }
        //            entity.Status = status;
        //            entity.ModifiedBy = vendor.ModifiedBy;
        //            entity.ModifiedOn = DateTime.Now;
        //            await _dbContext.SaveChangesAsync();
        //            //await SendMailToApprovalVendor(entity.Email1, entity.Phone1);
        //            return entity;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
       
        public async Task<bool> WelcomeMail(VendorDetailsSample VendorDetails)
        {
            try
            {
                VendorDetails vendordetail = new VendorDetails();
                vendordetail.VendorCode = VendorDetails.VendorCode;
                vendordetail.VendorName = VendorDetails.VendorName;
                vendordetail.state = VendorDetails.state;
                vendordetail.City = VendorDetails.City;
                vendordetail.ToMailId = VendorDetails.ToMailId;
                vendordetail.TransID = VendorDetails.trans_o;
                vendordetail.TransID_SAP = VendorDetails.trans_sap;
                string concatenatedEmails = string.Join(", ", VendorDetails.CCMailId);
                vendordetail.CCMailId = concatenatedEmails;
                var result =  _dbContext.VendorDetails.Add(vendordetail);
                await _dbContext.SaveChangesAsync();

                var vob = _dbContext.BPVendorOnBoardings.FirstOrDefault(x=>x.TransID==int.Parse(vendordetail.TransID));
                vob.Status = "Approved";

                await _dbContext.SaveChangesAsync();
                bool result1 = await WelcomeMailToVendor(VendorDetails.VendorCode, VendorDetails.VendorName, VendorDetails.state, VendorDetails.City, concatenatedEmails, VendorDetails.ToMailId);
                return result1;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("WelocmeMail" + ex.Message);
                return false; 
            }

        }
        public async Task<bool> WelcomeMailToVendor(string Vendorcode, string VendorName, string state, string City, string mail,string Tomail)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                WriteLog.WriteToFile($"Registration link has been sent successfully from {SMTPEmail}");
                //string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                string SMTPPAssword = STMPDetailsConfig["Password"];
                string SiteURL = _configuration.GetValue<string>("SiteURL");
                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                sb.Append(@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> <h2>Zenex Vendor Onboarding</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> <p>Dear Business Associate,</p> <p>Welcome to Zenex Animal Health family!</p> <p>We are delighted to have you on board as our Business Associate. Your operational details are as under.</p> <p>Vendor code: " + Vendorcode + "</p> <p>Vendor Name:  " + VendorName + "</p> <p>City: " + City + "</p> <p> State: " + state + "</p><p>We are sure that with your domain expertise, we can together move towards a prosperous tomorrow.</p><p>In case of any query, please contact with Zenex representative.</p><p>Warm regards,</p> <p>Zenex Animal Health India Private Limited</p> </div> </div> </div></body></html>");
                subject = "Vendor Onboading - Welcome Notes";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = true;

                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPPAssword);
                client.UseDefaultCredentials = false;
                MailMessage mailmessage = new MailMessage();
                mailmessage.From = new MailAddress(SMTPEmail);
                mailmessage.To.Add(Tomail);
                string[] splittedmail = mail.Split(',');
                foreach (string mailid in splittedmail)
                {
                    if(mailid != "")
                    {
                        mailmessage.CC.Add(mailid);
                    }
                    
                }

                mailmessage.Subject = subject;
                mailmessage.Body = sb.ToString();
                mailmessage.BodyEncoding = UTF8Encoding.UTF8;
                mailmessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mailmessage.IsBodyHtml = true;
                await client.SendMailAsync(mailmessage);
                WriteLog.WriteToFile($"Registration link has been sent successfully to {mailmessage.To}");
                return true;

            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        WriteLog.WriteToFile("IdentityRepository/WelcomeSendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("IdentityRepository/WelcomeSendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("IdentityRepository/WelcomeSendMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("IdentityRepository/WelcomeSendMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/Exception:- " + ex.Message, ex);
                return false;
            }
        }
        public string Decryptpassword(string Password, bool UseHashing)
        {
            try
            {
                //string EncryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];
                string EncryptionKey = "Exalca";
                byte[] KeyArray;
                byte[] ToEncryptArray = Convert.FromBase64String(Password);
                if (UseHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    KeyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(EncryptionKey));
                    hashmd5.Clear();
                }
                else
                {
                    KeyArray = UTF8Encoding.UTF8.GetBytes(EncryptionKey);
                }

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = KeyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     ToEncryptArray, 0, ToEncryptArray.Length);
                tdes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("AuthorizationServerProvider/Decrypt :- ", ex);
                return null;
            }

        }

        public string Encryptpassword(string Password, bool useHashing)
        {
            try
            {
                //string EncryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];
                string EncryptionKey = "Exalca";
                byte[] KeyArray;
                byte[] ToEncryptArray = UTF8Encoding.UTF8.GetBytes(Password);
                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    KeyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(EncryptionKey));
                    hashmd5.Clear();
                }
                else
                    KeyArray = UTF8Encoding.UTF8.GetBytes(EncryptionKey);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = KeyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(ToEncryptArray, 0,
                  ToEncryptArray.Length);
                tdes.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("AuthorizationServerProvider/Encrypt :- ", ex);
                return null;
            }
        }

        #endregion
        public static TDerived CopyBaseProperties<TBase, TDerived>(TBase source, TDerived target) where TDerived : TBase
        {
            foreach (var property in typeof(TBase).GetProperties())
            {
                if (property.CanRead && property.CanWrite)
                {
                    var value = property.GetValue(source);
                    property.SetValue(target, value);
                }
            }
            return target;
        }
    }


}
