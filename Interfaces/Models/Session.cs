using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interfaces.Models
{
    public class Session
    {
        public SessionStartParameters Parameters { get; set; }
        public List<int> Players { get; set; }
    }
}
