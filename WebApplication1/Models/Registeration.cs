using static Zenex.Authentication.Models.AuthMaster;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Zenex.Registration.Models
{
    public class Registeration
    {
        //[Table("bp_act_log")]
        //public class BPActivityLog : CommonClass
        //{
        //    [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //    public int LogID { get; set; }
        //    [ForeignKey("BPVendorOnBoarding"), Column(Order = 2)]
        //    public int TransID { get; set; }
        //    public string Activity { get; set; }
        //    public DateTime? Date { get; set; }
        //    public string Time { get; set; }
        //    public string Text { get; set; }
        //    public virtual BPVendorOnBoarding? BPVendorOnBoarding { get; set; } 
        //}
        [Table("bp_text")]
        public class BPText : CommonClass
        {
            [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int TextID { get; set; }
            public string Text { get; set; }
        }
        public class CommonClass
        {
            public bool IsActive { get; set; }
            public DateTime? CreatedOn { get; set; }
            public string? CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public string? ModifiedBy { get; set; }
        }
        //[Table("bp_id")]
        //public class BPIdentity : CommonClass
        //{
        //    [Key, ForeignKey("BPVendorOnBoarding"), Column(Order = 1)]
        //    public int TransID { get; set; }
        //    [Key, Column(Order = 2)]
        //    public string Type { get; set; }
        //    //public string? Option { get; set; }
        //    public string? IDNumber { get; set; }
        //    //public DateTime? ValidUntil { get; set; }
        //    public string? DocID { get; set; }
        //    public string? AttachmentName { get; set; }
        //    //public string? AttachmentContents { get; set; }
        //    public DateTime Date { get; set; }
        //    public string? Size { get; set; }
        //    //public bool IsValid { get; set; }
        //    public virtual BPVendorOnBoarding? BPVendorOnBoarding { get; set; }
        //}
       
        [Table("bp_bank")]
        public class BPBank : CommonClass
        {
            [Key, ForeignKey("BPVendorOnBoarding"), Column(Order = 1)]
            public int TransID { get; set; }
            [Key, Column(Order = 2)]
            public string AccountNo { get; set; }
            public string IFSC { get; set; }
            public string BankName { get; set; }
            public string Branch { get; set; }
            public string City { get; set; }
            //public string? Country { get; set; }
            //public string? DocID { get; set; }
            //public string? AttachmentName { get; set; }
            //public bool IsValid { get; set; }
            public virtual BPVendorOnBoarding? BPVendorOnBoarding { get; set; }
        }
        //public class BPAttachment
        //{
        //    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //    public int AttachmentID { get; set; }
        //    public string? ProjectName { get; set; }
        //    public int AppID { get; set; }
        //    public string? AppNumber { get; set; }
        //    public bool IsHeaderExist { get; set; }
        //    public string? HeaderNumber { get; set; }
        //    public string? AttachmentName { get; set; }
        //    public string? ContentType { get; set; }
        //    public string filetype { get; set; }
        //    public long ContentLength { get; set; }
        //    public byte[] AttachmentFile { get; set; }
        //    public bool IsActive { get; set; }
        //    public DateTime? CreatedOn { get; set; }
        //    public string? CreatedBy { get; set; }
        //    public DateTime? ModifiedOn { get; set; }
        //    public string? ModifiedBy { get; set; }
        //}
        
        public class TokenHistory
        {
            [Key]
            public int TokenHistoryID { get; set; }
            public int TransID { get; set; }
            public string? UserName { get; set; }
            public string? Token { get; set; }
            public string? OTP { get; set; }
            public string? EmailAddress { get; set; }
            public DateTime  CreatedOn { get; set; }
            public DateTime  ExpireOn { get; set; }
            public DateTime? UsedOn { get; set; }
            public bool  IsUsed { get; set; }
            public string? Comment { get; set; }
        }
        public class BPAttachment1
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int AttachmentID { get; set; }
            public string ProjectName { get; set; }
            public int AppID { get; set; }
            public string Type { get; set; }
            public string AppNumber { get; set; }
            public bool IsHeaderExist { get; set; }
            public string HeaderNumber { get; set; }
            public string AttachmentName { get; set; }
            public string ContentType { get; set; }
            public long ContentLength { get; set; }
            public byte[] AttachmentFile { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedOn { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public string ModifiedBy { get; set; }
        }

        public class FTPAttachment
        {
            public int AttachmentID { get; set; }
            public string TransID { get; set; }
            public string Type { get; set; }
            public string AttachmentName { get; set; }
            public byte[] AttachmentFile { get; set; }
        }
        public class VendorInitialzationClass
        {
            public int TransID { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string AccountGroup { get; set; }
            public string PurchaseOrg { get; set; }
            public string CompanyCode { get; set; }
            public string GSTValue { get; set; }
            public string CreatedUsername { get; set; }
            public string CreatedMailID { get; set; }
            public string Plant { get; set; }
            public bool? IsDocRequired { get; set; }
            public bool? BankDetailsRequired { get; set; }
            public string? TypeOfService { get; set; }

        }
        public class VendorTokenCheck
        {
            public int TransID { get; set; }
            public string EmailAddress { get; set; }
            public string Token { get; set; }
            public bool? IsValid { get; set; }
            public string? Message { get; set; }
        }
        public class BPVendorOnBoardingView : CommonClass
        {
            public int transID { get; set; }
            public string name { get; set; }
            public string? Useridvalue { get;set; }
            public string? AddressLine1 { get; set; }
            public string? AddressLine2 { get; set; }
            public string? AddressLine3 { get; set; }
            public string? AddressLine4 { get; set; }
            public string? AddressLine5 { get; set; }
            public string? city { get; set; }
            //public string? state { get; set; }
            public string? country { get; set; }
            public string? pinCode { get; set; }
            
            public string? gstNumber { get; set; }
            public string? gstStatus { get; set; }
            public string? panNumber { get; set; }
            public string? primaryContact { get; set; }
            public string? secondaryContact { get; set; }
            public string? primarymail { get; set; }
            public string? secondarymail { get; set; }
            public string? GSTValue { get; set; }
            public string? stateCode { get; set; }
            public string? status { get; set; }
            public string accountGroup { get; set; }
            public string purchaseOrg { get; set; }
            public string? department { get; set; }
            public string companyCode { get; set; }
            //public string typeofIndustry { get; set; }
            public string msmE_TYPE { get; set; }
            public string? msmE_Att_ID { get; set; }
            public string token { get; set; }
            //public List<BPIdentity> bPIdentities { get; set; }
            public List<BPBank> bPBanks { get; set; }
            //public List<BPContact> bPContacts { get; set; }
            public string? certificateNo { get; set; }
            public DateTime? validFrom { get; set; }
            public DateTime? validTo { get; set; }
            public string? registeredCity { get; set; }
            //public string createdUser { get;set; }
            //public string? certificateStatus { get; set; }

            public string Plant { get; set; }
            public string? TypeOfService { get; set; }
        }
        [Table("VendorRegisterLogin")]
        public class VendorRegisterLogin : CommonClass
        {

            [Key]
            public int Id { get; set; }
            public string UserId { get; set; }
            public string VendorMail { get; set; }
            public string Password1 { get; set; }
            public string Password2 { get; set; }
        }
        [Table("GSTFields")]
        public class GSTField
        {
            [Key]
            public string? Code { get; set; }
            public string? Description { get; set; }
            public bool? IsMandatory { get; set; }
        }
        [Table("bp_vob")]
        public class BPVendorOnBoarding : CommonClass
        {
            [Key, Column(Order = 1)]
            public int TransID { get; set; }
            public string? Name { get; set; }
            public string? AddressLine1 { get; set; }
            public string? AddressLine2 { get; set; }
            public string? AddressLine3 { get; set; }
            public string? AddressLine4 { get; set; }
            public string? AddressLine5 { get; set; }
            public string? City { get; set; }
            //public string? State { get; set; }
            public string? Country { get; set; }
            public string? PinCode { get; set; }
            public string? StateCode { get; set; }
            public string? Type { get; set; }
            public string? GSTNumber { get; set; }
            public string? GSTStatus { get; set; }
            public string? PANNumber { get; set; }
            public string? PrimaryContact { get; set; }
            public string? SecondaryContact { get; set; }
            public string? Primarymail { get; set; }
            public string? Secondarymail { get; set; }
            public string? Status { get; set; }
            public string? GSTValue { get; set; }
            public string? AccountGroup { get; set; }
            public string? PurchaseOrg { get; set; }
            public string? Department { get; set; }
            public string? CompanyCode { get; set; }
            //public string? TypeofIndustry { get; set; }
            public string? MSME_TYPE { get; set; }
            public string? MSME_Att_ID { get; set; }
            public string?   CertificateNo { get; set; }
            public DateTime? ValidFrom { get; set; }
            public DateTime? ValidTo { get; set; }
            public string? RegisteredCity { get; set; }
            //public string? CertificateStatus { get; set; }
            public bool? IsDocRequired { get; set; }
            public bool? BankDetailsRequired { get; set; }
            public string Plant { get; set; }
            public string? TypeOfService { get; set; }

        }
        public class ApproverUser
        {
            public string UserName { get; set; }
            public string Email { get; set; }
        }
        [Table("bp_contact")]
        public class BPContact : CommonClass
        {
            [Key, ForeignKey("BPVendorOnBoarding"), Column(Order = 1)]
            public int TransID { get; set; }
            [Key, Column(Order = 2)]
            //public string? Item { get; set; }
            public string? Name { get; set; }
            public string? Department { get; set; }
            public string? Title { get; set; }
            public string? Mobile { get; set; }
            public string? Email { get; set; }
            public virtual BPVendorOnBoarding? BPVendorOnBoarding { get; set; }
        }
        public class AccountCode
        {
            public List<string> ACC_Key { get; set; }
            public List<string> ACC_Value { get; set; }
            public List<string> Company_Key { get; set; }
            public List<string> Company_Value { get; set; }
            public List<string> PurchaseGrp_Key { get; set; }
            public List<string> PurchaseGrp_Value { get; set; }
            public List<string> Gst_Key { get; set; }
            public List<string> Gst_Value { get; set; }
            public List<string> Plant_Code { get; set; }
            public List<string> Plant_desc { get; set; }
            public List<string> Country_code { get; set; }
            public List<string> Country_desc { get; set; }
            public List<string> country_code_length { get; set; }
            public List<string> region_country { get; set; }
            public List<string> region_code { get; set; }
            public List<string> region_desc { get; set; }
            public List<string> typeOfService {  get; set; }
            //public List<string> MSME_Credit { get; set; }
            //public List<string> TXT30 { get; set; }

        }
        public class VendorDetails
        {
            [Key, Column(Order = 1)]
            public int id { get; set; }
            public string VendorName { get; set; }
            public string VendorCode { get; set; }
            public string ToMailId { get; set; }
            public string CCMailId { get; set; }
            // public string Mail { get; set; }
            public string City { get; set; }
            public string state { get; set; }
            public string TransID { get; set; }
            public string TransID_SAP { get; set; }
        }
        public class VendorDetailsSample
        {
            
            public string VendorName { get; set; }
            public string VendorCode { get; set; }
            public string ToMailId { get; set; }
            public string[] CCMailId { get; set; }
            // public string[] Mail { get; set; }
            public string City { get; set; }
            public string state { get; set; }
            public string trans_o { get; set; }
            public string trans_sap { get; set; }
        }
        public class BPVendorOnBoardingWithSapTransID: BPVendorOnBoarding
        {           
            public string TransID_SAP {  get; set; }

        }
    }
}
