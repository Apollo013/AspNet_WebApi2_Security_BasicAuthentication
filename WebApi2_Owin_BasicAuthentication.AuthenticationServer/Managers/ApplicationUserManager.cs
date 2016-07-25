using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApi2_Owin_BasicAuthentication.AuthenticationServer.DataAccesLayer;
using WebApi2_Owin_BasicAuthentication.AuthenticationServer.Models;
using WebApi2_Owin_BasicAuthentication.AuthenticationServer.Validators;

namespace WebApi2_Owin_BasicAuthentication.AuthenticationServer.Managers
{
    /// <summary>
    /// Responsible for managing instances of the 'ApplicationUser' class
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store) { }

        /// <summary>
        /// Where Identity data is accessed, this will help us to hide the details of how IdentityUser is stored throughout the application.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ApplicationUserManager Create()
        {
            var context = new ApplicationUserContext();

            var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            appUserManager.PasswordValidator = new ApplicationUserPasswordValidator();

            appUserManager.UserValidator = new ApplicationUserValidator(appUserManager);

            return appUserManager;
        }
    }
}
