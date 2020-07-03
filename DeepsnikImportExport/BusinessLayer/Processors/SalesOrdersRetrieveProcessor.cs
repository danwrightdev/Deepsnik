using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace BusinessLayer
{
    public class SalesOrdersRetrieveProcessor
    {
        IRetrieveSalesOrders _retrieveSalesOrders;
        public SalesOrdersRetrieveProcessor(IRetrieveSalesOrders retrieveSalesOrders)
        {
            _retrieveSalesOrders = retrieveSalesOrders;
        }

        public List<SalesOrderBaseModel> RetrieveSalesOrders()
        {
            return _retrieveSalesOrders.RetrieveSalesOrders();
        }

        public SalesOrderModel RetrieveSalesOrderById(int Id)
        {
            return _retrieveSalesOrders.RetrieveSalesOrderById(Id);
        }
    }
}
