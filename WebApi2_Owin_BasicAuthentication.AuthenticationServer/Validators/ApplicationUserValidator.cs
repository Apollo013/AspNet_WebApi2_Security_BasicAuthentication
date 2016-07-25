using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi2_Owin_BasicAuthentication.AuthenticationServer.Managers;
using WebApi2_Owin_BasicAuthentication.AuthenticationServer.Models;

namespace WebApi2_Owin_BasicAuthentication.AuthenticationServer.Validators
{
    public class ApplicationUserValidator : UserValidator<ApplicationUser>
    {
        // Restrict domain names to the following
        List<string> whiteList = new List<string> { "outlook.ie", "outlook.com", "hotmail.com", "gmail.com", "yahoo.com" };

        public ApplicationUserValidator(ApplicationUserManager manager) : base(manager)
        {
            AllowOnlyAlphanumericUserNames = true;
            RequireUniqueEmail = true;
        }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);

            var emailDomain = user.Email.Split('@')[1];

            if (!whiteList.Contains(emailDomain.ToLower()))
            {
                var errors = result.Errors.ToList();

                errors.Add($"Email domain '{emailDomain}' is not allowed");

                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}
