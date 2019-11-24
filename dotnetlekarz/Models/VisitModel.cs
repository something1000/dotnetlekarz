using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetlekarz.Models
{
    public class VisitModel : ICloneable
    {
        public int id { get; set; }
        public String doctor { get; set; }

        public String visitor { get; set; }

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
    }
}
