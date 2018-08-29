using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Railwaytrackingandtimings.Models
{
    public class TrainStationDetailsmodel
    {
      public  TemptblStationTrainDetail TemptblStationTrainDetail { get; set; }
        public IEnumerable<TemptblStationTrainDetail> TemptblStationTrainDetaillist { get; set; }
    }
}