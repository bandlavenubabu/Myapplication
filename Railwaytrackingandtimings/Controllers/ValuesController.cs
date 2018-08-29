using Railwaytrackingandtimings.Business_Layer;
using Railwaytrackingandtimings.Repositery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Railwaytrackingandtimings.Controllers
{
    public class ValuesController : ApiController
    {
        ITrainRepositery repositery = new TrainRepositery();
        ITrain Btrain = new Train();

        // GET api/values
        public HttpResponseMessage GetTrainsList()
        {
          var traindetails = repositery.GetTrainDetails();
            if (traindetails == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, traindetails);
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
