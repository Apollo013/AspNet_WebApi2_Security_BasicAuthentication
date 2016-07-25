using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using WebApi2_Owin_BasicAuthentication.AuthenticationServer.Authenticators;
using WebApi2_Owin_BasicAuthentication.Models;

namespace WebApi2_Owin_BasicAuthentication.Core.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        [BasicIdentityAthentication(Realm = "Hello")]
        [Authorize]
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            UserVM user = new UserVM
            {
                UserName = User.Identity.Name
            };

            ClaimsIdentity identity = User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                List<ClaimVM> claims = new List<ClaimVM>();

                foreach (Claim claim in identity.Claims)
                {
                    claims.Add(new ClaimVM
                    {
                        Type = claim.Type,
                        Value = claim.Value
                    });
                }

                user.Claims = claims;
            }

            return Ok(user);
        }
    }
}
