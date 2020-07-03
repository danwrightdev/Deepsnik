using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using BusinessLayer;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace DeepsnikImportExport.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IConfiguration _configuration;

        public FileUploadController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index(List<IFormFile> files)
        {
            string connectionString = _configuration.GetConnectionString("dbConnectionString");

            if (files.Count == 0)
                return RedirectToAction("Index", "Home");

            foreach (var formFile in files)
            {
                using (Stream s = formFile.OpenReadStream())
                {
                    XDocument XMLDocument = XDocument.Load(s);
                    var xmlProcessor = new ImportEmailXMLFile(XMLDocument);
                    var processOrder = new SalesOrdersProcessor(xmlProcessor);

                    var salesOrders = processOrder.GetSalesXMLOrders();

                    var salesProcessor = new DeepsnikSalesOrderSave(connectionString);
                    var processSalesOrderSave = new SalesOrdersSaveProcessor(salesProcessor);

                    processSalesOrderSave.RegisterSalesOrder(salesOrders.First());
                }
            }

            return RedirectToAction("Index", "Home", new { completed = true });
        }
    }
}
