using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Data
{
    public class Note
    {
        public int NoteID { get; set; }

        public int ShoppingListItemID { get; set; }

        public string Body { get; set; }

        public DateTimeOffset CreatedUtc { get; set; }

        public DateTimeOffset ModidiedUtc { get; set; }
    }
}
