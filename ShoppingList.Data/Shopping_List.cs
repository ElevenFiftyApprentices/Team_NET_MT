using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Data
{
    public class Shopping_List
    {
        [Key]
        public int Shopping_ListID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Color { get; set; }

        [Required]
        public DateTimeOffset CreatedUtc { get; set; }

        public DateTimeOffset? ModidiedUtc { get; set; }

    }
}
