using ShoppingList.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Models
{
    public class ShoppingItemEdit
    {
        public int ShoppingListItemID { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Please enter at least two characters.")]
        [MaxLength(128)]
        public string Contents { get; set; }

        [Required]
        [DefaultValue(Priorities.ItCanWait)]
        public Priorities Priority { get; set; }

        [MaxLength(8000)]
        public string Note { get; set; }

        public bool IsChecked { get; set; }
    }
}
