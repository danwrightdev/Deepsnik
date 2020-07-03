using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer
{
    public interface IRetrieveXMLOrders
    {
        public List<SalesOrderModel> ProcessXMLOrders();
    }
}
