using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class SalesOrderItemModel
    {
        public SalesOrderItemModel()
        {

        }

        public SalesOrderItemModel(string sku, int? qtyOrdered, decimal? unitPrice)
        {
            Sku = sku;
            QtyOrdered = qtyOrdered;
            UnitPrice = unitPrice;
        }

        public string Sku { get; set; }
        public int? QtyOrdered { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
