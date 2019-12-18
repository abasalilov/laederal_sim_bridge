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
    public class trigger_asthmaController : ApiController
    {
        // POST: api/trigger_asthma

        public async Task<string> Post(HttpRequestMessage request)
        {
            String[] scenarios = { "worsen", "severe", "albuterol_intervention", "mild" };
            HttpResponseMessage response = new HttpResponseMessage();
            LaerdalServerModel lsModelObj = new LaerdalServerModel();
            Boolean isConnected = lsModelObj.Check();
            JObject postData = JObject.Parse(request.Content.ReadAsStringAsync().Result);
            var intensity = postData["intensity"].ToString();
            if (isConnected)
            {
                if (intensity == scenarios[0])
                {
                    lsModelObj.trigger_state_asthma_worsen();
                    return "triggered worsen";
                }
                if (intensity == scenarios[1])
                {
                    lsModelObj.trigger_state_severe_asthma();
                    return "triggered severe";
                }
                if (intensity == scenarios[2])
                {
                    lsModelObj.trigger_intervention();
                    return "triggered intervention";
                }
                if (intensity == scenarios[3])
                {
                    lsModelObj.trigger_mild_asthma();
                    return "trigger mild";
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
                if (intensity == scenarios[0])
                {
                    lsModelObj.trigger_state_asthma_worsen();
                    return "triggered worsen";
                }
                if (intensity == scenarios[1])
                {
                    lsModelObj.trigger_state_severe_asthma();
                    return "triggered severe";
                }
                if (intensity == scenarios[2])
                {
                    lsModelObj.trigger_intervention();
                    return "triggered intervention";
                }
                if (intensity == scenarios[3])
                {
                    lsModelObj.trigger_mild_asthma();
                    return "trigger mild";
                }
                return "Was not able to find command: " + intensity;

            }

        }
    }
}


public class MyTrigger
{
    public string intensity { get; set; }
}
