using Saned.ArousQatar.Data.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Saned.ArousQatar.Data.Persistence;
using Saned.ArousQatar.Front.Properties;

namespace Saned.ArousQatar.Front.Controllers
{
    public class AdvertisementController : Controller
    {
        private IUnitOfWork unitOfWork;

        [Route("Advertisement")]
        public ActionResult Index(int id)
        {
            ViewBag.ApiUrl=Settings.Default.ApiUrl;
            ViewBag.PlayStoreUrl = Settings.Default.PlayStoreUrl;
            ViewBag.ApplePlayStore = Settings.Default.ApplePlayStore;
            unitOfWork=new UnitOfWork();
           var advertisement= unitOfWork.Advertisements.GetSingleData(id);
            if (advertisement == null)
            {
                return HttpNotFound();
            }

            return View(advertisement);
        }



    }
}