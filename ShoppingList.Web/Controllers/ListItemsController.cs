using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingList.Data;
using ShoppingList.Data.Models;
using ShoppingList.Models;
using Microsoft.AspNet.Identity;
using ShoppingList.Services;

namespace ShoppingList.Web.Controllers
{
    [Authorize]
    public class ListItemsController : Controller
    {
        private ShoppingListDbContext db = new ShoppingListDbContext();

        // GET: ListItems
        public ActionResult Index()
        {
            return View(db.ShoppingListItems.ToList());
        }

        // GET: ListItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingListItem shoppingListItem = db.ShoppingListItems.Find(id);
            if (shoppingListItem == null)
            {
                return HttpNotFound();
            }
            return View(shoppingListItem);
        }

        // GET: ListItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ListItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShoppingItemCreate model)
        {


            if (!ModelState.IsValid) return View(model);

            var service = CreateService();

            if (service.CreateItem(model))
            {
                TempData["SaveResult"] = "Your note was successfully created!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your note could not be created.");
            return RedirectToAction("Index");

        }

        // GET: ListItems/Edit/5
        public ActionResult Edit(int id)
        {

            var service = CreateService();
            var detail = service.GetNoteById(id);

            var model =
                new ShoppingItemEdit
                {
                    ShoppingListItemID = detail.ShoppingListItemID,
                    Contents = detail.Contents,
                    Priority = detail.Priority,
                    Note = detail.Note
                };
            return View(model);
        }

        // POST: ListItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ShoppingItemEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.ShoppingListItemID != id)
            {
                ModelState.AddModelError("", "ID Mismatch");
                return View(model);
            }

            var service = CreateService();


            if (service.UpdateItem(model))
            {
                TempData["SaveResult"] = "Your note was successfully updated!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your note could not be updated.");

            return View(model);
        }

        // GET: ListItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingListItem shoppingListItem = db.ShoppingListItems.Find(id);
            if (shoppingListItem == null)
            {
                return HttpNotFound();
            }
            return View(shoppingListItem);
        }

        // POST: ListItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingListItem shoppingListItem = db.ShoppingListItems.Find(id);
            db.ShoppingListItems.Remove(shoppingListItem);
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

        private ShoppingListItemService CreateService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ShoppingListItemService(userId);
            return service;
        }
    }
}
