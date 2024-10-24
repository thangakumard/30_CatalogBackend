﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public int  PurchaseQuantity { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
