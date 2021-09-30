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
            servers = database.ServerList();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(sessions);
        }

        [HttpPost]
        [Route("join")]
        public IActionResult Join(int sessionId)
        {
            sessions[sessionId].Players.Add(Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
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

        [HttpPost]
        public IActionResult RegisterSession(SessionStartParameters param)
        {
            var userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            Session session = new Session
            {
                Parameters = param,
                Players = { userId }
            };

            int id = database.CreateNewSession(param);
            sessions.Add(id, session);

            return Ok(id);
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult RefreshSession(int id, SessionStartParameters param)
        {
            database.RefreshSession(id, param);
            sessions[id].Parameters = param;
            return Ok();
        }

        [HttpPost]
        [Route("start")]
        public IActionResult StartSession(int id)
        {
            int serverId = -1;
            float minRatio = float.MaxValue;
            foreach(var server in servers)
            {
                float ratio = server.Value.ActiveSessions / (float)server.Value.SessionLimit;
                if (ratio < minRatio)
                {
                    serverId = server.Key;
                    minRatio = ratio;
                }
            }

            database.StartSession(id, sessions[id].Players, serverId);

            return Ok();
        }


        private DatabaseService database;
        private Dictionary<int, GameServer> servers;
        private Dictionary<int, Session> sessions = new Dictionary<int, Session>();
    }
}
