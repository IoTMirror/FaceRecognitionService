using System;
using System.Collections.Generic;
using FaceRecognitionService.Models;

namespace FaceRecognitionService.Logic.SessionManager.Simple
{
    public class RecognitionSessionManager : IRecognitionSessionManager
    {
        private static RecognitionSessionManager instance;
        private static Object instanceLock = new Object();

        private Object sessionsLock = new Object();
        private List<RecognitionSession> sessions = new List<RecognitionSession>();

        public RecognitionSession createSession()
        {
            RecognitionSession session = new RecognitionSession();
            session.sessionID = Guid.NewGuid().ToString();            
            return session;
        }

        public int removeSession(string sessionID)
        {
            if (sessionID == null) return 0;
            lock(sessions)
            {
                return sessions.RemoveAll((RecognitionSession session) => sessionID.Equals(session.sessionID));
            }
        }

        public RecognitionSession getSession(string sessionID)
        {
            if (sessionID == null) return null;
            lock(sessions)
            {
                return sessions.Find((RecognitionSession session) => sessionID.Equals(session.sessionID));
            }
        }

        public bool saveSession(RecognitionSession session)
        {
            lock (sessions)
            {
                var old = sessions.Find((RecognitionSession s) => session.sessionID.Equals(s.sessionID));
                if (old != null)
                {
                    old.mirrorID = session.mirrorID;
                    old.recognizedUser = session.recognizedUser;
                    old.state = session.state;
                }
                else sessions.Add(session);
            }
            return true;
        }

        public static RecognitionSessionManager getManager()
        {
            if (instance == null)
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RecognitionSessionManager();
                    }
                }
            }
            return instance;
        }
    }
}