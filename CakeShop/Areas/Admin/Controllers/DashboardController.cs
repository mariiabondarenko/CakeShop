﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CakeShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}