using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace FaceRecognitionService.Logic
{
    interface IServerAuthorizationMethod
    {
        UnauthorizedResult authorizeServer(ApiController controller);
    }
}
