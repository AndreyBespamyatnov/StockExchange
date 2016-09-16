using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockExchange.WebClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!AuthenticationContainer.Instance.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Title="Stock Exchange Web Client";
            return View();
        }

        [HttpPost]
        public JsonResult Stocks(string prefix)
        {
            if (!AuthenticationContainer.Instance.IsAuthenticated)
            {
                return new JsonResult();
            }

            var stocks = StockContainer.Instance.GetStocks(prefix);
            return Json(stocks, JsonRequestBehavior.AllowGet);
        } 

        [HttpPost]
        public ActionResult AddStockToUser(string stockCode)
        {
            if (!AuthenticationContainer.Instance.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var stocks = StockContainer.Instance.AddStockToUser(stockCode);
            return View("Index");
        }  
    }
}