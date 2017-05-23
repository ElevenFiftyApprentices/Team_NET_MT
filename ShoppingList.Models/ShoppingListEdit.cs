using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Models
{
    public class ShoppingListEdit
    {
        public int Shopping_ListID { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public DateTimeOffset? ModifiedUtc { get; set; }


    }
}
