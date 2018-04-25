using BugTracker1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker1.Models.Helpers
{
    public class ProjectHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Exception AddUserToProject(string userId, int projectId)
        {
            try
            {
                var prj = db.Projects.Find(projectId);
                var usr = db.Users.Find(userId);
                prj.Users.Add(usr);
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception RemoveUserFromProject(string userId, int projectId)
        {
            try
            {
                var prj = db.Projects.Find(projectId);
                var usr = db.Users.Find(userId);
                prj.Users.Remove(usr);
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public ICollection<Projects> ListUserProjects(string UserId)
        {
            return db.Users.Find(UserId).Projects.ToList();
        }

        public bool IsUserOnProject(string UserId, int ProjectId)
            {
                try
                {
                    var usr = db.Users.Find(UserId);
                    var result = db.Projects.Find(ProjectId).Users.Contains(usr);
                    return result;
                }
                catch
                {
                    return false;
                }
            }

        public ICollection<ApplicationUser> ListUsersNotInProject(int projectId)
        {
            List<ApplicationUser> usrNotInProject = new List<ApplicationUser>();
            List<ApplicationUser> usrs = db.Users.ToList();
            foreach (var u in usrs)
            {
                if (!IsUserOnProject(u.Id, projectId))
                {
                    usrNotInProject.Add(u);
                }
            }
            return usrNotInProject;
        }

        public List<ApplicationUser> ListUsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users.ToList();
        }

        //var usr = db.Users.Find(UserId);
        //List<Projects> ProjUsers = new List<Projects>();
        //try
        //{
        //    var prj = db.Projects.ToList();
        //    foreach (var p in prj)
        //    {
        //        if(p.Users.Contains(usr))
        //        {
        //            ProjUsers.Add(p);
        //        }
        //    }
        //    return ProjUsers;
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex);
        //    return null;
        //}        
    }
}