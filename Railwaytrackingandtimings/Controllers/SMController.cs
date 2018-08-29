using Railwaytrackingandtimings.Repositery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Railwaytrackingandtimings.Controllers
{
    public class SMController : Controller
    {
        IUserRepositery repositery = new UserRepositery();
        ITrainRepositery trepositery = new TrainRepositery();
        //private object trepositery;

        // GET: SM
        [Authorize]
        public ActionResult Index()
        {
            string userid= FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            string stationcode = repositery.SelectUserStation(userid).StationCode;            
            var std = trepositery.GettrainstationlistbyStation(stationcode);
            return View(std);
        }
        public ActionResult Trainupdate(int id)
        {
            string userid = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            string stationcode = repositery.SelectUserStation(userid).StationCode;
            var std = trepositery.GettrainstationlistbyStation(stationcode).Where(a=>a.Id == id).FirstOrDefault();
            return View(std);
        }
        [HttpPost]
        public ActionResult Trainupdate(StationTrainDetail std)
        {
            int i = trepositery.Updatestationtraindetails(std);
            if (i > 0)
                ViewBag.Status = "Train No" + std.TrainId + "has been updated successfully";
            return View(std);
        }
    }
}