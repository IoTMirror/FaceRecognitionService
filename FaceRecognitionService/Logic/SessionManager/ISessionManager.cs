namespace FaceRecognitionService.Logic.SessionManager
{
    public interface ISessionManager<SessionType>
    {
        SessionType createSession();
        bool saveSession(SessionType session);
        SessionType getSession(string sessionID);
        int removeSession(string sessionID);
    }
}
