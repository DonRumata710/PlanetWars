using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interfaces.Models
{
    public class Session
    {
        public int ServerId { get; set; }
        public string ServerAddress { get; set; }
        public SessionStartParameters Parameters { get; set; }
        public List<int> Players { get; set; }
    }
}
