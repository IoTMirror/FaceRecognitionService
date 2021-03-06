﻿using System;
using FaceRecognitionService.Models;
using System.Web.Configuration;
using Npgsql;

namespace FaceRecognitionService.Logic.SessionManager.Postgres
{
    public class TeachingSessionManager : ITeachingSessionManager
    {

        public TeachingSession createSession(TeachingSession session)
        {
            var connection = createConnection();
            connection.Open();
            int rows;
            session.sessionID = Guid.NewGuid().ToString();
            removeSessionByMirrorID(session.mirrorID);
            NpgsqlCommand command = new NpgsqlCommand(string.Format("INSERT INTO TeachingSessions (session_id,mirror_id,user_id) VALUES ('{0}','{1}','{2}');",
            session.sessionID, session.mirrorID, session.userID), connection);
            rows = command.ExecuteNonQuery();
            connection.Close();
            if (rows == 0) return null;
            return session;
        }

        public TeachingSession getSession(string sessionID)
        {
            var connection = createConnection();
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(string.Format("SELECT session_id,mirror_id, user_id FROM TeachingSessions WHERE session_id='{0}';",
            sessionID), connection);
            TeachingSession session=null;
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                session = new TeachingSession();
                session.sessionID = reader["session_id"].ToString();
                session.mirrorID = int.Parse(reader["mirror_id"].ToString());
                session.userID = int.Parse(reader["user_id"].ToString());
            }
            connection.Close();
            return session;
        }

        public TeachingSession getSessionByMirrorID(int mirrorID)
        {
            var connection = createConnection();
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(string.Format("SELECT session_id,mirror_id, user_id FROM TeachingSessions WHERE mirror_id='{0}';",
            mirrorID), connection);
            TeachingSession session = null;
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                session = new TeachingSession();
                session.sessionID = reader["session_id"].ToString();
                session.mirrorID = int.Parse(reader["mirror_id"].ToString());
                session.userID = int.Parse(reader["user_id"].ToString());
            }
            connection.Close();
            return session;
        }

        public int removeSession(string sessionID)
        {
            var connection = createConnection();
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(string.Format("DELETE FROM TeachingSessions WHERE session_id='{0}';",
            sessionID), connection);
            int rows = command.ExecuteNonQuery();
            connection.Close();
            return rows;
        }

        public int removeSessionByMirrorID(int mirrorID)
        {
            var connection = createConnection();
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(string.Format("DELETE FROM TeachingSessions WHERE mirror_id='{0}';",
            mirrorID), connection);
            int rows = command.ExecuteNonQuery();
            connection.Close();
            return rows;
        }

        public bool saveSession(TeachingSession session)
        {
            var connection = createConnection();
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(string.Format("UPDATE TeachingSessions SET session_id='{0}', mirror_id='{1}', user_id='{2}' WHERE session_id='{0}';",
            session.sessionID, session.mirrorID, session.userID), connection);
            int rows = command.ExecuteNonQuery();
            connection.Close();
            if (rows != 0) return true;
            else return false;
        }

        public NpgsqlConnection createConnection()
        {
            string host = WebConfigurationManager.AppSettings["DB_SERVER"] ?? "";
            string db = WebConfigurationManager.AppSettings["DB_NAME"] ?? "";
            string user = WebConfigurationManager.AppSettings["DB_USER"] ?? "";
            string pass = WebConfigurationManager.AppSettings["DB_PASS"] ?? "";
            string port = WebConfigurationManager.AppSettings["DB_PORT"] ?? "";
            bool ssl = false;
            bool.TryParse(WebConfigurationManager.AppSettings["DB_SSL"] ?? "", out ssl);
            var connectionString = string.Format("Server={0};Database={1};User Id={2};Password={3};Port={4}" + (ssl?";SSL Mode=Require; Use SSL Stream=true":""),
            host, db, user, pass, port);
            var con = new NpgsqlConnection(connectionString);
            if (ssl) con.UserCertificateValidationCallback = delegate { return true; };
            return con;
        }
    }
}