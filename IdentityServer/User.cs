using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer
{
    [DisplayName("User")]
    public class User
    {
        [Key]
        public ulong userId;
        public string name;
        public string email;
        public string password;
        public DateTime? registerTime;
        public bool isActive;

        public User(ulong _userId, string _name, string _email, string _password, DateTime? _regTime, bool _isActive)
        {
            userId = _userId;
            name = _name;
            email = _email;
            password = _password;
            registerTime = _regTime;
            isActive = _isActive;
        }
    }
}
