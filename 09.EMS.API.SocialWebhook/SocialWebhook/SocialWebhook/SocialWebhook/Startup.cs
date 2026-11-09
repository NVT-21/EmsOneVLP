using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(SocialWebhook.Startup), "Configuration")]

namespace SocialWebhook
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                map.RunSignalR();
                //map.UseCors(corsOption).RunSignalR();
            });

            /**/
            //var httpConfig = new HttpConfiguration();

            ////change this configuration as you want.
            //var cors = new EnableCorsAttribute("https://localhost:44304/", "*", "*");
            //httpConfig.EnableCors(cors);

            //SignalRConfig.Register(app, cors);

            //WebApiConfig.Register(httpConfig);

            //app.UseWebApi(httpConfig);

            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            /**/

            //app.MapSignalR();
            //app.Map("/signalr", map =>
            //{
            //    // Setup the CORS middleware to run before SignalR.
            //    // By default this will allow all origins. You can 
            //    // configure the set of origins and/or http verbs by
            //    // providing a cors options with a different policy.

            //    var hubConfiguration = new HubConfiguration
            //    {
            //        // You can enable JSONP by uncommenting line below.
            //        // JSONP requests are insecure but some older browsers (and some
            //        // versions of IE) require JSONP to work cross domain
            //        EnableJSONP = true,
            //        EnableDetailedErrors = true
            //    };
            //    // Run the SignalR pipeline. We're not using MapSignalR
            //    // since this branch already runs under the "/signalr"
            //    // path.
            //    map.RunSignalR(hubConfiguration);
            //});
            //var hubConfiguration = new HubConfiguration();
            //hubConfiguration.EnableDetailedErrors = true;
            //hubConfiguration.EnableJavaScriptProxies = false;
            //app.MapSignalR("/signalr", hubConfiguration);
        }
    }
}