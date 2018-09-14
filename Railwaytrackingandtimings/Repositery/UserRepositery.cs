using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Railwaytrackingandtimings.Repositery
{
    public class UserRepositery : IUserRepositery
    {
        private RailwaytrackingEntitieslatest db = null;

        public UserRepositery()
        {
            this.db = new RailwaytrackingEntitieslatest();
        }

        public int AddEmailNotification(tblEmailNotification notification)
        {
            db.tblEmailNotifications.Add(notification);
            return db.SaveChanges();
        }

        public int AddNotification(tblNotification notification)
        {
            db.tblNotifications.Add(notification);
            return db.SaveChanges();
        }

        public int Adduser(tblUser user)
        {
            db.tblUsers.Add(user);
            return db.SaveChanges();
        }

        public tblUser Checkuser(string id)
        {
            return db.tblUsers.Where(a => a.UserId == id).FirstOrDefault();
        }

        public tblUser SelectUserByID(string id, string Password)
        {
            return db.tblUsers.Where(a => a.UserId== id && a.Password == Password).FirstOrDefault();
        }

        public tblUserStation SelectUserStation(string userid)
        {
            return db.tblUserStations.Where(a => a.UserId == userid).FirstOrDefault();
        }

        public int Updateuserpassword(tblUser user,string newpassword)
        {
            var result = db.tblUsers.SingleOrDefault(b => b.UserId == user.UserId && b.Password==user.Password);
            if(result !=null)
            {
                result.ConfirmPassword = newpassword;
                result.Password = newpassword;
                result.LastUpdateDate = DateTime.Now;
               
            }
            return db.SaveChanges();
        }
        public int Updatenotifications(int id)
        {
            var delayset = db.tblNotifications.SingleOrDefault(b => b.Id == id);
            if (delayset != null)
            {
                delayset.Status = "N";

            }
            return db.SaveChanges();
        }
    }
}