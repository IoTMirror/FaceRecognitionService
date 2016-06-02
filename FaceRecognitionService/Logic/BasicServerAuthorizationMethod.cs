using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Results;

namespace FaceRecognitionService.Logic
{
    public class BasicServerAuthorizationMethod : IServerAuthorizationMethod
    {
        public UnauthorizedResult authorizeServer(ApiController controller)
        {
            var key = WebConfigurationManager.AppSettings["SERVERS_SECRET_KEY"] ?? "";
            AuthenticationHeaderValue auth = null;
            AuthenticationHeaderValue.TryParse(HttpContext.Current.Request.Headers.Get("Authorization"), out auth);
            if (auth == null || !"basic".Equals(auth.Scheme.ToLower()) || !key.Equals(auth.Parameter))
            {
                return new UnauthorizedResult(
                    new AuthenticationHeaderValue[] { new AuthenticationHeaderValue("Basic", "realm=\"Server communication\"") },
                    controller);
            }
            return null;
        }
    }
}