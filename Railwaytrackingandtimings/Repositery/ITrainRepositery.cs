using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railwaytrackingandtimings.Repositery
{
   public interface ITrainRepositery
    {
        List<tblStationDetail> getStations();
        int AddTrain(tblTrainDetail ttd );
        List<tblTrainDetail> GetTrainDetails();
        tblStationDetail GetStationName(string Stationcode);
        int ADDstationtraindetails(TemptblStationTrainDetail tstd);
        List<TemptblStationTrainDetail> GetTemptrainstation(int Trainid);
        List<StationTrainDetail> Gettrainstationlist(int Trainid);
        int AddTrainStation(StationTrainDetail std);
        List<StationTrainDetail> GettrainstationlistbyStation(string StationCode);
        int Updatestationtraindetails(StationTrainDetail std);
        int DeleteTempTrainStation(int Trainid);
        int AddNewuser(tblUser user, tblUserStation tsu);
        int Addrating(TblRating  rating);

        IEnumerable<Usp_rating_Result> GetFeedback(string StationCode);
        IEnumerable<usp_NotificationsUpdate_Result> Getnotification();

        int AddStation(tblStationDetail station);

    }
}
