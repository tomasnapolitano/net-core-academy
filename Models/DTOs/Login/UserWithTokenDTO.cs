using Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Login
{
    public class UserWithTokenDTO : UserDTO
    {
        public string Token { get; set; }
    }
}
