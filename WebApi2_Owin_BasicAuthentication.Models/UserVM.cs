using System.Collections.Generic;

namespace WebApi2_Owin_BasicAuthentication.Models
{
    public class UserVM
    {
        public string UserName { get; set; }

        public IEnumerable<ClaimVM> Claims { get; set; }
    }
}
