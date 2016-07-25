using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using WebApi2_Owin_BasicAuthentication.AuthenticationServer.Helpers;
using WebApi2_Owin_BasicAuthentication.AuthenticationServer.Results;

namespace WebApi2_Owin_BasicAuthentication.AuthenticationServer.Authenticators
{
    public abstract class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        #region PROPERTIES
        public string Realm { get; set; }

        public virtual bool AllowMultiple { get { return false; } }
        #endregion

        #region AUTHENTICATION METHODS
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // 1. Look for credentials in the request.
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            // 2. If there are no credentials, do nothing.
            if (authorization == null)
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
                return;
            }

            // 3. If there are credentials but the filter does not recognize the authentication scheme, do nothing.
            if (authorization.Scheme != "Basic")
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
                return;
            }

            // 4. If there are credentials that the filter understands, try to validate them.
            // 5. If the credentials are bad, set the error result.
            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult(request, "Credentials not supplied", "Missing credentials");
                return;
            }

            Tuple<string, string> userNameAndPasword = UsernameAndPasswordHelper.Extract(authorization.Parameter);

            if (userNameAndPasword == null)
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult(request, "Credentials not valid", "Invalid credentials");
            }

            string userName = userNameAndPasword.Item1;
            string password = userNameAndPasword.Item2;

            IPrincipal principal = await AuthenticateAsync(userName, password, cancellationToken);

            if (principal == null)
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult(request, "Username or password not valid", "Invalid username or password");
            }

            // 6. If the credentials are valid, set principal.
            else
            {
                // Authentication was attempted and succeeded. Set Principal to the authenticated user.
                context.Principal = principal;
            }

        }

        /// <summary>
        /// Abstract method that allows us to implement any custom authentication logic 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken);

        #endregion

        #region CHALLENGE METHODS
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter;

            if (String.IsNullOrEmpty(Realm))
            {
                parameter = null;
            }
            else
            {
                // A correct implementation should verify that Realm does not contain a quote character unless properly
                // escaped (precededed by a backslash that is not itself escaped).
                parameter = "realm=\"" + Realm + "\"";
            }

            context.ChallengeWith("Basic", parameter);
        }
        #endregion
    }
}
