using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Models
{
    public class GameServer
    {
        public string Address { get; set; }
        public long ActiveSessions { get; set; }
        public long SessionLimit { get; set; }
    }
}
