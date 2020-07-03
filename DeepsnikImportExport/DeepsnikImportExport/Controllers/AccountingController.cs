using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DeepsnikImportExport.Controllers
{
    public class AccountingController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AccountingController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("dbConnectionString");
        }
        public IActionResult Index()
        {
            var salesRetrieveProcessor = new DeepsnikRetrieveSalesOrders(_connectionString);
            var processSalesRetrieveOrder = new SalesOrdersRetrieveProcessor(salesRetrieveProcessor);

            var salesOrders = processSalesRetrieveOrder.RetrieveSalesOrders();
            return View(salesOrders);
        }

        public IActionResult ViewSalesOrder(int Id)
        {
            var salesRetrieveProcessor = new DeepsnikRetrieveSalesOrders(_connectionString);
            var processSalesRetrieveOrder = new SalesOrdersRetrieveProcessor(salesRetrieveProcessor);

            var salesOrder = processSalesRetrieveOrder.RetrieveSalesOrderById(Id);
            return View(salesOrder);
        }
    }
}
