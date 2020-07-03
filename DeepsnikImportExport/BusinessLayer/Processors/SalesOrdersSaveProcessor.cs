using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace BusinessLayer
{
    public class SalesOrdersSaveProcessor
    {
        ISalesOrderSave _salesOrderSave;
        public SalesOrdersSaveProcessor(ISalesOrderSave salesOrderSave)
        {
            _salesOrderSave = salesOrderSave;
        }

        public bool RegisterSalesOrder(SalesOrderModel salesOrder)
        {
            return _salesOrderSave.RegisterSalesOrder(salesOrder);
        }
    }
}
