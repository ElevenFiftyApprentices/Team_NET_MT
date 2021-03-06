﻿using ShoppingList.Data;
using ShoppingList.Data.Models;
using ShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Services
{
    public class ShoppingListService
    {

        private readonly Guid _userId;

        private ShoppingListDbContext db = new ShoppingListDbContext();

        public ShoppingListService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateItem(ShoppingListCreate model)
        {
            var entity =
                new Shopping_List
                { 
                    OwnerId = _userId,
                    Name = model.Name,
                    Color = model.Color,
                    CreatedUtc = DateTimeOffset.UtcNow
                };

            using (var ctx = new ShoppingListDbContext())
            {
                ctx.ShoppingLists.Add(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<ShoppingListIndex> GetLists()
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var query =
                    ctx
                        .ShoppingLists
                        .Where(e => e.OwnerId == _userId)
                        .Select(
                            e =>
                                new ShoppingListIndex
                                {
                                    Shopping_ListID = e.Shopping_ListID,
                                    Name = e.Name,
                                    Color = e.Color
                                }
                            );
                return query.ToArray();
            }

        }

        public ShoppingListEdit GetNoteById(int shoppingListID)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var entity =
                    ctx
                    .ShoppingLists
                    .Single(e => e.OwnerId == _userId && e.Shopping_ListID == shoppingListID);


                return
                    new ShoppingListEdit
                    {
                        Shopping_ListID = entity.Shopping_ListID,
                        Name = entity.Name,
                        Color = entity.Color,
                        ModifiedUtc = entity.ModifiedUtc
                    };
            }
        }

        public bool UpdateItem(ShoppingListEdit model)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var entity =
                    ctx
                    .ShoppingLists.Single(e => e.OwnerId == _userId && e.Shopping_ListID == model.Shopping_ListID);
                entity.Name = model.Name;
                entity.Color = model.Color;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteList(int listId)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var entity =
                    ctx
                    .ShoppingLists.Single(e => e.Shopping_ListID == listId && e.OwnerId == _userId);

                ctx.ShoppingLists.Remove(entity);

                db.ShoppingListItems.RemoveRange(
                    db
                    .ShoppingListItems
                    .Where(i => i.Shopping_ListID == listId)
                    );

                var listItems =
                    ctx
                    .ShoppingListItems
                    .Where(i => i.Shopping_ListID == listId);

                ctx.ShoppingListItems.RemoveRange(listItems);


                return ctx.SaveChanges() < 1;

            }

        }
    }
}
