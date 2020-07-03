using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ISalesOrderSave
    {
        public bool RegisterSalesOrder(SalesOrderModel salesOrder);
    }
}
