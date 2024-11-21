using System;
using System.Collections.Generic;

namespace WebAPIProHelp.Models
{
    public partial class ConsumedDish
    {
        public int DishId { get; set; }
        public int UserId { get; set; }
        public int? RecipeId { get; set; }
        public double Quantity { get; set; }
        public string Name { get; set; } = null!;
        public double ProteinsPer100g { get; set; }
        public double CarbsPer100g { get; set; }
        public double FatsPer100g { get; set; }
        public double CaloriesPer100g { get; set; }

        public virtual Recipe? Recipe { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
