using System;
using System.Linq;
using System.Web.Mvc;
using DialogueStore.Web.Infrastructure;
using DialogueStore.Web.Models;

namespace DialogueStore.Web.Controllers
{
    [AuthorizeAndRedirect]
    public class ReportsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RecentMovements()
        {
            var movements = Db.Movements.OrderBy(x => x.DateCreated).Take(50).ToList();
            return View(movements);
        }

        public ActionResult OutOfStockItems()
        {
            var stocks = Db.Stocks.Where(x => !x.Location.IsStore).ToList();
            return View(stocks);
        }


        public ActionResult StoreInventory(string format = "")
        {
            var stocks = Db.Stocks.Where(x => x.Location.IsStore).ToList();

            switch (format)
            {
                case "csv": return View(stocks);
                case "pdf": return View(stocks);
                default: return View(stocks);
            }
        }

        public FileResult DownloadCsv()
        {
            var columnHeaders = new[] { "A" };
            var rows = Enumerable.Empty<Stock>();
            Func<Stock, string> stockToCsv = stock => String.Format("{0}", stock.Item.Name);

            return DownloadCsvImpl(columnHeaders, rows, stockToCsv, "StoreInventory.csv");
        }

        public ActionResult GeneralInventory()
        {
            var stocks = Db.Stocks.ToList();
            return View(stocks);
        }

        public ActionResult ExpiredItems()
        {
            var stocks = Db.Stocks.Where(x => x.ExpiryDate <= DateTime.UtcNow).ToList();
            return View(stocks);
        }
    }
}
