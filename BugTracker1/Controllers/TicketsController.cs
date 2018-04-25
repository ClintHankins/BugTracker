using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker1.Models;
using BugTracker1.Models.Helpers;
using Microsoft.AspNet.Identity;

namespace BugTracker1.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Projects).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        //GET: MyTickets
        public ActionResult MyTickets()
        {
            var userId = User.Identity.GetUserId();
            var tickets = db.Tickets.Where(u => u.AssignedToUserId == userId).Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Projects).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        //GET: OwnerId
        public ActionResult OwnedTickets()
        {
            var userId = User.Identity.GetUserId();
            var tickets = db.Tickets.Where(u => u.OwnerUserId == userId).Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Projects).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FullName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectsId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.Priorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.Statuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.Types, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Submitter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,Updated,ProjectsId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId,MediaURL")]
        Tickets tickets, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                tickets.OwnerUserId = User.Identity.GetUserId();
                tickets.Created = DateTimeOffset.Now;
                db.Tickets.Add(tickets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectsId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectsId);
            ViewBag.TicketPriorityId = new SelectList(db.Priorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.Statuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.Types, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectsId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectsId);
            ViewBag.TicketPriorityId = new SelectList(db.Priorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.Statuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.Types, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
        }


        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectsId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                //var model = db.Tickets.Find(tickets.Id);
                var oldTic = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == tickets.Id);
                foreach (var prop in typeof(Tickets).GetProperties())
                {
                    if (prop.Name != null && prop.Name.In("Title", "Description", "TicketTypeId", "TicketPriorityId", "TicketStatusId", "AssignedToUserId"))
                    {

                        var oldInt = oldTic.GetType().GetProperty(prop.Name).GetValue(oldTic);
                        var newInt = tickets.GetType().GetProperty(prop.Name).GetValue(tickets);

                        var OldValue = oldTic.GetType().GetProperty(prop.Name).GetValue(oldTic).ToString();
                        var NewValue = tickets.GetType().GetProperty(prop.Name).GetValue(tickets).ToString();

                        if (prop.Name == "TicketTypeId")
                        {
                            OldValue = db.Types.Find(oldInt).Name;
                            NewValue = db.Types.Find(newInt).Name;
                        }
                        if (prop.Name == "TicketStatusId")
                        {
                            OldValue = db.Statuses.Find(oldInt).Name;
                            NewValue = db.Statuses.Find(newInt).Name;
                        }
                        if (prop.Name == "TicketPriorityId")
                        {
                            OldValue = db.Priorities.Find(oldInt).Name;
                            NewValue = db.Priorities.Find(newInt).Name;
                        }

                        if (OldValue != NewValue)
                        {
                            TicketHistory ticketHistory = new TicketHistory()
                            {
                                TicketId = tickets.Id,
                                UserId = User.Identity.GetUserId(),
                                Property = prop.Name,
                                OldValue = OldValue,
                                NewValue = NewValue,
                                Changed = DateTime.Now
                            };
                            db.Histories.Add(ticketHistory);
                            //db.TicketHistories.Add(ticketHistory);
                            //db.SaveChanges();
                        }
                    }
                    //return RedirectToAction("Details", new { id = tickets.Id });
                }
                db.Entry(tickets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");


            }
                ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
                ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "Name", tickets.OwnerUserId);
                ViewBag.ProjectsId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectsId);
                ViewBag.TicketPriorityId = new SelectList(db.Priorities, "Id", "Name", tickets.TicketPriorityId);
                ViewBag.TicketStatusId = new SelectList(db.Statuses, "Id", "Name", tickets.TicketStatusId);
                ViewBag.TicketTypeId = new SelectList(db.Types, "Id", "Name", tickets.TicketTypeId);
                return View(tickets);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tickets tickets = db.Tickets.Find(id);
            db.Tickets.Remove(tickets);
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
