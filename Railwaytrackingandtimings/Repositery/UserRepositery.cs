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
        public tblUser SelectUserByID(string id, string Password)
        {
            return db.tblUsers.Where(a => a.UserId== id && a.Password == Password).FirstOrDefault();
        }

        public tblUserStation SelectUserStation(string userid)
        {
            return db.tblUserStations.Where(a => a.UserId == userid).FirstOrDefault();
        }
    }
}