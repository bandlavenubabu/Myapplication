using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railwaytrackingandtimings.Business_Layer
{
   public interface ITrain
    {
        bool AddTrainStaSchedule(int delay, int Trainid);
    }
}
