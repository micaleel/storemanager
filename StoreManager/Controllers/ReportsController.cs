﻿using System;
using System.Linq;
using System.Web.Mvc;
using StoreManager.Infrastructure;

namespace StoreManager.Controllers {

    [AuthorizeAndRedirect]
    public class ReportsController : BaseController {

        public ActionResult Index() {
            return View();
        }

        public ActionResult RecentMovements() {
            var movements = Db.Movements.OrderBy(x => x.DateCreated).Take(50).ToList();
            return View(movements);
        }

        public ActionResult OutOfStockItems() {
            var stocks = Db.Stocks.Where(x => !x.Location.IsStore).ToList();
            return View(stocks);
        }


        public ActionResult StoreInventory() {
            var stocks = Db.Stocks.Where(x => x.Location.IsStore).ToList();
            return View(stocks);
        }

        public ActionResult GeneralInventory() {
            var stocks = Db.Stocks.ToList();
            return View(stocks);
        }

        public ActionResult ExpiredItems() {
            var stocks = Db.Stocks.Where(x => x.ExpiryDate <= DateTime.UtcNow).ToList();
            return View(stocks);
        }
    }
}
