using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Railwaytrackingandtimings.Repositery;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

using System.Text;

namespace Railwaytrackingandtimings.Business_Layer
{
    public class Train : ITrain
    {
        ITrainRepositery repositery = new TrainRepositery();

        public int AddStation(tblStationDetail station)
        {
            station.CreatedDate = DateTime.Now;
            station.LastUpdated = DateTime.Now;
            station.Status = "Y";
            return repositery.AddStation(station);
        }

        public bool AddTrainStaSchedule(int delay, int Trainid)
        {
            List<TemptblStationTrainDetail> tempdetails = new List<TemptblStationTrainDetail>();
            List<StationTrainDetail> stdetails = new List<StationTrainDetail>();
            try
            {
                tempdetails = repositery.GetTemptrainstation(Trainid);
                StationTrainDetail std = new StationTrainDetail();
                for (int i = 0; i < tempdetails.Count; i++)
                {
                    if (i == 0)
                    { std.Delay = delay; }
                    else { std.Delay = null; }
                    std.TrainId = tempdetails[i].TrainId;
                    std.Status = tempdetails[i].Status;
                    std.StationCode = tempdetails[i].StationCode;
                    std.StationName = tempdetails[i].StationName;
                    std.SchduleDate = tempdetails[i].SchduleDate;
                    std.DepatureTime = tempdetails[i].DepatureTime;
                    std.ArrivalTime = tempdetails[i].ArrivalTime;
                    repositery.AddTrainStation(std);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public string UpcomingStations(int TrainID)
        {
            List<StationTrainDetail> std;
            std = repositery.Gettrainstationlist(TrainID);
            //var schedules = new List<Schedule>()
            //{
            //    new  Schedule() { ScheduleTime = new TimeSpan(6,00,00)},
            //    new  Schedule() { ScheduleTime = new TimeSpan(7,00,00)},
            //    new  Schedule() { ScheduleTime = new TimeSpan(8,00,00)},
            //};

            var currentTime = DateTime.Now.TimeOfDay;
            string stationname= std
                .OrderBy(x => Math.Abs(currentTime.Ticks - x.ArrivalTime.Ticks))
                .FirstOrDefault().StationName;
            return stationname;
        }

       
    }
    
}