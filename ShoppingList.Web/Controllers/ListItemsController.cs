using Microsoft.AspNet.Identity;
using PagedList;
using ShoppingList.Data;
using ShoppingList.Data.Models;
using ShoppingList.Models;
using ShoppingList.Services;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ShoppingList.Web.Controllers
{
    [Authorize]
    public class ListItemsController : Controller
    {
        private ShoppingListDbContext db = new ShoppingListDbContext();
        private int? ID;

        // GET: ListItems
        // GET: Customer
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            if (TempData.ContainsKey("ID"))
            {
                ID = TempData["ID"] as int?;
            }
            //else
            //{
            //    ID = 0;
            //}

            var service = CreateService();

            var items = service.GetItems();


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

            TempData["ID"] = ID;

            return View(items.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "ShoppingList");
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
                TempData["SaveResult2"] = "Your item was successfully created!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your item could not be created.");

            TempData["ID"] = ID;

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

            TempData["ID"] = ID;

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
                TempData["SaveResult2"] = "Your list item was successfully updated!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your list item could not be updated.");

            ID = TempData["ID"] as int?;

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

            ID = TempData["ID"] as int?;

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

            ID = TempData["ID"] as int?;
            var userId = Guid.Parse(User.Identity.GetUserId());
            var listId = (int)ID;
            var service = new ShoppingListItemService(userId, listId);
            return service;
        }
    }
}
