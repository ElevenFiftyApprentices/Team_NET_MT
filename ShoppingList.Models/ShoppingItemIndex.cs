using ShoppingList.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Models
{
    public class ShoppingItemIndex
    {
        public int ShoppingListItemID { get; set; }

        public string Contents { get; set; }

        public Priorities Priority { get; set; }

        public string Note { get; set; }

        [Display(Name = "Checked")]
        public bool IsChecked { get; set; }
    }
}
