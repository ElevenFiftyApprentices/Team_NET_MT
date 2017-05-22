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

namespace ShoppingList.Web.Controllers
{
    public class ShoppingListController : Controller
    {
        private ShoppingListDbContext db = new ShoppingListDbContext();

        // GET: ShoppingList
        public ActionResult Index()
        {
            return View(db.ShoppingLists.ToList());
        }

        // GET: ShoppingList/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shopping_List shopping_List = db.ShoppingLists.Find(id);
            if (shopping_List == null)
            {
                return HttpNotFound();
            }
            return View(shopping_List);
        }

        // GET: ShoppingList/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShoppingList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShoppingListID,UserID,Name,Color,CreatedUtc,ModidiedUtc")] Shopping_List shopping_List)
        {
            if (ModelState.IsValid)
            {
                db.ShoppingLists.Add(shopping_List);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shopping_List);
        }

        // GET: ShoppingList/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shopping_List shopping_List = db.ShoppingLists.Find(id);
            if (shopping_List == null)
            {
                return HttpNotFound();
            }
            return View(shopping_List);
        }

        // POST: ShoppingList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShoppingListID,UserID,Name,Color,CreatedUtc,ModidiedUtc")] Shopping_List shopping_List)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shopping_List).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shopping_List);
        }

        // GET: ShoppingList/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shopping_List shopping_List = db.ShoppingLists.Find(id);
            if (shopping_List == null)
            {
                return HttpNotFound();
            }
            return View(shopping_List);
        }

        // POST: ShoppingList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shopping_List shopping_List = db.ShoppingLists.Find(id);
            db.ShoppingLists.Remove(shopping_List);
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
