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
    public class ShoppingListCreate
    {

        [Required]
        public Guid OwnerId { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public DateTimeOffset CreatedUtc { get; set; }
    }
}
