using System;
using System.Collections.Generic;

namespace DatingApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }

        public User()
        {
            Created=DateTime.Now;
        }

    }
}