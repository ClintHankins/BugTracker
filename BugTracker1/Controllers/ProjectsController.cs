using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker1.Models;
using BugTracker1.Models.Helpers;
using Microsoft.AspNet.Identity;

namespace BugTracker1.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        [Authorize(Roles ="Admin, ProjectManager")]
        public ActionResult Index()
        {

            return View(db.Projects.ToList());
        }

        public ActionResult MyProjects()
        {
            var userId = User.Identity.GetUserId();
            //var user = db.Users.Find(userId);
            //var model = db.Projects.Where(p => p.Users.Select(u => u.Id).Contains(userId)).Include("Users");

            var allProjects = db.Projects.Where(p => p.Ticket.Select(t => t.AssignedToUserId).Contains(userId) || p.Ticket.Select(t => t.OwnerUserId).Contains(userId)).Include("Ticket").ToList();

            return View(allProjects);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            ProjectDetailsView vm = new ProjectDetailsView();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vm.Projects = db.Projects.Find(id);
            if (vm.Projects == null)
            {
                return HttpNotFound();
            }
            vm.ProjectManager = db.Users.Find(vm.Projects.ProjectMangerId);
            return View(vm);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Create([Bind(Include = "Id,Name")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projects);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            UserRolesHelper helper = new UserRolesHelper();
            var pms = helper.UsersInRole("ProjectManager");

            ViewBag.ProjectManagerId = new SelectList(pms, "Id", "FullName");
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit([Bind(Include = "Id,Name,ProjectManagerId")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projects);
        }

        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult EditUsers(int id)
        {
            var project = db.Projects.Find(id);
            ProjectUsersViewModel pvm = new ProjectUsersViewModel();

            ProjectHelper helper = new ProjectHelper();

            var selected = helper.ListUsersOnProject(id);

            pvm.Users = new MultiSelectList(db.Users, "Id", "FirstName", selected);
            pvm.Project = project;

            return View(pvm);
        }

        [HttpPost]
        public ActionResult EditUsers(ProjectUsersViewModel model)
        {
            ProjectHelper helper = new ProjectHelper();

            foreach(var user in db.Users)
            {
                helper.RemoveUserFromProject(user.Id, model.Project.Id);
            }

            foreach(var user in model.SelectedUsers)
            {
                helper.AddUserToProject(user, model.Project.Id);
            }
            return RedirectToAction("Index");
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projects projects = db.Projects.Find(id);
            db.Projects.Remove(projects);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
