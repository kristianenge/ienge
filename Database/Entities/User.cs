using System;

namespace IEnge.Database.Entities
{
    public class User : DbMotherObj
    {

        public string Username { get; set; }
        public UserType UserType { get; set; }
        public UserRole UserRole { get; set; }
        public DateTimeOffset LastLogin { get; set; }
    }
}
