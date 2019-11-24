using dotnetlekarz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dotnetlekarz.Services
{
    public class VisitService : IVisitService
    {
        //private static readonly VisitService singletonVisitService = new VisitService();
        private List<VisitModel> visits { get; }
        private int lastId = 0;

        public VisitService()
        {
            visits = new List<VisitModel>();
            AddVisit(new VisitModel("jkowalski", "agrabowski", DateTime.Now));
            AddVisit(new VisitModel("jkowalski", "kjablonski", DateTime.Now));
            AddVisit(new VisitModel("wrzezucha", "bgawrych", DateTime.Now));
            AddVisit(new VisitModel("wrzezucha", "kjablonski", DateTime.Now));
        }

        //public static VisitService GetInstance()
        //{
        //    return singletonVisitService;
        //}


        public void AddVisit(VisitModel visit)
        {
            visit.id = Interlocked.Increment(ref lastId);
            visits.Add(visit);
        }

        public bool RemoveVisit(int id)
        {
            var toRemove = visits.Find(x => x.id == id);
            if(toRemove != null)
            {
                visits.Remove(toRemove);
                return true;
            }
            return false;
        }

        public void ModifyVisit(VisitModel visit)
        {
            var foundVisit = visits.FindIndex(x => x.id == visit.id);
            if(foundVisit >= 0)
            {
                visits[foundVisit] = visit;
            }
        }

        public VisitModel GetVisit(int id)
        {
            var foundVisit = visits.Find(v => v.id == id);
            if(foundVisit == null)
            {
                return null;
            }
            return (VisitModel)foundVisit.Clone();
        }

        public List<VisitModel> GetAllVisits()
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
