using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Railwaytrackingandtimings.Repositery;
using System.Web.Security;

namespace Railwaytrackingandtimings.Controllers
{
    public class HomeController : Controller
    {
        IUserRepositery repositery = new UserRepositery();
        ITrainRepositery trepositery = new TrainRepositery();
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                var std = trepositery.Gettrainstationlist(Convert.ToInt32(id));
                ViewBag.Delay = std.Where(a => a.Delay != null).FirstOrDefault().Delay;
                return View(std);
            }
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
                    ViewBag.Delay = std.Where(a => a.Delay != null).FirstOrDefault().Delay;
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
            if (ModelState.IsValid)
            {
               var userobj = repositery.SelectUserByID(user.UserId,user.Password);
                if (userobj != null)
                {
                    FormsAuthentication.SetAuthCookie(user.UserId, false);
                    Session["UserName"] = userobj.FirstName.ToString()+""+ userobj.LastName;
                    if (userobj.Role==1)
                    return RedirectToAction("Index","Admin");
                    else if(userobj.Role == 3)
                     return RedirectToAction("Index", "SM");
                    else
                     return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("User", "User Not exists");
                }
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
    }
}
