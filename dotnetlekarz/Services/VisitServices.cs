using dotnetlekarz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dotnetlekarz.Services
{
    public class VisitServices
    {
        private static readonly VisitServices singletonVisitService = new VisitServices();

        private VisitServices()
        {

        }

        public static VisitServices GetInstance()
        {
            return singletonVisitService;
        }

        private List<VisitModel> visits { get; }
        private int lastId = 0;

        private void AddVisit(VisitModel visit)
        {
            visit.id = Interlocked.Increment(ref lastId);
            visits.Add(visit);
        }

        private bool RemoveVisit(int id)
        {
            var toRemove = visits.Find(x => x.id == id);
            if(toRemove != null)
            {
                visits.Remove(toRemove);
                return true;
            }
            return false;
        }

        private void ModifyVisit(VisitModel visit)
        {
            var foundVisit = visits.FindIndex(x => x.id == visit.id);
            if(foundVisit >= 0)
            {
                visits[foundVisit] = visit;
            }
        }

        private VisitModel GetVisit(int id)
        {
            var foundVisit = visits.Find(v => v.id == id);
            if(foundVisit == null)
            {
                return null;
            }
            return (VisitModel)foundVisit.Clone();
        }

        private List<VisitModel> GetAllVisits()
        {
            List<VisitModel> listCopy = new List<VisitModel>();
            visits.ForEach(v =>
            {
                listCopy.Add((VisitModel)v.Clone());
            });
            return listCopy;
        }
    }
}
