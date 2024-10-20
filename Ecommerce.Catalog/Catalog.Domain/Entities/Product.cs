using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }  
        public string Sku { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductCategory { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
