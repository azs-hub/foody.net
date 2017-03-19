using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foodynet_webapp.Models
{
    public class PaginatedList
    {
        public int Current { get; set; }

        public int Total { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
