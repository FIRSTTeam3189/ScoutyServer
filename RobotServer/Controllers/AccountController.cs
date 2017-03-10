using System.Linq;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using ScoutingServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RobotServer.Models;
using Microsoft.Extensions.Logging;
using RobotServer.Interfaces;
using RobotServer.SQLDataObjects;
using RobotServer.ClientData;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Web.Http;
using AspNet.Security.OpenIdConnect.Server;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Extensions;
using Microsoft.AspNetCore.Http;

namespace ScoutingServer.Controllers {

    [Route("api/[Controller]")]
    public class AccountController : Controller {
        public const int MIN_PASSWORD_LENGTH = 8;
        public const int MAX_PASSWORD_LENGTH = 128;
        private readonly RoboContext context;
        private readonly ILogger logger;

        public AccountController(ILoggerFactory loggerFactory, RoboContext context) {
            this.context = context;
            logger = loggerFactory.CreateLogger("Account");
        }

        [Route("CustomRegister")]
        [ActionName("CustomRegister")]
        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage CustomRegister(CustomRegistrationRequest request) {
            string error = null;
            RegisterCheck(request.Username, request.Password);
            if(error != null) {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            } else {
                byte[] salt = CustomLoginProviderUtils.generateSalt();
                string guid = Guid.NewGuid().ToString();
                context.SaveChanges();

                Account newAccount = new Account(guid) {
                    Username = request.Username,
                    Salt = salt,
                    SaltedAndHashedPassword = CustomLoginProviderUtils.hash(request.Password, salt)
                };

                context.Accounts.Add(newAccount);
                context.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.Created);
            }
        }
        /*
        [Route("Register")]
        [ActionName("Register")]
        [Authorize]
        [HttpPost]
        public async Task<HttpResponseMessage> Register(RegistrationRequest request) {
            string error = RegisterCheck(request.Username, null);
            if(error != null) {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            } else {
                ClaimsPrincipal principal = this.User as ClaimsPrincipal;
                string provider = principal.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider").Value;

                ProviderCredentials creds = null;
                if(string.Equals(provider, "facebook", StringComparison.OrdinalIgnoreCase)) {
                    creds = await this.User.GetAppServiceIdentityAsync<FacebookCredentials>(this.Request);
                } else if(string.Equals(provider, "google", StringComparison.OrdinalIgnoreCase)) {
                    creds = await this.User.GetAppServiceIdentityAsync<GoogleCredentials>(this.Request);
                } else if(string.Equals(provider, "twitter", StringComparison.OrdinalIgnoreCase)) {
                    creds = await this.User.GetAppServiceIdentityAsync<TwitterCredentials>(this.Request);
                } else if(string.Equals(provider, "microsoft", StringComparison.OrdinalIgnoreCase)) {
                    creds = await this.User.GetAppServiceIdentityAsync<MicrosoftAccountCredentials>(this.Request);
                }

                string GodDamnFuckingId = string.Format("{0}:{1}", creds.Provider, creds.Claims[ClaimTypes.NameIdentifier]);


                if(GodDamnFuckingId != null) {
                    Account newAccount = new Account(GodDamnFuckingId) {
                        Username = request.Username,
                    };
                    context.Accounts.Add(newAccount);
                    context.SaveChanges();

                    return new HttpResponseMessage(HttpStatusCode.Created);
                } else {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
        }*/

        public string RegisterCheck(string username, string password) {
            if(username.Length >= 4 && username.Length <= 16 && !Regex.IsMatch(username, "^[a-zA-Z0-9]{4,}$")) {
                return "Invalid username (at least 4 chars and less than 16, alphanumeric only)";
            } else if(password != null && password.Length <= MIN_PASSWORD_LENGTH && password.Length >= MAX_PASSWORD_LENGTH) {
                return "Invalid password (at least 8 and less than 128 chars required)";
            }

            Account account = context.Accounts.Where(a => a.Username == username).FirstOrDefault();
            if(account != null) {
                if(!string.IsNullOrWhiteSpace(account.Username) && account.Username == username) {
                    return "That username already exists.";
                }
                //TODO fix the commented code.
            }
            return null;
        }

