using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DropDownBoxLinkage.DBOperator;
using DropDownBoxLinkage.Models;

namespace DropDownBoxLinkage.Controllers
{
    public class ProvinceController : Controller
    {
        private AddressHelper db;

        public ProvinceController()
        {
            db = new AddressHelper();
        }

        public ActionResult Index()
        {
            List<Province> list = db.GetAllProvince();
            ViewBag.ListProvince = list;
            return View();
        }

        [HttpPost]
        public JsonResult GetAllCityByProvinceID(int id)
        {
            List<City> cityList = db.GetCityListByProvinceID(id);
            return Json(cityList, JsonRequestBehavior.AllowGet);
        }
    }
}