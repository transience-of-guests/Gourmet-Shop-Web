using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GourmetShop.DataAccess.Models
{
    public class ProductViewModel
    {
        
            public IEnumerable<Product> Products { get; set; }
            public IEnumerable<Subcategory> Subcategories { get; set; }
        
    }
}
