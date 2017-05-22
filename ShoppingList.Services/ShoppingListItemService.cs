using ShoppingList.Data;
using System.Data.Entity;
using ShoppingList.Data.Models;
using ShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using ShoppingList.Services;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Services
{
    public class ShoppingListItemService
    {
        private readonly Guid _userId;

        private ShoppingListDbContext db = new ShoppingListDbContext();

        public ShoppingListItemService(Guid userId)
        {
            _userId = userId;
        }
        //TODO: Grab the Items from Database
        public bool CreateItem(ShoppingItemCreate model)
        {
            var entity =
                new ShoppingListItem
                {
                    OwnerId = _userId,
                    Contents = model.Contents,
                    Priority = model.Priority,
                    Note = model.Note,
                    IsChecked = false,
                    CreatedUtc = DateTimeOffset.UtcNow
                };

            using (var ctx = new ShoppingListDbContext())
            {
                ctx.ShoppingListItems.Add(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<ShoppingItemIndex> GetItems()
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var query =
                    ctx
                        .ShoppingListItems
                        .Where(e => e.OwnerId == _userId)
                        .Select(
                            e =>
                                new ShoppingItemIndex
                                {
                                    ShoppingListItemID = e.ShoppingListItemID,
                                    Note = e.Note,
                                    IsChecked = e.IsChecked,
                                    Contents = e.Contents,
                                    Priority = e.Priority
                                }
                            );
                return query.ToArray();
            }

        }

        public ShoppingItemEdit GetNoteById(int shoppingListItemID)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var entity =
                    ctx
                    .ShoppingListItems
                    .Single(e => e.ShoppingListItemID == shoppingListItemID && e.OwnerId == _userId);


                return
                    new ShoppingItemEdit
                    {
                        ShoppingListItemID = entity.ShoppingListItemID,
                        Contents = entity.Contents,
                        Priority = entity.Priority,
                        Note = entity.Note,
                        IsChecked = entity.IsChecked,
                        ModifiedUtc = entity.ModifiedUtc
                    };
            }
        }

        public bool UpdateItem(ShoppingItemEdit model)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var entity =
                    ctx
                    .ShoppingListItems.Single(e => e.ShoppingListItemID == model.ShoppingListItemID && e.OwnerId == _userId);
                entity.Contents = model.Contents;
                entity.Priority = model.Priority;
                entity.Note = model.Note;
                entity.IsChecked = model.IsChecked;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
