using Railwaytrackingandtimings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railwaytrackingandtimings.Business_Layer
{
    interface IUsersManagment
    {
        Userclass GetToken(string code, string redirection_url);
        int AddsocalUser(Userclass Userclass);

        bool Checkuser(string id);
        int AddNotification( string userid,int trainid,string emailid);
        int Sendemail(string email, string trainno, string delay, string arrival, string nextstationn);
    }
}
