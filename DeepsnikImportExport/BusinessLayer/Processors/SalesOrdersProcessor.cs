using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace BusinessLayer
{
    public class SalesOrdersProcessor
    {
        IRetrieveXMLOrders _retrieveXmlOrders;
        public SalesOrdersProcessor(IRetrieveXMLOrders retrieveXmlOrders)
        {
            _retrieveXmlOrders = retrieveXmlOrders;
        }

        public List<SalesOrderModel> GetSalesXMLOrders()
        {
            return _retrieveXmlOrders.ProcessXMLOrders();
        }
    }
}
