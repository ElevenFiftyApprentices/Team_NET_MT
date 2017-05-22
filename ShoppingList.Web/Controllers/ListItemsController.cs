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
using PagedList;

namespace ShoppingList.Web.Controllers
{
    public class ListItemsController : Controller
    {
        private ShoppingListDbContext db = new ShoppingListDbContext();

        // GET: ListItems
        // GET: Customer
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ItemSortParm = String.IsNullOrEmpty(sortOrder) ? "item_desc" : "";
            ViewBag.PrioritySortParm = sortOrder == "Priority" ? "priority_desc" : "Priority";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from s in db.ShoppingListItems
                            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Contents.Contains(searchString));
   
            }
            switch (sortOrder)
            {
                case "item_desc":
                    items = items.OrderByDescending(s => s.Contents);
                    break;
                case "Priority":
                    items = items.OrderBy(s => s.Priority);
                    break;
                case "priority_desc":
                    items = items.OrderByDescending(s => s.Priority);
                    break;
                default:
                    items = items.OrderBy(s => s.Contents);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Edit(int? id)
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

        // POST: ListItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShoppingListItemID,ShoppingListID,Contents,Priority,Note,IsChecked,CreatedUtc,ModidiedUtc")] ShoppingListItem shoppingListItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shoppingListItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shoppingListItem);
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
