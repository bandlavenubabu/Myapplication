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
    }
}