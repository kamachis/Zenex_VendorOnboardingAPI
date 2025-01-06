using Microsoft.AspNetCore.Mvc;
using static Zenex.Registration.Models.Registeration;
using Zenex.Registration.IRespository;
using Zenex.DBContext;
using Zenex.Registration.Respository;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using static Zenex.Master.Models.Master;

namespace Zenex.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentRepository _AttachmentRepository;
        private readonly IIdentityRepository _IdentityRepository;
        private readonly ZenexContext _dbContext;

        public AttachmentController(IAttachmentRepository attachmentRepository, ZenexContext dbContext,IIdentityRepository identityRepository)
        {
            _AttachmentRepository = attachmentRepository;
            _dbContext = dbContext;
            _IdentityRepository = identityRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAttachment()
        {
            try
            {
                var request = Request;
                var TransID = request.Form["TransID"].ToString();
                var CreatedBy = request.Form["CreatedBy"].ToString();
                var PerviousFileName = request.Form["PerviousFileName"];
                var filetype = request.Form["FileType"].ToString();
                var IDNumber = request.Form["IDNumber"].ToString();
                //var date = request.Form["fileuploaddate"].ToString();
                var size = request.Form["size"].ToString();
                //string format = "ddd MMM dd yyyy";
                //DateTime result;
                //if (DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                //{
                //    date = result;
                //}
                
                //DateTime filedate;
                //if (DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out filedate))
                //{
                //    date = filedate.ToString("yyyy-MM-dd");
                //    //Console.WriteLine("Parsed date: " + result.ToString("yyyy-MM-dd")); // Output: Parsed date: 2024-05-29
                //}
                //DateTime date = DateTime.ParseExact(request.Form["fileuploaddate"], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                IFormFileCollection postedfiles = request.Form.Files;

                if (postedfiles.Count > 0)
                {
                    for (int i = 0; i < postedfiles.Count; i++)
                    {
                        var FileName = postedfiles[i].FileName;
                        var ContentType = postedfiles[i].ContentType;
                        var ContentLength = postedfiles[i].Length;

                        using (Stream st = postedfiles[i].OpenReadStream())
                        {
                            using (BinaryReader br = new BinaryReader(st))
                            {
                                byte[] fileBytes = br.ReadBytes((Int32)st.Length);
                                if (fileBytes.Length > 0)
                                {
                                    IdentityAttachments BPAttachment = new IdentityAttachments();
                                    BPAttachment.TransID = int.Parse(TransID);
                                    BPAttachment.AttachmentName = FileName;
                                    BPAttachment.ContentType = ContentType;
                                    BPAttachment.ContentLength = ContentLength;
                                    BPAttachment.AttachmentFile = fileBytes;
                                    BPAttachment.DocType = filetype;
                                    BPAttachment.IDNumber = IDNumber;
                                    BPAttachment.Size = size;
                                    BPAttachment.Date = DateTime.Now ;
                                    //BPAttachment result = await _AttachmentRepository.UpdateAttachment(BPAttachment);
                                    IdentityAttachments result = await _AttachmentRepository.UploadUserAttachment(BPAttachment);
                                    //if (result != null)
                                    //{
                                    //    var identities = _dbContext.BPIdentities.Where(x => x.TransID == int.Parse(TransID)).ToList();
                                    //    if (identities.Count != 0)
                                    //    {
                                    //        foreach (var ident in identities)
                                    //        {
                                    //            if (filetype == ident.Type)
                                    //            {
                                    //                ident.AttachmentName = FileName;
                                    //                ident.IDNumber = IDNumber;
                                    //                ident.Size = size;
                                    //                ident.Type = filetype;
                                    //                var resultident = _dbContext.BPIdentities.Add(ident);
                                    //                await _dbContext.SaveChangesAsync();
                                    //            }
                                    //            else
                                    //            {
                                    //                BPIdentity bPIdentities = new BPIdentity();
                                    //                bPIdentities.AttachmentName = FileName;
                                    //                bPIdentities.TransID = int.Parse(TransID);
                                    //                bPIdentities.IDNumber = IDNumber;
                                    //                bPIdentities.Size = size;
                                    //                bPIdentities.Type = filetype;
                                    //                //bPIdentities.CreatedOn = date;
                                    //                var typeidentity = _dbContext.BPIdentities.Add(bPIdentities);
                                    //                await _dbContext.SaveChangesAsync();
                                    //            }
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        BPIdentity bPIdentity = new BPIdentity();
                                    //        bPIdentity.AttachmentName = FileName;
                                    //        bPIdentity.TransID = int.Parse(TransID);
                                    //        bPIdentity.IDNumber = IDNumber;
                                    //        bPIdentity.Size = size;
                                    //        bPIdentity.Type = filetype;
                                    //        //bPIdentity.CreatedOn = date;
                                    //        var resultidentity = _dbContext.BPIdentities.Add(bPIdentity);
                                    //        await _dbContext.SaveChangesAsync();
                                    //    }

                                    //}

                                    //await _IdentityRepository.CreateIdentities(bPIdentity, int.Parse(TransID));

                                    return Ok(result);
                                }
                            }

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Attachment/AddAttachment", ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        //[HttpPost]
        //public async Task<IActionResult> UploadUserAttachment()
        //{
        //    try
        //    {
        //        var request = Request;
        //        var TransID = request.Form["TransID"].ToString();
        //        var CreatedBy = request.Form["CreatedBy"].ToString();
        //        IFormFileCollection postedfiles = request.Form.Files;

        //        if (postedfiles.Count > 0)
        //        {
        //            for (int i = 0; i < postedfiles.Count; i++)
        //            {
        //                var FileName = postedfiles[i].FileName;
        //                var ContentType = postedfiles[i].ContentType;
        //                var ContentLength = postedfiles[i].Length;
        //                using (Stream st = postedfiles[i].OpenReadStream())
        //                {
        //                    using (BinaryReader br = new BinaryReader(st))
        //                    {
        //                        byte[] fileBytes = br.ReadBytes((Int32)st.Length);
        //                        if (fileBytes.Length > 0)
        //                        {
        //                            BPAttachment BPAttachment = new BPAttachment();
        //                            BPAttachment.HeaderNumber = TransID;
        //                            BPAttachment.AttachmentName = FileName;
        //                            BPAttachment.ContentType = ContentType;
        //                            BPAttachment.ContentLength = ContentLength;
        //                            BPAttachment.AttachmentFile = fileBytes;
        //                            BPAttachment result = await _AttachmentRepository.UpdateAttachment(BPAttachment);
        //                        }
        //                    }

        //                }

        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("Attachment/AddAttachment", ex);
        //        return BadRequest(ex.Message);
        //    }
        //    return Ok();
        //}

        //[HttpGet]
        //public List<BPAttachment> FilterAttachments(string ProjectName, int AppID = 0, string AppNumber = null)
        //{
        //    try
        //    {
        //        var attachments = _AttachmentRepository.FilterAttachments(ProjectName, AppID, AppNumber);
        //        return attachments;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("Attachment/FilterAttachments", ex);
        //        return null;
        //    }
        //}

        //[HttpGet]
        //public IActionResult DowloandAttachmentByID(int AttachmentID)
        //{
        //    try
        //    {
        //        BPAttachment BPAttachment = _AttachmentRepository.GetAttachmentByID(AttachmentID);
        //        if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
        //        {
        //            Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
        //            return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("Attachment/DowloandAttachmentByID", ex);
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpGet]
        //public IActionResult GetIdentityAttachment(string AppNumber, string HeaderNumber, string AttachmentName)
        //{
        //    try
        //    {
        //        BPAttachment BPAttachment = _AttachmentRepository.GetIdentityAttachment(AppNumber, HeaderNumber, AttachmentName);
        //        if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
        //        {
        //            Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
        //            return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("Attachment/DowloandAttachmentByID", ex);
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost]
        //public async Task<BPAttachment> AddDeclerationAttachment([FromForm] IFormFile[] Files)
        //{
        //    try
        //    {
        //        var result = new BPAttachment();
        //        var files = Files;
        //        var request = Request;
        //        var TransID = request.Form["TransID"].ToString();
        //        var CreatedBy = request.Form["CreatedBy"].ToString();
        //        var Name = request.Form["Name"].ToString();
        //        IFormFileCollection postedfiles = request.Form.Files;

        //        if (postedfiles.Count > 0)
        //        {
        //            for (int i = 0; i < postedfiles.Count; i++)
        //            {
        //                var FileName = postedfiles[i].FileName;
        //                var ContentType = postedfiles[i].ContentType;
        //                var ContentLength = postedfiles[i].Length;
        //                using (Stream st = postedfiles[i].OpenReadStream())
        //                {
        //                    using (BinaryReader br = new BinaryReader(st))
        //                    {
        //                        byte[] fileBytes = br.ReadBytes((Int32)st.Length);
        //                        if (fileBytes.Length > 0)
        //                        {
        //                            BPAttachment BPAttachment = new BPAttachment();
        //                            BPAttachment.HeaderNumber = TransID;
        //                            BPAttachment.AttachmentName = FileName;
        //                            BPAttachment.ContentType = ContentType;
        //                            BPAttachment.ContentLength = ContentLength;
        //                            BPAttachment.AttachmentFile = fileBytes;
        //                            result = await _AttachmentRepository.AddDeclerationAttachment(BPAttachment, TransID, Name);
        //                            if (result != null)
        //                            {
        //                                try
        //                                {
        //                                    if (Name == "MSME")
        //                                    {
        //                                        BPVendorOnBoarding VPVendor = _dbContext.BPVendorOnBoardings.Where(x => x.TransID == int.Parse(TransID)).FirstOrDefault();
        //                                        VPVendor.MSME_Att_ID = result.AttachmentID.ToString();
        //                                        await _dbContext.SaveChangesAsync();
        //                                    }
        //                                    //else if (Name == "RP")
        //                                    //{
        //                                    //    BPVendorOnBoarding VPVendor = _dbContext.BPVendorOnBoardings.Where(x => x.TransID == int.Parse(TransID)).FirstOrDefault();
        //                                    //    VPVendor.RP_Att_ID = result.AttachmentID.ToString();
        //                                    //    await _dbContext.SaveChangesAsync();
        //                                    //}
        //                                    //else if (Name == "LTDS")
        //                                    //{
        //                                    //    BPVendorOnBoarding VPVendor = _dbContext.BPVendorOnBoardings.Where(x => x.TransID == int.Parse(TransID)).FirstOrDefault();
        //                                    //    VPVendor.TDS_Att_ID = result.AttachmentID.ToString();
        //                                    //    await _dbContext.SaveChangesAsync();
        //                                    //}
        //                                    else
        //                                    {
        //                                        throw new Exception("Decleration Name is not found");
        //                                    }
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    throw ex;
        //                                }
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[HttpGet]
        //public IActionResult DowloandAttachmentByIDAndName(string HeaderNumber, string AttachmentName)
        //{
        //    try
        //    {
        //        BPAttachment BPAttachment = _AttachmentRepository.GetAttachmentByIDAndName(HeaderNumber, AttachmentName);
        //        if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
        //        {
        //            Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
        //            return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("Attachment/DowloandAttachmentByID", ex);
        //        return BadRequest(ex.Message);
        //    }
        //}

    }
}
