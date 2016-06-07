using System.Web.Http;
using FaceRecognitionService.Models;
using FaceRecognitionService.Logic.SessionManager.Postgres;
using FaceRecognitionService.Logic.SessionManager;
using FaceRecognitionService.Logic;

namespace FaceRecognitionService.Controllers.Private
{
    [RoutePrefix("private/TeachingSessions")]
    public class TeachingSessionsController : ApiController
    {

        [Route("{sessionID:guid}", Name ="TeachingSession")]
        public IHttpActionResult Get(string sessionID)
        {
            var authorizationResult = (new BasicServerAuthorizationMethod()).authorizeServer(this);
            if (authorizationResult != null) return authorizationResult;
            ITeachingSessionManager sessionsManager = new TeachingSessionManager();
            TeachingSession session = sessionsManager.getSession(sessionID);
            if (session == null) return NotFound();
            else return Ok(session);
        }

        // creates teaching session for mirror and user
        [Route("mirrors/{mirrorID:int}/users/{userID:int}")]
        public IHttpActionResult Post(int mirrorID, int userID)
        {
            var authorizationResult = (new BasicServerAuthorizationMethod()).authorizeServer(this);
            if (authorizationResult != null) return authorizationResult;
            ITeachingSessionManager sessionsManager = new TeachingSessionManager();
            var teachingSession = new TeachingSession();
            teachingSession.userID = userID;
            teachingSession.mirrorID = mirrorID;
            teachingSession = sessionsManager.createSession(teachingSession);
            return Created(Url.Route("TeachingSession", new { sessionID = teachingSession.sessionID }), teachingSession);
        }

        [Route("{sessionID:guid}")]
        public IHttpActionResult Delete(string sessionID)
        {
            var authorizationResult = (new BasicServerAuthorizationMethod()).authorizeServer(this);
            if (authorizationResult != null) return authorizationResult;
            ITeachingSessionManager sessionsManager = new TeachingSessionManager();
            sessionsManager.removeSession(sessionID);
            return Ok();
        }
    }
}