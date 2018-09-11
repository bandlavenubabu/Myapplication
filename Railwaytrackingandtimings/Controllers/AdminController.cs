using Railwaytrackingandtimings.Business_Layer;
using Railwaytrackingandtimings.Models;
using Railwaytrackingandtimings.Repositery;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Railwaytrackingandtimings.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        ITrainRepositery repositery = new TrainRepositery();
        ITrain Btrain = new Train();
        [Authorize]
        public ActionResult Index()
        {
            var traindetails = repositery.GetTrainDetails();
            return View(traindetails);
        }
        [Authorize]
        public ActionResult NewTrain()
        {
            ViewBag.stations = repositery.getStations();
            return View();
        }
        [HttpPost]
        public ActionResult NewTrain(tblTrainDetail ttd)
        {
            if (ModelState.IsValid)
            {
                ttd.CreatedDate = DateTime.Now;
                ttd.LastUpdatedDate = DateTime.Now;
                ttd.Status = "Y";
                int status = repositery.AddTrain(ttd);
                if (status > 0)
                {
                    ViewBag.Status = "Train Details added successfully.";
                    ttd = new tblTrainDetail();
                }
                else { ViewBag.Status = "Error. please try Agin"; }
            }
            ViewBag.stations = repositery.getStations();
            return View(ttd);
        }
        public ActionResult TrainSchedule(int id)
        {
            ViewBag.stations = repositery.getStations();
            TrainStationDetailsmodel tsdm  = new TrainStationDetailsmodel();            
            tsdm.TemptblStationTrainDetaillist =repositery.GetTemptrainstation(id);
            ViewBag.TrainId = id;
            return View(tsdm);
        }
        [HttpPost]
        public ActionResult TrainSchedule(TrainStationDetailsmodel tstd)
        {
            ViewBag.stations = repositery.getStations();
            ViewBag.TrainId = tstd.TemptblStationTrainDetail.TrainId;
            TrainStationDetailsmodel tsdm = new TrainStationDetailsmodel();
            if (ModelState.IsValid)
            {
                tblStationDetail tsd = new tblStationDetail();
                tsd = repositery.GetStationName(tstd.TemptblStationTrainDetail.StationCode);
                tstd.TemptblStationTrainDetail.StationName = tsd.StationName;
                tstd.TemptblStationTrainDetail.Status = "Y";
                TemptblStationTrainDetail temp = new TemptblStationTrainDetail();
                temp = tstd.TemptblStationTrainDetail;
                repositery.ADDstationtraindetails(temp);
                tsdm.TemptblStationTrainDetaillist = repositery.GetTemptrainstation(tstd.TemptblStationTrainDetail.TrainId);
                ViewBag.TrainId = tstd.TemptblStationTrainDetail.TrainId;
                return View(tsdm);

            }
            return View(tstd);
        }
        public ActionResult SavetrainStation( )
        {
            int time = Convert.ToInt32(Request["Delayeditor"].ToString());
            int TrainId = Convert.ToInt32(Request["hdntrianid"].ToString());
            bool status = Btrain.AddTrainStaSchedule(time, TrainId);
            if (status)
                repositery.DeleteTempTrainStation(TrainId);
            return RedirectToAction("Index","Admin");
        }
        public ActionResult NewUser()
        {
            ViewBag.stations = repositery.getStations();
            return View();
        }
        [HttpPost]
        public ActionResult NewUser(tblUser user, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                string station = form["ddlstation"].ToString();
                user.Status = "Y";
                user.CreatedDate = DateTime.Now;
                user.LastUpdateDate = DateTime.Now;
                user.Role = 3;
                tblUserStation tus = new tblUserStation();
                tus.UserId = user.UserId;
                tus.StationCode = station;
               int status= repositery.AddNewuser(user, tus);
                if (status > 0)
                {
                    ViewBag.Status = "User Details added successfully.";
                 }
            }
            ViewBag.stations = repositery.getStations();
            return View();
        }
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection form)
        {
            string Oldpass = form["Oldpassword"].ToString();
            string newpass = form["Newpassword"].ToString();
            string cnfpass= form["cnfpassword"].ToString();
            if (string.IsNullOrEmpty(Oldpass) || string.IsNullOrEmpty(newpass) || string.IsNullOrEmpty(cnfpass))
            {
                ModelState.AddModelError(string.Empty, "Please fill all the details.");
            }
            else
            {
                string strCurrentUserId = User.Identity.Name;
                IUserRepositery repositery = new UserRepositery();
                var userobj = repositery.SelectUserByID(strCurrentUserId, Oldpass);
                if (userobj == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Old Password");
                }
                else{
                    if (newpass != cnfpass)
                    {
                        ModelState.AddModelError(string.Empty, "New Password and confirm Password should be the same");
                    }
                    else {
                        int result=repositery.Updateuserpassword(userobj, newpass);
                        if (result > 0)
                        {
                            ViewBag.Status = "User Details Updated successfully.";
                        }
                        else {
                            ModelState.AddModelError(string.Empty, "Unable to change password.please check with admin");
                        }

                    }
                }
            }
                  
            return View();
        }
    }
}