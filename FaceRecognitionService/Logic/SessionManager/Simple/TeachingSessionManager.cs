using System;
using System.Collections.Generic;
using FaceRecognitionService.Models;

namespace FaceRecognitionService.Logic.SessionManager.Simple
{
    public class TeachingSessionManager : ITeachingSessionManager
    {
        private static ITeachingSessionManager instance;
        private static Object instanceLock = new Object();

        private Object sessionsLock = new Object();
        private List<TeachingSession> sessions = new List<TeachingSession>();

        public int removeSession(string sessionID)
        {
            if (sessionID == null) return 0;
            lock (sessions)
            {
                return sessions.RemoveAll((TeachingSession session) => sessionID.Equals(session.sessionID));
            }
        }

        public TeachingSession getSession(string sessionID)
        {
            if (sessionID == null) return null;
            lock(sessions)
            {
                return sessions.Find((TeachingSession session) => sessionID.Equals(session.sessionID));
            }
        }

        public TeachingSession createSession()
        {
            TeachingSession session = new TeachingSession();
            session.sessionID = Guid.NewGuid().ToString();
            return session;
        }

        public bool saveSession(TeachingSession session)
        {
            lock (sessions)
            {
                var old = sessions.Find((TeachingSession s) => session.sessionID.Equals(s.sessionID));
                if(old!=null)
                {
                    old.userID = session.userID;
                }
                else sessions.Add(session);
            }
            return true;
        }

        public static ITeachingSessionManager getManager()
        {
            if (instance == null)
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TeachingSessionManager();
                    }
                }
            }
            return instance;
        }
    }
}