using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodeManikinBridge.Models;
using System.Diagnostics;
using System.ComponentModel;

namespace NodeManikinBridge.Controllers
{
    public class set_respiratory_rateController : ApiController
    {
        // GET: api/set_respiratory_rate
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/set_respiratory_rate/5
        public string Get(int id)
        {
            LaerdalServerModel lsModelObj = new LaerdalServerModel();

            int goalHR = id;
            lsModelObj.UpdateHeartRate(goalHR);

            System.Threading.Thread.Sleep(500);
            while (lsModelObj.HeartRate < goalHR)
            {
                lsModelObj.UpdateHeartRate(goalHR);
                System.Threading.Thread.Sleep(500);
            }
            lsModelObj.PauseEvent.Value = false;
            return lsModelObj.HeartRate.ToString();
        }

        // POST: api/set_respiratory_rate
        public async Task<string> Post(HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            LaerdalServerModel lsModelObj = new LaerdalServerModel();
            Boolean isConnected = lsModelObj.Check();

            JObject postData = JObject.Parse(request.Content.ReadAsStringAsync().Result);
            var rr_rate = postData["rate"].ToString();
            int resp_rate = Int32.Parse(rr_rate);
            if (isConnected)
            {
                bool rr_acheived = lsModelObj.UpdateRespiratoryRate(resp_rate);
                if (rr_acheived)
                {
                    return "Respiratory Rate updated";
                }
                else
                {
                    return "Was not able to set respiratory rate to: " + resp_rate;
                }

            }
            else
            {
                string error;
                string address = "10.100.2.166";
                Boolean connected = lsModelObj.Connect(address, out error);
                System.Threading.Thread.Sleep(500);

                lsModelObj.SetupParameters();
                System.Diagnostics.Debug.Write(connected, "connected");
                System.Threading.Thread.Sleep(500);

                bool rr_acheived = lsModelObj.UpdateRespiratoryRate(resp_rate);
                if (rr_acheived)
                {
                    return "Respiratory Rate updated";
                }
                else
                {
                    return "Was not able to set respiratory rate to: " + resp_rate;
                }
            }
        }
        // PUT: api/set_respiratory_rate/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/set_respiratory_rate/5
        public void Delete(int id)
        {
        }
    }
}
