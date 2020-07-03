using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IRetrieveSalesOrders
    {
        public List<SalesOrderBaseModel> RetrieveSalesOrders();

        public SalesOrderModel RetrieveSalesOrderById(int Id);
    }
}
