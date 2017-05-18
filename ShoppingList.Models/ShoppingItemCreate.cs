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
    public class ShoppingItemCreate
    {
        //private Guid userId;

        //public ShoppingItemCreate(Guid userId)
        //{
        //    this.userId = userId;
        //}

        [Required]
        [MinLength(2, ErrorMessage = "Please enter at least two characters.")]
        [MaxLength(128)]
        public string Contents { get; set; }

        [Required]
        [DefaultValue(Priorities.ItCanWait)]
        public Priorities Priority { get; set; }

        [MaxLength(8000)]
        public string Note { get; set; }
    }
}
