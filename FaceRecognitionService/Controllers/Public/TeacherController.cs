using Emgu.CV;
using Emgu.CV.CvEnum;
using FaceRecognitionService.Logic;
using FaceRecognitionService.Models;
using System.Web;
using System.Web.Http;
using FRLib;
using System.Drawing;
using FaceRecognitionService.Logic.SessionManager.Postgres;
using FaceRecognitionService.Logic.SessionManager;
using System.Web.Configuration;

namespace FaceRecognitionService.Controllers.Public
{
    [RoutePrefix("public/Teacher")]
    public class TeacherController : ApiController
    {

        // teaches user's recognizer with given images, removes session
        [Route("{sessionID:guid}")]
        public IHttpActionResult Put(string sessionID)
        {
            if (sessionID == null) return NotFound();
            ITeachingSessionManager sessionsManager = new TeachingSessionManager();
            TeachingSession session = sessionsManager.getSession(sessionID);
            if (session == null) return NotFound();
            if (HttpContext.Current.Request.Files.Count == 0) return BadRequest("Images Required: 1+, Provided: " + HttpContext.Current.Request.Files.Count);

            Size imageSize = new Size(200, 200);
            var thresholdProp = WebConfigurationManager.AppSettings["threshold"];
            var minWidthProp = WebConfigurationManager.AppSettings["minWidth"];
            var minHeightProp = WebConfigurationManager.AppSettings["minHeight"];
            var compressionProp = WebConfigurationManager.AppSettings["compression"];
            var cascadeFile = WebConfigurationManager.AppSettings["cascadeFileName"];
            var facesDataDir = WebConfigurationManager.AppSettings["facesDataDirectoryName"];
            int imageWidth;
            if (int.TryParse(minWidthProp, out imageWidth))
            {
                imageSize.Width = imageWidth;
            }
            int imageHeight;
            if (int.TryParse(minHeightProp, out imageHeight))
            {
                imageSize.Height = imageHeight;
            }

            for (int i=0;i< HttpContext.Current.Request.Files.Count;++i)
            {
                Mat image = new HttpPostedFileMatExtractor(HttpContext.Current.Request.Files[i],LoadImageType.Color).extract();

                CascadeClassifier cc = new CascadeClassifier(HttpContext.Current.Server.MapPath("~/App_Data/" + cascadeFile));
                CascadeDetector detector = new CascadeDetector(cc);
                detector.MinSize = imageSize;
                Mat face = detector.ExtractLargest(image);

                FaceImagePreprocessor imageProcessor = new FaceImagePreprocessor();
                imageProcessor.OutputImageSize = imageSize;
                face = imageProcessor.Process(face);

                LBPHUserFaceRecognizer recognizer = new LBPHUserFaceRecognizer(HttpContext.Current.Server.MapPath("~/App_Data/" + facesDataDir));
                int threshold;
                if (int.TryParse(thresholdProp, out threshold))
                {
                    recognizer.Threshold = threshold;
                }
                bool compression;
                if (bool.TryParse(compressionProp, out compression))
                {
                    recognizer.Compression = compression;
                }
                recognizer.Train(session.userID,face);
            }
            sessionsManager.removeSession(sessionID);
            return Ok();
        }

        [Route("mirrors/{mirrorID:int}")]
        public IHttpActionResult Get(int mirrorID)
        {
            ITeachingSessionManager sessionsManager = new TeachingSessionManager();
            TeachingSession session = sessionsManager.getSessionByMirrorID(mirrorID);
            if (session == null) return NotFound();
            else return Ok(session.sessionID);
        }
    }
}