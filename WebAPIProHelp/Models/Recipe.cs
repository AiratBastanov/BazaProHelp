using System;
using System.Collections.Generic;

namespace WebAPIProHelp.Models
{
    public partial class Recipe
    {
        public Recipe()
        {
            ConsumedDishes = new HashSet<ConsumedDish>();
        }

        public int RecipeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public byte[]? Image { get; set; }

        public virtual ICollection<ConsumedDish> ConsumedDishes { get; set; }
    }
}
