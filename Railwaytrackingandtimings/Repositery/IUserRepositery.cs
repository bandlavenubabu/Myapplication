using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railwaytrackingandtimings.Repositery
{
    public interface IUserRepositery
    {
        tblUser SelectUserByID(string id,string Password);
        tblUserStation SelectUserStation(string userid);

    }
}
