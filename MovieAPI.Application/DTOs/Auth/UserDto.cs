using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Application.DTOs.Auth {
    public class UserDto {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        //public string PhoneNumber { get; set; }
        //public bool EmailConfirmed { get; set; }
        //public bool PhoneNumberConfirmed { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime LastLoginAt { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
