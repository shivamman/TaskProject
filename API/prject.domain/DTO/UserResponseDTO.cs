using System;
using System.Collections.Generic;
using System.Text;

namespace project.domain.DTO
{
    public class UserResponseDTO : LoginDTO
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> RoleNames { get; set; }
    }


    public class LoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
