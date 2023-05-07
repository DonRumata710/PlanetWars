using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using GameServer.Models;
using Interfaces.Models;
using Serilog;
using static GameServer.DatabaseService;

namespace GameServer.Controllers
{
    [Route("game")]
    [Authorize]
    [ApiController]
    public class GameSessionController : ControllerBase
    {
        public GameSessionController(DatabaseService db, GameManager gm)
        {
            database = db;
            gameManager = gm;

            database.SetServerStatus(true);
        }

        [HttpGet]
        public IActionResult StartSession(int id)
        {
            try
            {
                Log.Information("Session " + id.ToString() + " started");

                Session session = database.GetSession(id);
                gameManager.AddRoom(id, new Room(session));

                Room room = gameManager.GetRoom(id);

                GameState state = room.ToGameState();
                return Ok(state);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to start session: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post()
        {
            return Ok();
        }

        private DatabaseService database;
        private GameManager gameManager;
    }
}
