using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService sellerService;

        public SellersController(SellerService service)
        {
            sellerService = service;
        }

        public IActionResult Index()
        {
            var list = sellerService.FindAll();
            return View(list);
        }
    }
}
