using BugTracker1.Models;
using BugTracker1.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker1.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            List<AdminIndexViewModel> model = new List<AdminIndexViewModel>();
            UserRolesHelper helper = new UserRolesHelper();
            foreach(var user in db.Users)
            {
            AdminIndexViewModel vm = new AdminIndexViewModel();
                vm.User = user;
                vm.Roles = helper.ListUserRoles(user.Id);
                model.Add(vm);
            }
            return View(model);
        }

        //GET: EditUser
        public ActionResult EditUser(string id)
        {
            var user = db.Users.Find(id);
            AdminUserViewModel AdminModel = new AdminUserViewModel();
            UserRolesHelper helper = new UserRolesHelper();
            var selected = helper.ListUserRoles(id);
            AdminModel.Roles = new MultiSelectList(db.Roles, "Name", "Name", selected);
            AdminModel.User = new ApplicationUser();
            AdminModel.User.Id= user.Id;
            AdminModel.User.FullName = user.FullName;
            return View(AdminModel);
        }

        //POST:  EditUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(AdminUserViewModel model)
        {
            //var user = db.Users.Find(model.User.Id);
            UserRolesHelper helper = new UserRolesHelper();
            foreach (var rolermv in db.Roles.Select(r => r.Name).ToList())
            {
                helper.RemoveUserFromRole(model.User.Id, rolermv);
            }
            foreach (var roleadd in model.SelectedRoles)
            {
                helper.AddUserToRole(model.User.Id, roleadd);
            }
            return RedirectToAction("Index");
        }
    }
}