using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NodeManikinBridge.Models;
using System.Diagnostics;

namespace NodeManikinBridge.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            LaerdalServerModel lsModelObj = new LaerdalServerModel();
            string error;

            string address = "10.0.0.135";
            Boolean connected = lsModelObj.Connect(address, out error);
            Boolean check = lsModelObj.Check();
            System.Diagnostics.Debug.Write(connected, "note");
            System.Diagnostics.Debug.Write(check, "check");
            return new string[] { "Sucessfully invoked Laerdal simulator"};
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
