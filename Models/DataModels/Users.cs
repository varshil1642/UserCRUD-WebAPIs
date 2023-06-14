using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DataModels
{
    public class Users
    {
        public long UserId {  get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string MobileNo { get; set; } = string.Empty;

        public int Gender { get; set; }

        public DateTime DateofBirth { get; set; }

        public string ProfileImage { get; set; } = string.Empty;

        public int UserType { get; set; }
    }
}
