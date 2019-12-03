using dotnetlekarz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace dotnetlekarz.Services
{
    public class VisitService : IVisitService
    {
        //private static readonly VisitService singletonVisitService = new VisitService();
        private List<Visit> visits { get; }

        private IUserService userService;
        private DocDbContext _dbContext { get; }
        public VisitService(IUserService userService, DocDbContext dbContext)
        {
            _dbContext = dbContext;
            this.userService = userService;

            visits = new List<Visit>();

            if (_dbContext.Users.Count() == 0 && _dbContext.Visits.Count() == 0)
            {
                userService.AddUser(new User("Jan", "Kowalski", "jkowalski", "123", User.Role.Doctor));
                userService.AddUser(new User("Wojciech", "Rzezucha", "wrzezucha", "123", User.Role.Doctor));
                userService.AddUser(new User("Bart", "Gawrych", "bgawrych", "123", User.Role.Visitor));
                userService.AddUser(new User("Adam", "Grabowski", "agrabowski", "123", User.Role.Visitor));
                userService.AddUser(new User("Kamil", "Jablonski", "kjablonski", "123", User.Role.Visitor));

                AddVisit(new Visit(userService.GetUserByLogin("jkowalski"), userService.GetUserByLogin("agrabowski"), DateTime.Now));
                AddVisit(new Visit(userService.GetUserByLogin("jkowalski"), userService.GetUserByLogin("kjablonski"), DateTime.Now));
                AddVisit(new Visit(userService.GetUserByLogin("wrzezucha"), userService.GetUserByLogin("bgawrych"), DateTime.Now));
                AddVisit(new Visit(userService.GetUserByLogin("wrzezucha"), userService.GetUserByLogin("kjablonski"), DateTime.Now));
            }
        }

        //public static VisitService GetInstance()
        //{
        //    return singletonVisitService;
        //}


        public void AddVisit(Visit visit)
        {
            //visit.id = Interlocked.Increment(ref lastId);
            //visits.Add(visit);
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

        public void ModifyVisit(Visit visit)
        {
            //var foundVisit = visits.(x => x.id == visit.id);
            //if(foundVisit >= 0)
            //{
            //    visits[foundVisit] = visit;
            //}
            _dbContext.Visits.Update(visit);
        }

        public Visit GetVisit(int id)
        {
            var foundVisit = _dbContext.VisitsWithUsers.SingleOrDefault(v => v.VisitId == id);
            if(foundVisit == null)
            {
                return null;
            }
            return foundVisit;
        }

        public List<Visit> GetAllVisits()
        {
            List<Visit> listCopy = _dbContext.VisitsWithUsers.ToList();
            //visits.ForEach(v =>
            //{
            //    listCopy.Add((VisitModel)v.Clone());
            //});
            return listCopy;
        }

        public List<Visit> GetVisitsByVisitor(string visitorLogin)
        {
            List<Visit> visits = _dbContext.VisitsWithUsers.Where(v => v.Visitor.Login == visitorLogin).ToList();
            
            return visits;
        }

        public List<Visit> GetVisitsByDoctor(string doctorLogin)
        {
            List<Visit> visits = _dbContext.VisitsWithUsers.Where(v => v.Doctor.Login == doctorLogin).ToList();

            return visits;
        }

        public List<Visit> GetVisitsDocDate(User doctor, DateTime date)
        {
            List <Visit> visits = _dbContext.Visits.Where(x => x.Doctor.UserId == doctor.UserId).Where(y => y.DateTime.Date == date.Date).ToList();
            return visits;
        }
    }
}
