using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Models
{
    public class SessionStartParameters
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Size { get; set; }
        public int PlanetCount { get; set; }
        public int PlayerLimit { get; set; }
    }
}
