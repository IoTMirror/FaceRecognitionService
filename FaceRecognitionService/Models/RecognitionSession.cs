using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceRecognitionService.Models
{
    public class RecognitionSession
    {
        public enum RecognitionState
        {
            PRE_RECOGNITION=0,
            POST_RECOGNITION=1
        };

        public string sessionID { get; set; }
        public RecognitionState state { get; set; } = RecognitionState.PRE_RECOGNITION;
        public int mirrorID { get; set; }
        public int recognizedUser { get; set; } = -1;
    }
}