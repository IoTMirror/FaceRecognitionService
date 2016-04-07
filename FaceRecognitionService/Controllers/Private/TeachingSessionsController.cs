﻿using System.Web.Http;
using FaceRecognitionService.Models;
using FaceRecognitionService.Logic.SessionManager.Postgres;
using FaceRecognitionService.Logic.SessionManager;

namespace FaceRecognitionService.Controllers.Private
{
    [RoutePrefix("private/TeachingSessions")]
    public class TeachingSessionsController : ApiController
    {

        [Route("{sessionID:guid}", Name ="TeachingSession")]
        public IHttpActionResult Get(string sessionID)
        {
            ITeachingSessionManager sessionsManager = new TeachingSessionManager();
            TeachingSession session = sessionsManager.getSession(sessionID);
            if (session == null) return NotFound();
            else return Ok(session);
        }

        // creates teaching session for user
        [Route("{userID:int}")]
        public IHttpActionResult Post(int userID)
        {
            ITeachingSessionManager sessionsManager = new TeachingSessionManager();
            var teachingSession = sessionsManager.createSession();
            teachingSession.userID = userID;
            sessionsManager.saveSession(teachingSession);
            return Created(Url.Route("TeachingSession", new { sessionID = teachingSession.sessionID }), teachingSession);
        }

        [Route("{sessionID:guid}")]
        public IHttpActionResult Delete(string sessionID)
        {
            ITeachingSessionManager sessionsManager = new TeachingSessionManager();
            if (sessionsManager.removeSession(sessionID) > 0) return Ok();
            else return NotFound();
        }
    }
}