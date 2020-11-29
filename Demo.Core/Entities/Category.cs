using Demo.Core.Entities.Base;
using System.Collections.Generic;

namespace Demo.Core.Entities
{
    public class Category : Entity
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }        
        public ICollection<Product> Products { get; private set; }
    }
}
