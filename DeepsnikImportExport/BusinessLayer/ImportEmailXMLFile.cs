using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace BusinessLayer
{
    public class ImportEmailXMLFile : IRetrieveXMLOrders
    {
        private XDocument _XMLDocument;
        private string _connectionString;

        public ImportEmailXMLFile(XDocument XMLDocument)
        {
            _XMLDocument = XMLDocument;
        }

        /// <summary>
        /// Processes Xml file and stores Sales Order in account database
        /// </summary>
        /// <returns></returns>
        public List<SalesOrderModel> ProcessXMLOrders()
        {

            var salesOrders = _XMLDocument.Descendants("SalesOrder")
                              .Select(n =>
                                new SalesOrderModel
                                {
                                    Id = int.Parse(n.Element("Id")?.Value),
                                    AccountReference = n.Element("AccountReference")?.Value ?? "",
                                    VatInclusive = bool.Parse(n.Element("VatInclusive")?.Value),
                                    SalesOrderDate = DateTime.Parse(n.Element("SalesOrderDate")?.Value),
                                    SalesOrderAddress = n.Descendants("SalesOrderAddress")
                                    .Select(oa => new AddressModel(
                                        oa.Element("Title")?.Value ?? "",
                                        oa.Element("Forename")?.Value ?? "",
                                        oa.Element("Surname")?.Value ?? "",
                                        oa.Element("Company")?.Value ?? "",
                                        oa.Element("Address1")?.Value ?? "",
                                        oa.Element("Address2")?.Value ?? "",
                                        oa.Element("Town")?.Value ?? "",
                                        oa.Element("Postcode")?.Value ?? "",
                                        oa.Element("County")?.Value ?? "")
                                    ).FirstOrDefault(),
                                    SalesOrderDeliveryAddress = n.Descendants("SalesOrderAddress")
                                    .Select(da => new AddressModel(
                                        da.Element("Title")?.Value ?? "",
                                        da.Element("Forename")?.Value ?? "",
                                        da.Element("Surname")?.Value ?? "",
                                        da.Element("Company")?.Value ?? "",
                                        da.Element("Address1")?.Value ?? "",
                                        da.Element("Address2")?.Value ?? "",
                                        da.Element("Town")?.Value ?? "",
                                        da.Element("Postcode")?.Value ?? "",
                                        da.Element("County")?.Value ?? "")
                                    ).FirstOrDefault(),
                                    SalesOrderItems = n.Descendants("SalesOrderItems").Descendants("Item")
                                                      .Select(i => new SalesOrderItemModel(
                                                          i.Element("Sku")?.Value ?? "",
                                                          int.Parse(i.Element("QtyOrdered")?.Value),
                                                          decimal.Parse(i.Element("UnitPrice")?.Value)
                                                      )).ToList()
                                })
                            .ToList();

            return salesOrders;
        }
    }
}
