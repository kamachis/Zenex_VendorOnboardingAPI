using static Zenex.Authentication.Models.AuthMaster;

namespace Zenex.Authentication.IRespository
{
    public interface IAuthRepository
    {
        Client FindClient(string clientId);
        Task<AuthenticationResult> AuthenticateUser(string UserName, string Password);
         Task<bool> VendorLogin(VendorLoginModel VendorLoginModel);
    }
}
