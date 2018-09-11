using Railwaytrackingandtimings.Models;
using Railwaytrackingandtimings.Repositery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Railwaytrackingandtimings.Business_Layer
{
    public class UsersManagement:IUsersManagment
    {
        IUserRepositery repo = new UserRepositery();

        public int AddNotification(string userid, int trainid, string emailid)
        {
            tblNotification notifi = new tblNotification();
            notifi.UserId = userid;
            notifi.EmailId = emailid;
            notifi.CreatedDate = DateTime.Now;
            notifi.Status = "Y";
            notifi.TrainId = trainid;
            return repo.AddNotification(notifi);
        }

        public int AddsocalUser(Userclass Userclass)
        {
            tblUser user = new tblUser();
            user.UserId = Userclass.id;
            user.Password = "Test12345";
            user.ConfirmPassword = "Test12345";
            user.Role = 5;
            user.FirstName = Userclass.given_name;
            user.LastName = Userclass.family_name;
            user.CreatedDate = DateTime.Now;
            user.LastUpdateDate = DateTime.Now;
            user.Status = "Y";
            return repo.Adduser(user);
        }

        public bool Checkuser(string id)
        {
            tblUser user = repo.Checkuser(id);
            if (user != null)
                return true;
            else
                return false;
        }

        public Userclass GetToken(string code,string redirection_url)
        {
            //your client id
            string clientid = "18165402201-i4hq1sqdt8h4ftph4kf647l551jqp2tu.apps.googleusercontent.com";

            //your client secret  
            string clientsecret = "Q_yDKwN32V0lgA9mW6edEvyA";
            //your redirection url  
            //string redirection_url = "http://localhost:50602/Home/Notification";
            string url = "https://accounts.google.com/o/oauth2/token";
            string poststring = "grant_type=authorization_code&code=" + code + "&client_id=" + clientid + "&client_secret=" + clientsecret + "&redirect_uri=" + redirection_url + "";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            UTF8Encoding utfenc = new UTF8Encoding();
            byte[] bytes = utfenc.GetBytes(poststring);
            Stream outputstream = null;
            try
            {
                request.ContentLength = bytes.Length;
                outputstream = request.GetRequestStream();
                outputstream.Write(bytes, 0, bytes.Length);
            }
            catch { }
            var response = (HttpWebResponse)request.GetResponse();
            var streamReader = new StreamReader(response.GetResponseStream());
            string responseFromServer = streamReader.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            var obj = js.Deserialize<Tokenclass>(responseFromServer);
            Userclass uc = GetuserProfile(obj.access_token);
            return uc;

        }
        public Userclass GetuserProfile(string accesstoken)
        {
            string url = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + accesstoken + "";
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            response.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            Userclass userinfo = js.Deserialize<Userclass>(responseFromServer);
            //Userc.ImageUrl = userinfo.picture;
            //Userc.id = userinfo.id;
            //Userc.gender = userinfo.gender;
            //Userc.locale = userinfo.locale;
            //Userc.name = userinfo.name;
            //Userc.link = userinfo.link;
            return userinfo;
        }
        public int Sendemail(string email,string trainno,string delay,string arrival,string nextstationn)
        {
            tblEmailNotification notification = new tblEmailNotification();
            notification.EmailTo = email;           
            string htmlString = @"<html><body><p>Dear User,</p><p>Thank you for visiting our site,please find the below train details</p><p>TrainNo:"+trainno+"</p><p>Delay:"+delay+"Mins</p><p>Expected Arrival:"+arrival+"</p><p>Next Station:"+nextstationn+"</p><p>Sincerely,<br>-Railway Tracking System</br></p></body></html>";
            notification.Message = htmlString;
            notification.CreatedDate = DateTime.Now;
            return repo.AddEmailNotification(notification);
        }
    }
}