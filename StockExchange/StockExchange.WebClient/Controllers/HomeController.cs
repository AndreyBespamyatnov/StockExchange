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

            StockContainer.Instance.AddStockToUser(stockCode);
            return View("Index");
        }  

        [HttpPost]
        public JsonResult RemoveStockFromUser(string stockCode)
        {
            if (!AuthenticationContainer.Instance.IsAuthenticated)
            {
                return new JsonResult();
            }

            var stocks = StockContainer.Instance.RemoveStockFromUser(stockCode);
            return Json(stocks, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetUserStocks()
        {
            if (!AuthenticationContainer.Instance.IsAuthenticated)
            {
                return new JsonResult();
            }

            var stocks = StockContainer.Instance.GetUserStocks();
            return Json(stocks, JsonRequestBehavior.AllowGet);
        }  
    }
}