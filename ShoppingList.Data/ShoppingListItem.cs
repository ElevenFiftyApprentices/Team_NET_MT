using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Data
{
    public enum Priorities
    {
        ItCanWait,
        NeedItSoon,
        GrabItNow
    }

    public class ShoppingListItem
    {
        [Key]       
        public int ShoppingListItemID { get; set; }

        public int ShoppingListID { get; set; }

        public Guid OwnerId { get; set; }

        [Required]
        public string Contents { get; set; }

        [Required]
        [DefaultValue(Priorities.ItCanWait)]
        public Priorities Priority { get; set; }

        public string Note { get; set; }

        [DefaultValue(false)]
        public bool IsChecked { get; set; }

        [Required]
        public DateTimeOffset CreatedUtc { get; set; }

        public DateTimeOffset? ModifiedUtc { get; set; }


    }
}