        [Route("Login")]
        [ActionName("Login")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginRequest request) {
            if(!Regex.IsMatch(request.Username, "^[a-zA-Z0-9]{4,}$")) {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            if(!IsPasswordValid(request.Username, request.Password))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var info = context.Accounts.FirstOrDefault(d => d.Username.ToLower() == request.Username.ToLower());

            if(info == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            byte[] incoming = CustomLoginProviderUtils.hash(request.Password, info.Salt);

            if(!CustomLoginProviderUtils.slowEquals(incoming, info.SaltedAndHashedPassword))
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var temp = new ClaimsIdentity(new GenericIdentity(request.Username, "Token"), new Claim[] { });

            var identity = new ClaimsIdentity(
                OpenIdConnectServerDefaults.AuthenticationScheme,
                OpenIdConnectConstants.Claims.Name, null);

            identity.AddClaim(OpenIdConnectConstants.Claims.Subject,
                Guid.NewGuid().ToString(),
                OpenIdConnectConstants.Destinations.AccessToken);

            identity.AddClaim(OpenIdConnectConstants.Claims.Name, info.Username,
                OpenIdConnectConstants.Destinations.AccessToken);
            var principal = new ClaimsPrincipal(identity);
            // Ask OpenIddict to generate a new token and return an OAuth2 token response.
//            {
//                "access_token"  : "...",
//                "token_type"    : "...",
//                "expires_in"    : "...",
//                "refresh_token" : "...",
//            }
            return SignIn(principal, OpenIdConnectServerDefaults.AuthenticationScheme);
        }

        [Route("Logout")]
        [ActionName("Logout")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Logout(LoginRequest request) {
            return SignOut();
        }

        private bool IsPasswordValid(string username, string password) {
            var security = context.Accounts.FirstOrDefault(a => a.Username == username);
            if(security != null) {
                byte[] incoming = CustomLoginProviderUtils.hash(password, security.Salt);
                if(CustomLoginProviderUtils.slowEquals(incoming, security.SaltedAndHashedPassword)) {
                    return true;
                }
            }
            return false;
        }

        [Authorize]
        [Route("GetMyAccount")]
        [ActionName("GetMyAccount")]
        [HttpPost]
        public async Task<ClientAccount> GetMyAccount() {
            return (await GetAccount(context, User, Request)).GetClientAccount();
        }

        [Authorize]
        [Route("GetAccountInfo")]
        [ActionName("GetAccountInfo")]
        [HttpPost]
        public ClientAccount GetAccount(string username) {
            return context.Accounts.FirstOrDefault(a => a.Username == username).GetClientAccount();
        }

        [Authorize]
        [Route("GetAccountInfos")]
        [ActionName("GetAccountInfos")]
        [HttpPost]
        public List<ClientAccount> GetAccount(IList<string> id) {
            foreach(var i in id) {
            }
            var results = context.Accounts.Where(a => id.Contains(a.Id)).ToList().GetClientList(context);
            return results;
        }

        [Authorize]
        [Route("UsernameExists")]
        [ActionName("UsernameExists")]
        [HttpPost]
        public HttpResponseMessage UsernameExists(string un) {
            var thing = context.Accounts.Where(a => a.Username == un).SingleOrDefault();
            if(thing != null && thing.Id != null) {
                return new HttpResponseMessage(HttpStatusCode.Found);
            }
            return new HttpResponseMessage(HttpStatusCode.Unused);
        }

        /*[Authorize]
        [Route("ChangeRole")]
        [ActionName("ChangeRole")]
        [HttpPost]
        public HttpResponseMessage ChangeRole(ChangeRoleRequest request) {
            MobileServiceContext context = new MobileServiceContext();
            var account = GetAccount(context, User);
            if(account.role.IsDevLevel()) {
                var user = context.Accounts.Where(a => a.Id == request.UserId).SingleOrDefault();

                if(user != null) {
                    user.role.SetLevel(request.Level);
                    return Request.CreateResponse(HttpStatusCode.OK);
                } else {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Could not find User");
                }
            } else {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        [Authorize]
        [Route("GetRole")]
        [ActionName("GetRole")]
        [HttpPost]
        public int GetRole(string UserId) {
            MobileServiceContext context = new MobileServiceContext();
            var account = GetAccount(context, User);
            if(account.role.IsModLevel()) {
                var user = context.Accounts.Where(a => a.Id == UserId).SingleOrDefault();

                if(user != null) {
                    return user.role.GetLevel();
                } else {
                    return -1;
                }
            } else {
                return -1;
            }
        }*/

        public static async Task<Account> GetAccount(RoboContext context, IPrincipal User, HttpRequest Request) {
            ClaimsPrincipal principal = User as ClaimsPrincipal;
            string provider = principal.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider")?.Value;

            //ProviderCredentials creds = null;
            string UserId;
            Account account = null;

            if(string.IsNullOrWhiteSpace(provider)) {
                UserId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                account = context.Accounts.SingleOrDefault(a => a.Id == UserId);
            }/* else {
                if(string.Equals(provider, "facebook", StringComparison.OrdinalIgnoreCase)) {
                    creds = await User.GetAppServiceIdentityAsync<FacebookCredentials>(Request);
                } else if(string.Equals(provider, "google", StringComparison.OrdinalIgnoreCase)) {
                    creds = await User.GetAppServiceIdentityAsync<GoogleCredentials>(Request);
                } else if(string.Equals(provider, "twitter", StringComparison.OrdinalIgnoreCase)) {
                    creds = await User.GetAppServiceIdentityAsync<TwitterCredentials>(Request);
                } else if(string.Equals(provider, "microsoft", StringComparison.OrdinalIgnoreCase)) {
                    creds = await User.GetAppServiceIdentityAsync<MicrosoftAccountCredentials>(Request);
                }
                UserId = string.Format("{0}:{1}", creds.Provider, creds.Claims[ClaimTypes.NameIdentifier]);
                account = context.Accounts.SingleOrDefault(a => a.Id == UserId);
            }*/

            if(account == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return account;
        }

        /*private string GetSiteUrl() {
            var settings = this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if(string.IsNullOrEmpty(settings.HostName)) {
                return "http://localhost";
            } else {
                return "https://" + settings.HostName + "/";
            }
        }*/
        /*
        private string GetSigningKey() {
            var settings = this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if(string.IsNullOrEmpty(settings.HostName)) {
                return ConfigurationManager.AppSettings["SigningKey"];
            } else {
                return Environment.GetEnvironmentVariable("WEBSITE_AUTH_SIGNING_KEY");
            }
        }*/
    }

    public static class IListAccountInfoExtention {
        public static List<ClientAccount> GetClientList(this IList<Account> me, RoboContext context) {

            return (from person in me
                    select new ClientAccount() {
                        Id = person.Id,
                        Username = person.Username,
                        RealName = person.RealName,
                        TeamNumber = person.TeamNumber
                    }).ToList();
        }
    }
}