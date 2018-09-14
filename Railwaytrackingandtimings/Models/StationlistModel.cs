using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Railwaytrackingandtimings.Models
{
    public class StationlistModel
    {
        public tblStationDetail tblstation { get; set; }
        public IEnumerable<tblStationDetail> tblstationslist { get; set; }
    }
}