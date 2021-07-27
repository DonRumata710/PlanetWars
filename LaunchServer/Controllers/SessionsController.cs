using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Interfaces.Models;

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
        [Route("default")]
        public IActionResult GetDefaultSettings()
        {
            SessionStartParameters res = new SessionStartParameters();
            res.PlanetCount = 4;
            res.PlayerLimit = 2;
            res.Size = 10;
            return Ok(res);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(sessions);
        }

        [HttpPost]
        public IActionResult Post(SessionStartParameters param)
        {
            Session session = new Session
            {
                Parameters = param
            };

            int id = database.CreateNewSession(session);
            sessions.Add(id, session);

            return Ok(id);
        }

        [HttpPut]
        public IActionResult Put(int id)
        {
            string serverAddress = null;
            float minRatio = float.MaxValue;
            foreach(var server in servers)
            {
                float ratio = server.Value.ActiveSessions / (float)server.Value.SessionLimit;
                if (ratio < minRatio)
                {
                    serverAddress = server.Key;
                    minRatio = ratio;
                }
            }

            database.StartSession(id);

            return Ok();
        }


        private DatabaseService database;
        private Dictionary<string, GameServer> servers;
        private Dictionary<int, Session> sessions = new Dictionary<int, Session>();
    }
}
