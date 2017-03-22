using System.Linq;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RobotServer.Models;
using Microsoft.Extensions.Logging;
using RobotServer.Interfaces;
using RobotServer.SQLDataObjects;
using RobotServer.ClientData;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using NUglify.Helpers;
using Microsoft.AspNetCore.Identity;
using IdentityModel.Client;
using RobotServer;

namespace ScoutingServer.Controllers
{

    [Route("api/[Controller]")]
    public class AccountController : Controller {
        public const int MIN_PASSWORD_LENGTH = 8;
        public const int MAX_PASSWORD_LENGTH = 128;
        private readonly RoboContext context;
        private readonly UserManager<Account> UserMan;
        private readonly ILogger logger;
        //private readonly SignInManager<Account> SignMan;

        public AccountController(/*SignInManager<Account> sim, */UserManager<Account> um, ILoggerFactory loggerFactory, RoboContext context) {
            this.context = context;
            logger = loggerFactory.CreateLogger("Account");
            UserMan = um;
            //SignMan = sim;
        }

        [Route("CustomRegister")]
        [ActionName("CustomRegister")]
        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage CustomRegister([FromBody]CustomRegistrationRequest request) {
            string error = null;
            error = RegisterCheck(request.Username, request.Password);
            if(error.IsNullOrWhiteSpace()) {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            } else {
                string guid = Guid.NewGuid().ToString();

                Account newAccount = new Account() {
                    UserName = request.Username
                };

                UserMan.CreateAsync(newAccount, request.Password).Wait();

                return new HttpResponseMessage(HttpStatusCode.Created);
            }
        }
        /*
        [Route("Register")]
        [ActionName("Register")]
        [Authorize]
        [HttpPost]
        public async Task<HttpResponseMessage> Register(RegistrationRequest request) {
            string error = RegisterCheck(request.UserName, null);
            if(error != null) {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            } else {
                ClaimsPrincipal principal = this.User as ClaimsPrincipal;
                string provider = principal.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider").Value;

                SignIn();
                    
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
                        UserName = request.UserName,
                    };
                    context.Accounts.Add(newAccount);
                    context.SaveChanges();

                    return new HttpResponseMessage(HttpStatusCode.Created);
                } else {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
        }
        */
        public string RegisterCheck(string UserName, string password) {
            if(UserName.Length >= 4 && UserName.Length <= 16 && !Regex.IsMatch(UserName, "^[a-zA-Z0-9]{4,}$")) {
                return "Invalid UserName (at least 4 chars and less than 16, alphanumeric only)";
            } else if(password != null && password.Length <= MIN_PASSWORD_LENGTH && password.Length >= MAX_PASSWORD_LENGTH) {
                return "Invalid password (at least 8 and less than 128 chars required)";
            }

            Account account = context.Accounts.FirstOrDefault(a => a.UserName == UserName);
            if(account != null) {
                if(!string.IsNullOrWhiteSpace(account.UserName) && account.UserName == UserName) {
                    return "That UserName already exists.";
                }
                //TODO fix the commented code.
            }
            return null;
        }

        [Route("CustomLogin")]
        [ActionName("CustomLogin")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequest request) {
            if (ModelState.IsValid)
            {
                var disco = await DiscoveryClient.GetAsync(Config.HOST);
                var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", Config.SECRET);
                var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(request.Username, request.Password,
                    "api");

                if (tokenResponse.IsError)
                {
                    logger.LogError(tokenResponse.Error);
                    return BadRequest("Invalid User/Pass");
                }

                return Json(tokenResponse.Json);
            }

            return BadRequest("Invalid model");
        }

        [Route("Logout")]
        [ActionName("Logout")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Logout(LoginRequest request) {
            return SignOut();
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
        public ClientAccount GetAccount(string UserName) {
            return context.Accounts.FirstOrDefault(a => a.UserName == UserName).GetClientAccount();
        }

        [Authorize]
        [Route("GetAccountInfos")]
        [ActionName("GetAccountInfos")]
        [HttpPost]
        public List<ClientAccount> GetAccount(IList<string> id) {
            foreach(var i in id) {
            }
            var results = context.Accounts.Where(a => id.Contains(a.UserName)).ToList().GetClientList(context);
            return results;
        }

        [Authorize]
        [Route("UserNameExists")]
        [ActionName("UserNameExists")]
        [HttpPost]
        public HttpResponseMessage UserNameExists(string un) {
            var thing = context.Accounts.FirstOrDefault(a => a.UserName == un);
            if(thing != null && thing.UserName != null) {
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
                account = context.Accounts.SingleOrDefault(a => a.UserName == UserId);
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
                        Username = person.UserName,
                        RealName = person.RealName,
                        TeamNumber = person.TeamNumber
                    }).ToList();
        }
    }
}