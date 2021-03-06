﻿using System.Web.Http;
using FaceRecognitionService.Models;
using FaceRecognitionService.Logic.SessionManager.Postgres;
using FaceRecognitionService.Logic.SessionManager;
using FaceRecognitionService.Logic;

namespace FaceRecognitionService.Controllers.Private
{
    [RoutePrefix("private/RecognitionSessions")]
    public class RecognitionSessionsController : ApiController
    {
        [Route("{sessionID:guid}", Name="RecognitionSession")]
        //returns session state and recognized user, if it is the last step removes session
        public IHttpActionResult Get(string sessionID)
        {
            var authorizationResult = (new BasicServerAuthorizationMethod()).authorizeServer(this);
            if (authorizationResult != null) return authorizationResult;
            IRecognitionSessionManager sessionsManager = new RecognitionSessionManager();
            RecognitionSession session = sessionsManager.getSession(sessionID);
            if (session == null)
            {
                return NotFound();
            }
            else
            {
                if (session.state == RecognitionSession.RecognitionState.POST_RECOGNITION)
                {
                    sessionsManager.removeSession(sessionID);
                }
                return Ok(session);
            }
        }

        [Route("{mirrorID:int}")]
        // creates recognition session for mirror
        public IHttpActionResult Post(int mirrorID)
        {
            var authorizationResult = (new BasicServerAuthorizationMethod()).authorizeServer(this);
            if (authorizationResult != null) return authorizationResult;
            IRecognitionSessionManager sessionsManager = new RecognitionSessionManager();
            var recognitionSession = new RecognitionSession();
            recognitionSession.mirrorID = mirrorID;
            recognitionSession = sessionsManager.createSession(recognitionSession);
            recognitionSession.mirrorID = mirrorID;
            sessionsManager.saveSession(recognitionSession);
            return Created(Url.Route("RecognitionSession",new { sessionID = recognitionSession.sessionID}), recognitionSession);
        }

        [Route("{sessionID:guid}")]
        // removes session
        public IHttpActionResult Delete(string sessionID)
        {
            var authorizationResult = (new BasicServerAuthorizationMethod()).authorizeServer(this);
            if (authorizationResult != null) return authorizationResult;
            IRecognitionSessionManager sessionsManager = new RecognitionSessionManager();
            sessionsManager.removeSession(sessionID);
            return Ok();
        }
    }
}
