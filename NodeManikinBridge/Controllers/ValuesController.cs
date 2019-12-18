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
            // Instantiate Model
            LaerdalServerModel lsModelObj = new LaerdalServerModel();

            // Initialize parameters
            System.Threading.Thread.Sleep(300);

            lsModelObj.SetupParameters();

            //Boolean check = lsModelObj.Check();
           // System.Diagnostics.Debug.Write(check, "check");
            //TODO: GET FROM SESSION, DON'T MAKE NEW CONNECTION
                string error;
                string address = "192.168.0.134";
                Boolean connected = lsModelObj.Connect(address, out error);
                if (connected)
                {
                    System.Diagnostics.Debug.Write(connected, "note");
                    lsModelObj.PulseOximeter.Value = 1;
                }
            System.Diagnostics.Debug.Write(lsModelObj.PulseOximeter.Value, "PulseOximeter");
            
            return new string[] { "Sucessfully invoked Laerdal simulator" };

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
