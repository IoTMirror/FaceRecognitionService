using Emgu.CV;
using Emgu.CV.CvEnum;
using FaceRecognitionService.Logic;
using FaceRecognitionService.Logic.SessionManager;
using FaceRecognitionService.Logic.SessionManager.Postgres;
using FaceRecognitionService.Models;
using FRLib;
using System.Drawing;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace FaceRecognitionService.Controllers.Public
{
    [RoutePrefix("public/Recognizer")]
    public class RecognizerController : ApiController
    {
        // recognizes user on given logging session
        [Route("{sessionID:guid}")]
        public IHttpActionResult Put(string sessionID)
        {
            if (sessionID == null) return NotFound();
            IRecognitionSessionManager sessionsManager = new RecognitionSessionManager();
            RecognitionSession session = sessionsManager.getSession(sessionID);
            if (session == null) return NotFound();
            if (session.state == RecognitionSession.RecognitionState.POST_RECOGNITION) return Ok();
            if (HttpContext.Current.Request.Files.Count != 1) return BadRequest("Images Required: 1, Provided: " + HttpContext.Current.Request.Files.Count);

            Size imageSize = new Size(200, 200);
            var thresholdProp = WebConfigurationManager.AppSettings["threshold"];
            var minWidthProp = WebConfigurationManager.AppSettings["minWidth"];
            var minHeightProp = WebConfigurationManager.AppSettings["minHeight"];
            var compressionProp = WebConfigurationManager.AppSettings["compression"];
            var cascadeFile = WebConfigurationManager.AppSettings["cascadeFileName"];
            var facesDataDir = WebConfigurationManager.AppSettings["facesDataDirectoryName"];
            int imageWidth;
            if(int.TryParse(minWidthProp, out imageWidth))
            {
                imageSize.Width = imageWidth;
            }
            int imageHeight;
            if (int.TryParse(minHeightProp, out imageHeight))
            {
                imageSize.Height = imageHeight;
            }

            Mat image = new HttpPostedFileMatExtractor(HttpContext.Current.Request.Files[0], LoadImageType.Color).extract();

            CascadeClassifier cc = new CascadeClassifier(HttpContext.Current.Server.MapPath("~/App_Data/" + cascadeFile));
            CascadeDetector detector = new CascadeDetector(cc);
            detector.MinSize = imageSize;
            Mat face = detector.ExtractLargest(image);


            FaceImagePreprocessor imageProcessor = new FaceImagePreprocessor();
            imageProcessor.OutputImageSize = imageSize;
            face = imageProcessor.Process(face);

            if (face == null) return BadRequest("Invalid file type");

            LBPHUserFaceRecognizer recognizer = new LBPHUserFaceRecognizer(HttpContext.Current.Server.MapPath("~/App_Data/"+facesDataDir));
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
            session.recognizedUser=recognizer.Recognize(face);
            

            session.state = RecognitionSession.RecognitionState.POST_RECOGNITION;
            sessionsManager.saveSession(session);
            return Ok();
        }

    }
}
