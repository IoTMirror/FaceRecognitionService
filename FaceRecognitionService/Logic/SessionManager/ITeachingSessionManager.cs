using FaceRecognitionService.Models;

namespace FaceRecognitionService.Logic.SessionManager
{
    public interface ITeachingSessionManager : ISessionManager<TeachingSession>
    {
        TeachingSession getSessionByMirrorID(int mirrorID);
        int removeSessionByMirrorID(int mirrorID);
    }
}
