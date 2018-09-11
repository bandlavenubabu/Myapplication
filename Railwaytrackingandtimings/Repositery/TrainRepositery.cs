using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Railwaytrackingandtimings.Repositery
{
    public class TrainRepositery : ITrainRepositery
    {
        private RailwaytrackingEntitieslatest db = null;
        public TrainRepositery()
        {
            this.db = new RailwaytrackingEntitieslatest();
        }

        public int AddNewuser(tblUser user, tblUserStation tsu)
        {
            db.tblUsers.Add(user);
            db.tblUserStations.Add(tsu);
            return db.SaveChanges();
        }

        public int Addrating(TblRating rating)
        {
            db.TblRatings.Add(rating);
            return db.SaveChanges();
        }

        public int ADDstationtraindetails(TemptblStationTrainDetail tstd)
        {
            db.TemptblStationTrainDetails.Add(tstd);
            return db.SaveChanges();
        }

        public int AddTrain(tblTrainDetail ttd)
        {
            db.tblTrainDetails.Add(ttd);
            return db.SaveChanges();
        }

        public int AddTrainStation(StationTrainDetail std)
        {
            db.StationTrainDetails.Add(std);
            return db.SaveChanges();
        }

        public int DeleteTempTrainStation(int Trainid)
        {
            db.TemptblStationTrainDetails.RemoveRange(db.TemptblStationTrainDetails.Where(x => x.TrainId == Trainid));
            return db.SaveChanges();
        }

        public tblStationDetail GetStationName(string Stationcode)
        {
            return db.tblStationDetails.Where(a => a.StationCode == Stationcode).FirstOrDefault();
        }

        public List<tblStationDetail> getStations()
        {
            return db.tblStationDetails.Where(a => a.Status == "Y").ToList();
        }

        public List<TemptblStationTrainDetail> GetTemptrainstation(int Trainid)
        {
            return db.TemptblStationTrainDetails.Where(a => a.TrainId == Trainid).ToList();
        }

        public List<tblTrainDetail> GetTrainDetails()
        {
            return db.tblTrainDetails.Where(a => a.Status == "Y").ToList();
        }

        public List<StationTrainDetail> Gettrainstationlist(int Trainid)
        {
            return db.StationTrainDetails.Where(a => a.TrainId == Trainid && a.Status == "Y" && a.SchduleDate >= DateTime.Today).ToList();
        }

        public List<StationTrainDetail> GettrainstationlistbyStation(string StationCode)
        {
            return db.StationTrainDetails.Where(a => a.StationCode == StationCode && a.Status == "Y" && a.SchduleDate >= DateTime.Today).ToList();
        }

        public int Updatestationtraindetails(StationTrainDetail std)
        {
            int i = 0;
            var result = db.StationTrainDetails.SingleOrDefault(b => b.Id == std.Id);
            var delayset = db.StationTrainDetails.SingleOrDefault(b => b.Delay != null && b.TrainId == std.TrainId && b.SchduleDate == result.SchduleDate);
            if (delayset != null && result != null)
            {
                delayset.Delay = null;
                db.SaveChanges();
            }
            if (result != null)
            {
                result.ArrivalTime = std.ArrivalTime;
                result.DepatureTime = std.DepatureTime;
                result.Delay = std.Delay;
                i = db.SaveChanges();
            }
            return i;
        }
    }

}