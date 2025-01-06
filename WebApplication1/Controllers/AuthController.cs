using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static Zenex.Authentication.Models.AuthMaster;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Zenex.Authentication.IRespository;

namespace Zenex.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private IConfiguration _configuration;
        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            //ErrorLog.WriteToFile("Auth/AuthController:- Auth Controller Called");
            _authRepository = authRepository;
            _configuration = configuration;
        }
        [HttpPost]
        [ActionName("token")]
        public async Task<ActionResult> GetToken(LoginModel loginModel)
        {
            ErrorLog.WriteToFile("Auth/GetToken:- Get Token Called");
            try
            {
                Client client = _authRepository.FindClient(loginModel.clientId);
                if (client != null)
                {
                    AuthenticationResult authenticationResult = await _authRepository.AuthenticateUser(loginModel.UserName, loginModel.Password);
                    if (authenticationResult != null)
                    {
                        IConfiguration JWTSecurityConfig = _configuration.GetSection("JWTSecurity");
                        string securityKey = JWTSecurityConfig.GetValue<string>("securityKey");
                        string issuer = JWTSecurityConfig.GetValue<string>("issuer");
                        string audience = JWTSecurityConfig.GetValue<string>("audience");
                        //symmetric security key
                        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
                        //signing credentials
                        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

                        //add claims
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, authenticationResult.UserName));
                        claims.Add(new Claim(ClaimTypes.Role, authenticationResult.UserRole));
                        //claims.Add(new Claim("UserID", authenticationResult.UserID.ToString()));
                        //claims.Add(new Claim("UserName", authenticationResult.UserName));
                        //claims.Add(new Claim("UserRole", authenticationResult.UserRole));
                        //claims.Add(new Claim("DisplayName", authenticationResult.DisplayName));
                        //claims.Add(new Claim("EmailAddress", authenticationResult.EmailAddress));
                        //claims.Add(new Claim("MenuItemNames", authenticationResult.MenuItemNames));
                        //claims.Add(new Claim("IsChangePasswordRequired", authenticationResult.IsChangePasswordRequired));
                        //claims.Add(new Claim("Profile", authenticationResult.Profile));
                        //create token
                        var token = new JwtSecurityToken(
                                issuer: issuer,
                                audience: audience,
                                expires: DateTime.Now.AddHours(4),
                                signingCredentials: signingCredentials,
                                claims: claims
                            );

                        //return token
                        authenticationResult.Token = new JwtSecurityTokenHandler().WriteToken(token);
                        return Ok(authenticationResult);
                    }
                    else
                    {
                        return BadRequest("The user name or password is incorrect");
                    }
                }
                else
                {
                    return BadRequest("Invalid client id");
                }

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Auth/GetToken", ex);
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPost]
        public async Task<ActionResult> vendorlogin(VendorLoginModel vendorloginModel)
        {
            try
            {
                var result = await _authRepository.VendorLogin(vendorloginModel);
                
                    return Ok(result);
                
            }
            catch(Exception ex)
            {
                ErrorLog.WriteToFile("VendorLogin/Exception : ", ex);
                return BadRequest(ex.Message);
            }
        }
    }

}

