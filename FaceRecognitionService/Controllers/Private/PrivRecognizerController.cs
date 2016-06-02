using System.Web;
using System.Web.Http;
using FRLib;
using System.Web.Configuration;
using FaceRecognitionService.Logic;

namespace FaceRecognitionService.Controllers.Private
{
    [RoutePrefix("private/Recognizer")]
    public class PrivRecognizerController : ApiController
    {
        //deletes face recognition data related to the user specified by id
        [Route("{userID:int}")]
        public IHttpActionResult Delete(int userID)
        {
            var authorizationResult = (new BasicServerAuthorizationMethod()).authorizeServer(this);
            if (authorizationResult != null) return authorizationResult;
            var facesDataDir = WebConfigurationManager.AppSettings["facesDataDirectoryName"];
            LBPHUserFaceRecognizer recognizer = new LBPHUserFaceRecognizer(HttpContext.Current.Server.MapPath("~/App_Data/" + facesDataDir));
            recognizer.RemoveUserData(userID);
            return Ok();
        }
    }
}
