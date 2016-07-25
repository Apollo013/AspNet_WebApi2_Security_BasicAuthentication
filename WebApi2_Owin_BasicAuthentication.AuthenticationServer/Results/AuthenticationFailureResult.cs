using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi2_Owin_BasicAuthentication.AuthenticationServer.Results
{
    /// <summary>
    /// Responsible for generating an error result if authentication fails
    /// </summary>
    public class AuthenticationFailureResult : IHttpActionResult
    {
        private HttpRequestMessage request;
        private string message;
        private string reasonPhrase;

        public AuthenticationFailureResult(HttpRequestMessage request, string message, string reasonPhrase)
        {
            this.request = request;
            this.message = message;
            this.reasonPhrase = reasonPhrase;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
            {
                RequestMessage = request,
                Content = new StringContent(message),
                ReasonPhrase = reasonPhrase
            };
        }
    }
}
