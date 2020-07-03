using BusinessLayer.Models;
using BusinessLayer.Factories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BusinessLayer.Interfaces;

namespace BusinessLayer
{
    public class DeepsnikSalesOrderSave: ISalesOrderSave
    {
        string _ConnectionString;

        public DeepsnikSalesOrderSave(string ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        private class SalesAddressIds
        {
            public int SalesOrderAddressId { get; set; }
            public int SalesOrderDeliveryAddressId { get; set; }
        }

        //Register sales in accounts database
        public bool RegisterSalesOrder(SalesOrderModel salesOrder)
        {
            string sqlOrderDetails = "SELECT Count(Id) FROM SalesOrder WHERE Id = @Id;";
            using (var connection = ConnectionFactory.GetConnection(_ConnectionString))
            {
                var orderCount = connection.Query<int>(sqlOrderDetails, new { salesOrder.Id }).First();

                if(orderCount > 0)
                {
                    UpdateSale(connection, salesOrder);
                }
                else
                {
                    AddSale(connection, salesOrder);
                }
            }
            return true;
        }
        /// <summary>
        /// Handles adding sales orders
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="salesOrder"></param>

        private void AddSale(IDbConnection connection, SalesOrderModel salesOrder)
        {
            string sqlInsertSalesOrder = @"INSERT INTO dbo.SalesOrder(Id, AccountReference, SalesOrderDate, VatInclusive) 
                                           VALUES(@Id, @AccountReference, @SalesOrderDate, @VatInclusive)";

            connection.Execute(sqlInsertSalesOrder, new { salesOrder.Id, salesOrder.AccountReference, salesOrder.SalesOrderDate, salesOrder.VatInclusive });

            string sqlInsertSalesAddress = @"INSERT INTO dbo.[Address](Title, Forename, Surname, Company, Address1, Address2, Town, Postcode, County) 
                                             VALUES(@Title, @Forename, @Surname, @Company, @Address1, @Address2, @Town, @Postcode, @County)
                                             SELECT CAST(SCOPE_IDENTITY() as int)";

            var salesOrderAddressId = connection.QuerySingle<int>(sqlInsertSalesAddress, salesOrder.SalesOrderAddress);
            var salesOrderDeliveryAddressId = connection.QuerySingle<int>(sqlInsertSalesAddress, salesOrder.SalesOrderDeliveryAddress);

            AddSalesItems(connection, salesOrder);

            var sqlUpdateSalesAddress = @"UPDATE dbo.SalesOrder
                                          SET SalesOrderAddressId = @salesOrderAddressId,
                                              SalesOrderDeliveryAddressId = @salesOrderDeliveryAddressId
                                          WHERE Id = @Id";

            connection.Execute(sqlUpdateSalesAddress, new { salesOrderAddressId, salesOrderDeliveryAddressId, salesOrder.Id });
        }

        /// <summary>
        /// Handles updating sales orders
        /// </summary>
        private void UpdateSale(IDbConnection connection, SalesOrderModel salesOrder)
        {
            string sqlOrderAddressDetails = "SELECT SalesOrderAddressId, SalesOrderDeliveryAddressId FROM dbo.SalesOrder where Id = @Id;";

            var salesOrderAddressIds = connection.QuerySingle<SalesAddressIds>(sqlOrderAddressDetails, new { salesOrder.Id });

            UpdateAddress(connection, salesOrder.SalesOrderAddress, salesOrderAddressIds.SalesOrderAddressId);
            UpdateAddress(connection, salesOrder.SalesOrderDeliveryAddress, salesOrderAddressIds.SalesOrderDeliveryAddressId);


            var sqlUpdateSalesOrder = @"UPDATE dbo.SalesOrder
                                        SET AccountReference = @AccountReference, 
                                            SalesOrderDate = @SalesOrderDate, 
                                            VatInclusive = @VatInclusive
                                        WHERE Id = @Id";

            connection.Execute(sqlUpdateSalesOrder, new { salesOrder.AccountReference, salesOrder.SalesOrderDate, salesOrder.VatInclusive, salesOrder.Id });

            ClearSalesItems(connection, salesOrder.Id);

            AddSalesItems(connection, salesOrder);
        }

        /// <summary>
        /// Helper for updating Address data
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="Address"></param>
        /// <param name="addressId"></param>
        private void UpdateAddress(IDbConnection connection, AddressModel Address, int addressId)
        {
            string sqlString = @"UPDATE dbo.[Address]
                                 SET Title = @Title, 
                                     Forename = @Forename, 
                                     Surname = @Surname, 
                                     Company = @Company, 
                                     Address1 = @Address1, 
                                     Address2 = @Address2, 
                                     Town = @Town, 
                                     Postcode = @Postcode, 
                                     County = @County
                                 WHERE Id = @Id;";
            
            connection.Execute(sqlString, new
            {
                Id = addressId,
                Address.Title,
                Address.Forename,
                Address.Surname,
                Address.Company,
                Address.Address1,
                Address.Address2,
                Address.Town,
                Address.Postcode,
                Address.County
            });
        }

        /// <summary>
        /// Helper for clearing previous sales items
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="salesOrderId"></param>
        private void ClearSalesItems(IDbConnection connection, int? salesOrderId)
        {
            var sqlDeleteSalesItems = @"DELETE FROM dbo.SalesOrderItem WHERE OrderId = @Id";

            connection.Execute(sqlDeleteSalesItems, new { Id = salesOrderId });
        }

        /// <summary>
        /// Helper for adding sales items
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="salesOrder"></param>
        private void AddSalesItems(IDbConnection connection, SalesOrderModel salesOrder)
        {
            var sqlInsertSalesItem = @"INSERT INTO dbo.SalesOrderItem(Sku, QtyOrdered, UnitPrice, OrderId) 
                                      VALUES(@Sku, @QtyOrdered, @UnitPrice, @OrderId)";

            foreach (var item in salesOrder.SalesOrderItems)
            {
                connection.Execute(sqlInsertSalesItem, new { item.Sku, item.QtyOrdered, item.UnitPrice, OrderId = salesOrder.Id });
            }
        }
    }
}
