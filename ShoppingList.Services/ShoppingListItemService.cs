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
    public class ShoppingListItemService
    {
        private readonly Guid _userId;

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
                    Contents = model.Contents,
                    Priority = model.Priority,
                    Note = model.Note,
                    IsChecked = false,

                };

            using (var ctx = new ShoppingListDbContext())
            {
                ctx.ShoppingListItems.Add(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
