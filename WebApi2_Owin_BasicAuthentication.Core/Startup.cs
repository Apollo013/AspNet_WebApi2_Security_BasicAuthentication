using Microsoft.Owin;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

[assembly: OwinStartup(typeof(WebApi2_Owin_BasicAuthentication.Core.Startup))]

namespace WebApi2_Owin_BasicAuthentication.Core
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            RegisterMessageFormat(config);

            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
        }

        private static void RegisterMessageFormat(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
