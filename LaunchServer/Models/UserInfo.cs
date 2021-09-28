using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaunchServer.Models
{
    public class UserInfo
    {
        public ulong userId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime? registerTime { get; set; }
    }
}
