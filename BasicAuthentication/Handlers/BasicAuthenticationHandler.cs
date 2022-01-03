using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BasicAuthentication.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }


        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string username = string.Empty;
            try
            {
                string auth = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(auth);
                if (string.IsNullOrWhiteSpace(authHeader?.Parameter))
                {
                    throw new Exception("Creadential not supplied.");
                }
                var decryptedTokenString = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(":");
                username = decryptedTokenString.FirstOrDefault();
                string pass = decryptedTokenString.LastOrDefault();

                // This shoud probably move to some service.
                if (username.ToLower() != "raj" || pass.ToLower() != "pass")
                {
                    throw new Exception("Invalid Credentials.");
                }

            }
            catch (Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail(ex));
            }

            List<Claim> claims = new List<Claim>()
            { 
                new Claim("UserName", username)
            };
            var claimIdentity = new ClaimsIdentity(claims, Scheme.Name);
            var claimPrinciple = new ClaimsPrincipal(claimIdentity);
            var authTicket = new AuthenticationTicket(claimPrinciple, null, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(authTicket));
        }
    }
}
