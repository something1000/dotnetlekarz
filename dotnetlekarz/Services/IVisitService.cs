using dotnetlekarz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetlekarz.Services
{
    public interface IVisitService
    {
        void AddVisit(Visit visit);

        bool RemoveVisit(int id);

        void ModifyVisit(Visit visit);


        Visit GetVisit(int id);

        List<Visit> GetAllVisits();
    }
}
