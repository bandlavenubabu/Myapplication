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
            if(std.Count !=0)
            ViewBag.stationname = trepositery.GettrainstationlistbyStation(stationcode).FirstOrDefault().StationName;
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
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult Feedback()
        {
            string userid = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.stationcode = repositery.SelectUserStation(userid).StationCode;
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection form)
        {
            string Oldpass = form["Oldpassword"].ToString();
            string newpass = form["Newpassword"].ToString();
            string cnfpass = form["cnfpassword"].ToString();
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
                else
                {
                    if (newpass != cnfpass)
                    {
                        ModelState.AddModelError(string.Empty, "New Password and confirm Password should be the same");
                    }
                    else
                    {
                        int result = repositery.Updateuserpassword(userobj, newpass);
                        if (result > 0)
                        {
                            ViewBag.Status = "User Details Updated successfully.";
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Unable to change password.please check with admin");
                        }

                    }
                }
            }

            return View();
        }
    }
}