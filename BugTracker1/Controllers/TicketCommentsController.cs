using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker1.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker1.Controllers
{
    public class TicketCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketComments
        public ActionResult Index()
        {
            var ticketComments = db.Comments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketComments.ToList());
        }

        // GET: TicketComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComments ticketComments = db.Comments.Find(id);
            if (ticketComments == null)
            {
                return HttpNotFound();
            }
            return View(ticketComments);
        }

        // GET: TicketComments/Create
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName");
            return View();
        }

        // POST: TicketComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Comment,Created,TicketId,UserId")] TicketComments ticketComments)
        {
            if (ModelState.IsValid)
            {
                //db.Comments.Add(ticketComments);
                //db.SaveChanges();           
                var ticket = db.Tickets.Find(ticketComments.TicketId);
                ticketComments.UserId = User.Identity.GetUserId();
                ticketComments.Created = DateTime.Now;
                db.Comments.Add(ticketComments);
                db.SaveChanges();
     
                return RedirectToAction("Details","Tickets", new { id = ticket.Id });
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName", ticketComments.UserId);
            return View(ticketComments);
        }

        // GET: TicketComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComments ticketComments = db.Comments.Find(id);
            if (ticketComments == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName", ticketComments.UserId);
            return View(ticketComments);
        }

        // POST: TicketComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Comment,Created,TicketId,UserId")] TicketComments ticketComments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketComments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName", ticketComments.UserId);
            return View(ticketComments);
        }

        // GET: TicketComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComments ticketComments = db.Comments.Find(id);
            if (ticketComments == null)
            {
                return HttpNotFound();
            }
            return View(ticketComments);
        }

        // POST: TicketComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketComments ticketComments = db.Comments.Find(id);
            db.Comments.Remove(ticketComments);
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
