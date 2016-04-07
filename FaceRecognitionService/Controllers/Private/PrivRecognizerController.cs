using System.Web;
using System.Web.Http;
using FRLib;
using System.Web.Configuration;

namespace FaceRecognitionService.Controllers.Private
{
    [RoutePrefix("private/Recognizer")]
    public class PrivRecognizerController : ApiController
    {
        //deletes face recognition data related to the user specified by id
        [Route("{userID:int}")]
        public IHttpActionResult Delete(int userID)
        {
            var facesDataDir = WebConfigurationManager.AppSettings["facesDataDirectoryName"];
            LBPHUserFaceRecognizer recognizer = new LBPHUserFaceRecognizer(HttpContext.Current.Server.MapPath("~/App_Data/" + facesDataDir));
            recognizer.removeUserData(userID);
            return Ok();
        }
    }
}
