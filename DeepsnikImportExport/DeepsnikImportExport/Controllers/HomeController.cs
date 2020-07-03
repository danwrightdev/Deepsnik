using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BusinessLayer;
using Microsoft.AspNetCore.Http;
using System.IO;
using BusinessLayer.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace DeepsnikImportExport.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(bool? completed = false)
        {
            return View(completed);
        }
    }
}
