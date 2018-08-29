using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Railwaytrackingandtimings.Repositery;

namespace Railwaytrackingandtimings.Business_Layer
{
    public class Train : ITrain
    {
        ITrainRepositery repositery = new TrainRepositery();
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
    }
}