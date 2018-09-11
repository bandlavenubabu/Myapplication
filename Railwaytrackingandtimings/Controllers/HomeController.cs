using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Railwaytrackingandtimings.Repositery;
using System.Web.Security;
using Railwaytrackingandtimings.Business_Layer;
using Railwaytrackingandtimings.Models;
using System.Configuration;

namespace Railwaytrackingandtimings.Controllers
{
    public class HomeController : Controller
    {
        IUserRepositery repositery = new UserRepositery();
        ITrainRepositery trepositery = new TrainRepositery();
        ITrain btrain = new Train();
        IUsersManagment ium = new UsersManagement();
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                var std = trepositery.Gettrainstationlist(Convert.ToInt32(id));
                if (std.Count > 0)
                {
                    ViewBag.hdnTrainid = id;
                    int delay = Convert.ToInt32(std.Where(a => a.Delay != null).FirstOrDefault().Delay);
                    string s = btrain.UpcomingStations(Convert.ToInt32(id));
                    ViewBag.stationame = s;
                    ViewBag.Delay = delay;
                    TimeSpan arrival = std.Where(a => a.StationName == s).FirstOrDefault().ArrivalTime;
                    if (arrival < DateTime.Now.TimeOfDay)
                        ViewBag.Arrivaltime = "Reached Endpoint";
                    else
                        ViewBag.Arrivaltime = arrival;
                    if (delay != 0)
                    {
                        TimeSpan ts = TimeSpan.FromMinutes(delay);
                        ts = ts + arrival;                       
                        if (ts < DateTime.Now.TimeOfDay)
                            ViewBag.Arrivaltime = "Reached Endpoint";
                        else
                            ViewBag.Arrivaltime = ts;
                        //ViewBag.Arrivaltime = ts;
                    }
                   
                }
                return View(std);
            }
            //if (Request.QueryString["code"] != null)
            //{
            //    IUsersManagment ium = new UsersManagement();
            //    string trainid = Request.Form["hdntrianid"].ToString();
            //    string redirection_url = "http://localhost:50602/Home/Index";
            //    bool result = ium.GetToken(Request.QueryString["code"].ToString(), redirection_url);
            //    if (result)
            //        ViewBag.Sucess = "Notification Sent Sucessfully";
            //    else
            //        ViewBag.OauthErr = "Failed to login Please try again.";


            //}
            ViewBag.Title = "Home Page";

            return View();
        }
        [HttpPost]
        public ActionResult Index(string txttrainid)
        {
            if (!string.IsNullOrEmpty(txttrainid))
            {
                
               var std = trepositery.Gettrainstationlist(Convert.ToInt32(txttrainid));
                if (std.Count > 0)
                {
                    int delay = Convert.ToInt32(std.Where(a => a.Delay != null).FirstOrDefault().Delay);
                    string s = btrain.UpcomingStations(Convert.ToInt32(txttrainid));
                    ViewBag.stationame = s;
                    ViewBag.Delay = delay;
                    ViewBag.hdnTrainid = txttrainid;
                    TimeSpan arrival= std.Where(a => a.StationName  == s).FirstOrDefault().ArrivalTime;
                    if (arrival < DateTime.Now.TimeOfDay)
                        ViewBag.Arrivaltime = "Reached Endpoint";
                    else
                        ViewBag.Arrivaltime = arrival;
                    if (delay != 0)
                    {
                        TimeSpan ts = TimeSpan.FromMinutes(delay);
                        ts = ts + arrival;
                        if (ts < DateTime.Now.TimeOfDay)
                            ViewBag.Arrivaltime = "Reached Endpoint";
                        else
                            ViewBag.Arrivaltime = ts;
                        //ViewBag.Arrivaltime = ts;
                    }
                }
                if (std.Count == 0)
                    ViewBag.Error = "No Trains Found.";
                return View(std);
            }
            else {
                ViewBag.Error = "Please provide the Train No.";
            }
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
       
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tblUser user)
        {
            if (!string.IsNullOrEmpty(user.UserId) && !string.IsNullOrEmpty(user.Password))
            {
                var userobj = repositery.SelectUserByID(user.UserId, user.Password);
                if (userobj != null)
                {
                    FormsAuthentication.SetAuthCookie(user.UserId, false);
                    Session["UserName"] = userobj.FirstName.ToString() + "" + userobj.LastName;
                    if (userobj.Role == 1)
                        return RedirectToAction("Index", "Admin");
                    else if (userobj.Role == 3)
                        return RedirectToAction("Index", "SM");
                    else
                        return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "User Not exists");
                }
            }
            else {
                ModelState.AddModelError(string.Empty, "UserId and password should not be blank");
            }
            return View (user);
        }
        public ActionResult Trainsbystation()
        {
            ViewBag.stations = trepositery.getStations();
            return View();
        }
        [HttpPost]
        public ActionResult Trainsbystation(FormCollection form)
        {
            string station= form["ddlstation"].ToString();
            var std = trepositery.GettrainstationlistbyStation(station);
            if (std.Count == 0)
                ViewBag.Error = "No Trains Found.";
            ViewBag.stations = trepositery.getStations();
            return View(std);
        }
        public ActionResult Track(FormCollection form)
        {
            
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult Share(FormCollection form)
        {
            
            string  trainid = form["hdntrianid"].ToString();
            string Emailid = form["txtEmailid"].ToString();
            string Delay = form["hdnDelay"].ToString();
            string Arrivaltime = form["hdnarrivaltime"].ToString();
            string NextStation = form["hdnNextstat"].ToString();
            ium.Sendemail(Emailid, trainid, Delay, Arrivaltime, NextStation);
            return RedirectToAction("Index", new { id = trainid });
        }
        public ActionResult Notification()
        {
            Userclass user = new Userclass();
            string rootpath = System.Configuration.ConfigurationManager.AppSettings["rooturl"].ToString();
            if (Request.QueryString["code"] != null)
            {
               
                string redirection_url = rootpath+"Home/Notification";
                user = ium.GetToken(Request.QueryString["code"].ToString(), redirection_url);
                if(!ium.Checkuser(user.id))
                { 
                ium.AddsocalUser(user);
                }
                ViewBag.Id = user.id;
            }
            else {
                string clientid = "18165402201-i4hq1sqdt8h4ftph4kf647l551jqp2tu.apps.googleusercontent.com";
                //your client secret  
                string clientsecret = "Q_yDKwN32V0lgA9mW6edEvyA";              
                //your redirection url  
                string redirection_url = rootpath+"Home/Notification";
                string url = "https://accounts.google.com/o/oauth2/v2/auth?scope=profile&include_granted_scopes=true&redirect_uri=" + redirection_url + "&response_type=code&client_id=" + clientid + "";
                Response.Redirect(url);
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult Notification(FormCollection form)
        {
            string userid = form["hdnuserid"].ToString();
            int trainid =Convert.ToInt32(form["trainid"].ToString());
            string emailid = form["txtEmail"].ToString();
            int result = ium.AddNotification(userid, trainid, emailid);
            if(result>0)
                ViewBag.Status = "Notifications has been initiated";
            return View();
        }

        public ActionResult FeedBack()
        {
            ViewBag.stations = trepositery.getStations();
            return View();
        }
        [HttpPost]
        public ActionResult FeedBack(TblRating  tbl)
        {
            int std = trepositery.Addrating(tbl);
            if (std > 0)
                ViewBag.Status = "FeedBack Submited Sucessfully";
            ViewBag.stations = trepositery.getStations();
            return View();
        }


    }
}
