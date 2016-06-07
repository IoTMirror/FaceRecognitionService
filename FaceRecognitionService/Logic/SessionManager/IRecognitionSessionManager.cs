using FaceRecognitionService.Models;

namespace FaceRecognitionService.Logic.SessionManager
{
    public interface IRecognitionSessionManager : ISessionManager<RecognitionSession>
    {
        int removeSessionByMirrorID(int mirrorID);
    }
}
