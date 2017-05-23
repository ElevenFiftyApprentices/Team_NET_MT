using ShoppingList.Data;
using ShoppingList.Data.Models;
using ShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                                    Name = e.Name,
                                    Color = e.Color
                                }
                            );
                return query.ToArray();
            }

        }

        public ShoppingListEdit GetNoteById(int shoppingListItemID)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var entity =
                    ctx
                    .ShoppingLists
                    .Single(e => e.OwnerId == _userId);


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
                    .ShoppingLists.Single(e => e.OwnerId == _userId);
                entity.Name = model.Name;
                entity.Color = model.Color;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
