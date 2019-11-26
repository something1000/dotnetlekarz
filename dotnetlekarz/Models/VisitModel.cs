using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetlekarz.Models
{
    public class VisitModel : ICloneable
    {
        public VisitModel()
        {
        }

        public VisitModel(UserModel doctor, UserModel visitor, DateTime dateTime)
        {
            this.doctor = doctor;
            this.visitor = visitor;
            this.dateTime = dateTime;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public UserModel doctor { get; set; }

        public UserModel visitor { get; set; }

        public System.DateTime dateTime { get; set; }

        public object Clone()
        {
            var clone = new VisitModel();
            clone.id = this.id;
            clone.doctor = this.doctor;
            clone.visitor = this.visitor;
            clone.dateTime = this.dateTime;
            return clone;
        }

        public override String ToString()
        {
            return doctor + " " + visitor + " " + dateTime;
        }
    }
}
