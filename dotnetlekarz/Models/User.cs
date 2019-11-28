using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetlekarz.Models
{


    public class User : ICloneable
    {
        public User()
        {

        }
        public User(string name, string surname, string login, string password, Role role)
        {
            this.Name = name;
            this.Surname = surname;
            this.Login = login;
            this.Password = password;
            this.UserRole = role;
        }

        public enum Role { Doctor, Visitor };
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public String Name { get; set; }

        public String Surname { get; set; }

        public String Login { get; set; }

        public String Password { get; set; }

        [InverseProperty("Doctor")]

        public List<Visit> ManagedVisits { get; set; }

        [InverseProperty("Visitor")]
        public List<Visit> AttendedVisits { get; set; }

        public Role UserRole { get; set; }

        public object Clone()
        {
            var clone = new User();
            clone.UserId = this.UserId;
            clone.Name = this.Name;
            clone.Surname = this.Surname;
            clone.Login = this.Login;
            clone.Password = this.Password;
            clone.UserRole = this.UserRole;
            return clone;
        }
    }
}
