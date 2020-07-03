using BusinessLayer;
using BusinessLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SalesUnitTests
{
    [TestClass]
    public class SalesOrderUnitTests
    {
        public List<SalesOrderModel> _mockSalesData = new List<SalesOrderModel>();

        public SalesOrderUnitTests()
        {
            var SalesOrder = new SalesOrderModel
            {
                Id = 123,
                AccountReference = "JOE001",
                SalesOrderDate = DateTime.Parse("2014-01-01 00:00:00"),
                VatInclusive = true,
                SalesOrderAddress = new AddressModel
                {
                    Title = "Mr",
                    Forename = "Joe",
                    Surname = "Harrison",
                    Company = "Deepsnik Software Limited",
                    Address1 = "Business Centre",
                    Address2 = "Renishaw",
                    Town = "Sheffield",
                    Postcode = "S21 3WY",
                    County = "South Yorkshire"
                },
                SalesOrderDeliveryAddress = new AddressModel
                {
                    Title = "Mr",
                    Forename = "Joe",
                    Surname = "Harrison",
                    Company = "Deepsnik Software Limited",
                    Address1 = "Business Centre",
                    Address2 = "Renishaw",
                    Town = "Sheffield",
                    Postcode = "S21 3WY",
                    County = "South Yorkshire"
                },
                SalesOrderItems = new List<SalesOrderItemModel>
                {
                    new SalesOrderItemModel
                    {
                        Sku = "TEST01",
                        QtyOrdered = 1,
                        UnitPrice = 200
                    },
                    new SalesOrderItemModel
                    {
                        Sku = "TEST02",
                        QtyOrdered = 1,
                        UnitPrice = 300
                    }
                }
            };

            _mockSalesData.Add(SalesOrder);
        }

        [TestMethod]
        public void XMLProcessTest()
        {
            XDocument XMLDocument = XDocument.Load(@"Xml\SalesOrder.xml");
            var xmlProcessor = new ImportEmailXMLFile(XMLDocument);
            var processOrder = new BusinessLayer.SalesOrdersProcessor(xmlProcessor);

            var salesOrders = processOrder.GetSalesXMLOrders();

            var JSONMock = JsonConvert.SerializeObject(_mockSalesData);
            var JSONSales = JsonConvert.SerializeObject(salesOrders);

            Assert.AreEqual(JSONMock, JSONSales);
        }
    }
}
