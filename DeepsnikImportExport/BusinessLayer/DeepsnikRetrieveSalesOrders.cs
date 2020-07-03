using BusinessLayer.Factories;
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class DeepsnikRetrieveSalesOrders : IRetrieveSalesOrders
    {
        string _ConnectionString;
        public DeepsnikRetrieveSalesOrders(string ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        //Retrieve All Sales Orders
        public List<SalesOrderBaseModel> RetrieveSalesOrders()
        {
            using (var connection = ConnectionFactory.GetConnection(_ConnectionString))
            {
                var sqlAllOrders = @"SELECT Id, AccountReference, SalesOrderDate, VatInclusive
                                     FROM dbo.SalesOrder";

                return connection.Query<SalesOrderBaseModel>(sqlAllOrders).ToList();
            }
        }


        //Retrieve Sales Order by Id from Database
        public SalesOrderModel RetrieveSalesOrderById(int Id)
        {
            using (var connection = ConnectionFactory.GetConnection(_ConnectionString))
            {
                var sqlAllOrders = @"SELECT Id, AccountReference, SalesOrderDate, VatInclusive, SalesOrderAddressId, SalesOrderDeliveryAddressId
                                     FROM dbo.SalesOrder
                                     WHERE Id = @Id";

                SalesOrderModel model = connection.Query<SalesOrderModel>(sqlAllOrders, new { Id }).FirstOrDefault();

                model.SalesOrderAddress = GetAddress(connection, model.SalesOrderAddressId);
                model.SalesOrderDeliveryAddress = GetAddress(connection, model.SalesOrderDeliveryAddressId);

                var sqlSalesItems = @"SELECT Sku, QtyOrdered, UnitPrice
                                      FROM dbo.SalesOrderItem
                                      WHERE OrderId = @Id";

                model.SalesOrderItems = connection.Query<SalesOrderItemModel>(sqlSalesItems, new { Id }).ToList();

                return model;
            }
        }

        //Get address' from database
        private AddressModel GetAddress(IDbConnection connection, int addressId)
        {
            string sqlString = @"SELECT Title, Forename, Surname, Company, Address1, Address2, Town, Postcode, County
                                 FROM dbo.[Address]
                                 WHERE Id = @Id;";

            AddressModel Model = connection.Query<AddressModel>(sqlString, new { Id = addressId }).FirstOrDefault();

            return Model;
        }
    }
}
