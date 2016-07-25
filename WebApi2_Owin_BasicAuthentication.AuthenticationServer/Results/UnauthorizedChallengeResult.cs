using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi2_Owin_BasicAuthentication.AuthenticationServer.Results
{
    /// <summary>
    /// Responsible for generating a challenge result when authorization fails
    /// </summary>
    public class UnauthorizedChallengeResult : IHttpActionResult
    {
        private AuthenticationHeaderValue challenge;
        private IHttpActionResult innerResult;

        public UnauthorizedChallengeResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
        {
            this.challenge = challenge;
            this.innerResult = innerResult;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await innerResult.ExecuteAsync(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Only add one challenge per authentication scheme.
                if (!response.Headers.WwwAuthenticate.Any((h) => h.Scheme == challenge.Scheme))
                {
                    response.Headers.WwwAuthenticate.Add(challenge);
                }
            }

            return response;
        }

    }
}
