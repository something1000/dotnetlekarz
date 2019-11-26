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

        private IUserService userService;
        private DocDbContext _dbContext { get; }
        public VisitService(IUserService userService, DocDbContext dbContext)
        {
            _dbContext = dbContext;
            this.userService = userService;

            visits = new List<VisitModel>();


            userService.AddUser(new UserModel("Jan", "Kowalski", "jkowalski", "123", UserModel.Role.Doctor));
            userService.AddUser(new UserModel("Wojciech", "Rzezucha", "wrzezucha", "123", UserModel.Role.Doctor));
            userService.AddUser(new UserModel("Bart", "Gawrych", "bgawrych", "123", UserModel.Role.Visitor));
            userService.AddUser(new UserModel("Adam", "Grabowski", "agrabowski", "123", UserModel.Role.Visitor));
            userService.AddUser(new UserModel("Kamil", "Jablonski", "kjablonski", "123", UserModel.Role.Visitor));

            AddVisit(new VisitModel(userService.GetUserByLogin("jkowalski"), userService.GetUserByLogin("agrabowski"), DateTime.Now));
            AddVisit(new VisitModel(userService.GetUserByLogin("jkowalski"), userService.GetUserByLogin("kjablonski"), DateTime.Now));
            AddVisit(new VisitModel(userService.GetUserByLogin("wrzezucha"), userService.GetUserByLogin("bgawrych"), DateTime.Now));
            AddVisit(new VisitModel(userService.GetUserByLogin("wrzezucha"), userService.GetUserByLogin("kjablonski"), DateTime.Now));
        }

        //public static VisitService GetInstance()
        //{
        //    return singletonVisitService;
        //}


        public void AddVisit(VisitModel visit)
        {
            //visit.id = Interlocked.Increment(ref lastId);
            visits.Add(visit);
            _dbContext.Visits.Add(visit);
            _dbContext.SaveChanges();
        }

        public bool RemoveVisit(int id)
        {
            var toRemove = _dbContext.Visits.Find(id);
            if(toRemove != null)
            {
                _dbContext.Visits.Remove(toRemove);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public void ModifyVisit(VisitModel visit)
        {
            //var foundVisit = visits.(x => x.id == visit.id);
            //if(foundVisit >= 0)
            //{
            //    visits[foundVisit] = visit;
            //}
            _dbContext.Visits.Update(visit);
        }

        public VisitModel GetVisit(int id)
        {
            var foundVisit = _dbContext.Visits.Find(id);
            if(foundVisit == null)
            {
                return null;
            }
            return foundVisit;
        }

        public List<VisitModel> GetAllVisits()
        {
            List<VisitModel> listCopy = _dbContext.Visits.ToList();
            //visits.ForEach(v =>
            //{
            //    listCopy.Add((VisitModel)v.Clone());
            //});
            return listCopy;
        }
    }
}
