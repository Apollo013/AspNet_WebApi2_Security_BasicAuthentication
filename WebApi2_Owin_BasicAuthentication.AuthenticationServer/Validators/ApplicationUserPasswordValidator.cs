using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi2_Owin_BasicAuthentication.AuthenticationServer.Validators
{
    public class ApplicationUserPasswordValidator : PasswordValidator
    {
        // Do not allow the following passwords
        List<string> blackList = new List<string> { "1234", "12345", "123456", "password", "password1" };

        public ApplicationUserPasswordValidator()
        {
            RequiredLength = 6;
            RequireNonLetterOrDigit = true;
            RequireDigit = false;
            RequireLowercase = true;
            RequireUppercase = true;
        }

        public override async Task<IdentityResult> ValidateAsync(string password)
        {
            IdentityResult result = await base.ValidateAsync(password);

            if (blackList.Contains(password.ToLower()))
            {
                var errors = result.Errors.ToList();
                errors.Add("Password can not contain sequence of chars");
                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}
