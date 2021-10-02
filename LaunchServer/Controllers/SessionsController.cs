using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Interfaces.Models;
using System.Security.Claims;

namespace LaunchServer.Controllers
{
    [Route("sessions")]
    [Authorize]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        public SessionsController(DatabaseService db)
        {
            database = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(database.GetNotStartedSessions());
        }

        [HttpPost]
        [Route("join")]
        public IActionResult Join(int sessionId)
        {
            database.AddPlayer(sessionId, Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok();
        }

        [HttpGet]
        [Route("leave")]
        public IActionResult Leave()
        {
            database.LeaveSessions(Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok();
        }

        [HttpGet]
        [Route("default")]
        public IActionResult GetDefaultSettings()
        {
            SessionStartParameters res = new SessionStartParameters();
            res.PlanetCount = 4;
            res.PlayerLimit = 2;
            res.Size = 10;
            return Ok(res);
        }

        const int PlanetMinCount = 2;
        const int PlayerMinCount = 2;
        const int MinSize = 3;

        [HttpGet]
        [Route("min")]
        public IActionResult GetMinSettings()
        {
            SessionStartParameters res = new SessionStartParameters();
            res.PlanetCount = PlanetMinCount;
            res.PlayerLimit = PlayerMinCount;
            res.Size = MinSize;
            return Ok(res);
        }

        [HttpPost]
        public IActionResult RegisterSession([FromBody]SessionStartParameters param)
        {
            if (param.Name == null ||
                param.PlanetCount < PlanetMinCount ||
                param.PlayerLimit < PlayerMinCount ||
                param.Size < MinSize
            )
                return StatusCode(422);

            var userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            int id = database.CreateNewSession(param);
            database.AddPlayer(userId, id);

            return Ok(id);
        }

        [HttpPut]
        public IActionResult RefreshSession(int id, SessionStartParameters param)
        {
            database.RefreshSession(id, param);
            return Ok();
        }

        [HttpGet]
        [Route("start")]
        public IActionResult StartSession(int id)
        {
            int serverId = -1;
            string serverAddress = null;
            float minRatio = float.MaxValue;
            foreach(var server in database.ServerList())
            {
                float ratio = server.Value.ActiveSessions / (float)server.Value.SessionLimit;
                if (ratio < minRatio)
                {
                    serverId = server.Key;
                    serverAddress = server.Value.Address;
                    minRatio = ratio;
                }
            }

            database.StartSession(id, serverId);

            return Ok(serverAddress);
        }


        private DatabaseService database;
    }
}
