using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Index = Microsoft.EntityFrameworkCore.Metadata.Internal.Index;

namespace dotnetlekarz.Models
{
    public class Visit : ICloneable
    {
        public Visit()
        {
        }

        public Visit(User doctor, User visitor, DateTime dateTime)
        {
            this.Doctor = doctor;
            this.Visitor = visitor;
            this.DateTime = dateTime;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VisitId { get; set; }
        public int DoctorId { get; set; }
        public User Doctor { get; set; }

        public int VisitorId { get; set; }
        public User Visitor { get; set; }

        public System.DateTime DateTime { get; set; }

        public object Clone()
        {
            var clone = new Visit();
            clone.VisitId = this.VisitId;
            clone.Doctor = this.Doctor;
            clone.Visitor = this.Visitor;
            clone.DateTime = this.DateTime;
            return clone;
        }
    }
}
