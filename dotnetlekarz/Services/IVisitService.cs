using dotnetlekarz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetlekarz.Services
{
    public interface IVisitService
    {
        void AddVisit(VisitModel visit);

        bool RemoveVisit(int id);

        void ModifyVisit(VisitModel visit);


        VisitModel GetVisit(int id);

        List<VisitModel> GetAllVisits();
    }
}
