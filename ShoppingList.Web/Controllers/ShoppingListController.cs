﻿using Microsoft.AspNet.Identity;
using ShoppingList.Data;
using ShoppingList.Data.Models;
using ShoppingList.Models;
using ShoppingList.Services;
using System;
using System.Net;
using System.Web.Mvc;

namespace ShoppingList.Web.Controllers
{
    [Authorize]
    public class ShoppingListController : Controller
    {
        private ShoppingListDbContext db = new ShoppingListDbContext();

        // GET: ShoppingList
        public ActionResult Index()
        {
            var service = CreateService();
            var model = service.GetLists();
            return View(model);
        }

        // GET: ShoppingList/Details/5
        public ActionResult Details(int? id)
        {

            var service = CreateService();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shopping_List shopping_List = db.ShoppingLists.Find(id);
            if (shopping_List == null)
            {
                return HttpNotFound();
            }

            TempData["ID"] = id;
            return RedirectToAction("Index","ListItems");
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
        public ActionResult Create(ShoppingListCreate model)
        {

            if (!ModelState.IsValid) return View(model);

            var service = CreateService();

            if (service.CreateItem(model))
            {
                TempData["SaveResult"] = "Your list was successfully created!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your list could not be created.");
            return RedirectToAction("Index");
        }

        // GET: ShoppingList/Edit/5
        public ActionResult Edit(int id)
        {
            var service = CreateService();
            var detail = service.GetNoteById(id);

            var model =
                new ShoppingListEdit
                {
                    Shopping_ListID = detail.Shopping_ListID,
                    Name = detail.Name,
                    Color = detail.Color,
                };
            return View(model);
        }

        // POST: ShoppingList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ShoppingListEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.Shopping_ListID != id)
            {
                ModelState.AddModelError("", "ID Mismatch");
                return View(model);
            }

            var service = CreateService();


            if (service.UpdateItem(model))
            {
                TempData["SaveResult"] = "Your list was successfully updated!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your list could not be updated.");

            return View(model);
        }

        // GET: ShoppingList/Delete/5
        public ActionResult Delete(int id)
        {
            var service = CreateService();
            var model = service.GetNoteById(id);
            return View(model);
        }

        // POST: ShoppingList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var service = CreateService();

            if (service.DeleteList(id))
            {
                TempData["SaveResult"] = "Your list was successfully deleted.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your list could not be deleted!");


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

        private ShoppingListService CreateService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ShoppingListService(userId);
            return service;
        }

    }
}
