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
    public class trigger_stateController : ApiController
    {

        // POST: api/trigger_state
        public async Task<string> Post(HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            LaerdalServerModel lsModelObj = new LaerdalServerModel();
            Boolean isConnected = lsModelObj.Check();
            JObject postData = JObject.Parse(request.Content.ReadAsStringAsync().Result);
            var intensity = postData["intensity"].ToString();
            if (isConnected)
            {
                if (intensity == "healthy")
                {
                    //lsModelObj.trigger_healthy_state();
                    return "triggered healthy";
                }
                return "Was not able to find command: " + intensity;
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
                if (intensity == "healthy")
                {
                    //lsModelObj.trigger_healthy_state();
                    return "triggered healthy";
                }

                return "Was not able to find command: " + intensity;

            }

        }
    }
}
