using System;
using System.Web.Mvc;
using thingslineWeb.Models;
using thingslineWeb.Modules;
using Microsoft.AspNet.Identity;


namespace thingslineWeb.Controllers
{
    public class MapController : Controller
    {

        public ActionResult Index()
        {
            
           
            
            
            mdlMap mMap = new mdlMap();
            MapViweModel getMapViweModel = new MapViweModel();

            //今日の日付をセット
            getMapViweModel.SearchCndDate = DateTime.Now;
            getMapViweModel.SearchCndDateED = DateTime.Now;
            getMapViweModel.UserID = User.Identity.GetUserId();


            MapViweModel retMapViweModel = mMap.getMapData(getMapViweModel);

            retMapViweModel.SearchCndDate = DateTime.Now;
            retMapViweModel.SearchCndDateED = DateTime.Now;
            ViewBag.retMapViweModel = retMapViweModel;
            //ViewBag.MapData = retMapViweModel.MapData;

            return View();

        }



        [HttpPost]
        public ActionResult Index(MapViweModel getMapViweModel)
        {
            mdlMap mMap = new mdlMap();
            MapViweModel retMapViweModel = new MapViweModel();
            getMapViweModel.UserID = User.Identity.GetUserId();

            if (getMapViweModel.uploadFile != null)
            {
                _ = mMap.UploadImgAsync(getMapViweModel.uploadFile, User.Identity.GetUserId());
            }

            retMapViweModel = mMap.getMapData(getMapViweModel);

            ViewBag.retMapViweModel = retMapViweModel;
            retMapViweModel.SearchCndDate = getMapViweModel.SearchCndDate;
            retMapViweModel.SearchCndDateED = getMapViweModel.SearchCndDateED;


            return View();

        }






        public ActionResult About2()
        {

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }
        public ActionResult Register()
        {

            return View();
        }
    }
}