using System.Web;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using System.IO;

namespace FaceRecognitionService.Logic
{
    public class HttpPostedFileMatExtractor : MatExtractor
    {
        private HttpPostedFile file;
        private LoadImageType type;

        public HttpPostedFileMatExtractor(HttpPostedFile file, LoadImageType type)
        {
            this.file = file;
            this.type = type;
        }

        public Mat extract()
        {
            var reader = new BinaryReader(file.InputStream);
            Mat image = new Mat();
            try
            {
                CvInvoke.Imdecode(reader.ReadBytes(file.ContentLength), type, image);
            }
            catch(CvException)
            {
                image = null;
            }
            file.InputStream.Position = 0;
            return image;
        }
    }
}