using LaunchServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LaunchServer.Controllers
{
    [Route("user")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController(DatabaseService _database)
        {
            database = _database;
        }

        [HttpGet]
        public IActionResult Get(string username)
        {
            try
            {
                UserInfo res = new UserInfo();
                res.name = username;

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                UserInfo userInfo = database.GetUserInfo(int.Parse(userId));

                if (userInfo.name == username)
                {
                    res = userInfo;
                }

                return Ok(res);
            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id:int:min(1)}")]
        public IActionResult GetName(int id)
        {
            UserInfo userInfo = database.GetUserInfo(id);
            return Ok(userInfo.name);
        }

        private DatabaseService database;
    }
}
