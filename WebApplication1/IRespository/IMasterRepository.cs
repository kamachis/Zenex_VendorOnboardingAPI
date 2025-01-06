﻿using static Zenex.Authentication.Models.AuthMaster;
using Zenex.DBContext;
using static Zenex.Registration.Models.Registeration;

namespace Zenex.Authentication.IRespository
{
    public interface IMasterRepository
    {
        #region Authentication
        Client FindClient(string clientId);
        Task<AuthenticationResult> AuthenticateUser(string UserName, string Password);

        #endregion

        #region User

        List<UserWithRole> GetAllUsers();
        List<User> GetAdminUsers();
        List<User> GetApprovers();
        Task<UserWithRole> CreateUser(UserWithRole userWithRole);
        Task<UserWithRole> UpdateUser(UserWithRole userWithRole);
        Task<UserWithRole> DeleteUser(UserWithRole userWithRole);
        Task<UserWithRole> CreateVendorUser(VendorUser vendorUser);
        Task<UserWithRole> CreateApproverUser(Models.AuthMaster.ApproverUser approverUser);
        List<string> GetApproverPlants(Guid UserID);
        #endregion

        #region Role

        List<RoleWithApp> GetAllRoles();
        Task<RoleWithApp> CreateRole(RoleWithApp roleWithApp);
        Task<RoleWithApp> UpdateRole(RoleWithApp roleWithApp);
        Task<RoleWithApp> DeleteRole(RoleWithApp roleWithApp);
        Task<BPVendorOnBoardingView> SendMailToApprover(BPVendorOnBoardingView vendor);
        #endregion

        #region App

        List<App> GetAllApps();
        Task<App> CreateApp(App app);
        Task<App> UpdateApp(App app);
        Task<App> DeleteApp(App app);

        #endregion

        #region SessionMaster

        SessionMaster GetSessionMasterByProject(string ProjectName);
        List<SessionMaster> GetAllSessionMasters();
        List<SessionMaster> GetAllSessionMastersByProject(string ProjectName);
        Task<SessionMaster> CreateSessionMaster(SessionMaster SessionMaster);
        Task<SessionMaster> UpdateSessionMaster(SessionMaster SessionMaster);
        Task<SessionMaster> DeleteSessionMaster(SessionMaster SessionMaster);

        #endregion

        #region LogInAndChangePassword

        Task<UserLoginHistory> LoginHistory(Guid UserID, string Username);
        List<UserLoginHistory> GetAllUsersLoginHistory();
        List<UserLoginHistory> GetCurrentUserLoginHistory(Guid UserID);
        Task<UserLoginHistory> SignOut(Guid UserID);

        #endregion

        #region ChangePassword

        Task<User> ChangePassword(ChangePassword changePassword);
        Task<User> SendResetLinkToMail(EmailModel emailModel);
        Task<Models.AuthMaster.TokenHistory> ForgotPassword(ForgotPassword forgotPassword);
        Task<bool> changepassword(ForgotPasswordModel forgotPassword);

        #endregion

        #region sendMail

        //Task<bool> SendMail(string code, string UserName, string toEmail, string userID, string siteURL);
        //Task<bool> SendMailToVendor(string toEmail, string password);

        #endregion

        #region EncryptAndDecrypt
        string Decrypt(string Password, bool UseHashing);
        string Encrypt(string Password, bool useHashing);

        #endregion
    }
}
