using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetlekarz.Models
{
    public class UserModel : ICloneable
    {
        public UserModel()
        {

        }
        public UserModel(string name, string surname, string login, string password, Role role)
        {
            this.name = name;
            this.surname = surname;
            this.login = login;
            this.password = password;
            this.role = role;
        }

        public enum Role { Doctor, Visitor};

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public String name { get; set; }

        public String surname { get; set; }

        public String login { get; set; }

        public String password { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public Role role { get; set; }

        public object Clone()
        {
            var clone = new UserModel();
            clone.id = this.id;
            clone.name = this.name;
            clone.surname = this.surname;
            clone.login = this.login;
            clone.password = this.password;
            clone.role = this.role;
            return clone;
        }
    }
}
