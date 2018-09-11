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
        int Updateuserpassword(tblUser user, string newpassword);

        int Adduser(tblUser user);
        tblUser Checkuser(string id);
        int AddNotification(tblNotification notification);
        int AddEmailNotification(tblEmailNotification notification);

    }
}
