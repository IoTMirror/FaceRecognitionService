using FaceRecognitionService.Logic;
using FaceRecognitionService.Logic.UsersManager;
using FaceRecognitionService.Logic.UsersManager.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FaceRecognitionService.Controllers.Private
{
    [RoutePrefix("private/Users")]
    public class UsersController : ApiController
    {
        //returns user's mirrors
        [Route("{userID:int}/mirrors")]
        public IHttpActionResult Get(int userID)
        {
            var authorizationResult = (new BasicServerAuthorizationMethod()).authorizeServer(this);
            if (authorizationResult != null) return authorizationResult;
            IUserMirrorsManager um = new UserMirrorsManager();
            return Ok(um.getUsersMirrors(userID));
        }

        //adds mirror to user's mirrors
        [Route("{userID:int}/mirrors/{mirrorID:int}")]
        public IHttpActionResult Put(int userID, int mirrorID)
        {
            var authorizationResult = (new BasicServerAuthorizationMethod()).authorizeServer(this);
            if (authorizationResult != null) return authorizationResult;
            IUserMirrorsManager um = new UserMirrorsManager();
            um.addUsersMirror(userID, mirrorID);
            return Ok();
        }

        //deletes mirror from user's mirrors
        [Route("{userID:int}/mirrors/{mirrorID:int}")]
        public IHttpActionResult Delete(int userID, int mirrorID)
        {
            var authorizationResult = (new BasicServerAuthorizationMethod()).authorizeServer(this);
            if (authorizationResult != null) return authorizationResult;
            IUserMirrorsManager um = new UserMirrorsManager();
            um.deleteUsersMirror(userID, mirrorID);
            return Ok();
        }

        //deletes all user's mirrors
        [Route("{userID:int}/mirrors")]
        public IHttpActionResult Delete(int userID)
        {
            var authorizationResult = (new BasicServerAuthorizationMethod()).authorizeServer(this);
            if (authorizationResult != null) return authorizationResult;
            IUserMirrorsManager um = new UserMirrorsManager();
            um.deleteUsersMirrors(userID);
            return Ok();
        }
        
    }
}
