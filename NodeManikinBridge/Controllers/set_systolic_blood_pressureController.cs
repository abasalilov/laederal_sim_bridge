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
    public class set_systolic_blood_pressureController : ApiController
    {
        // POST: api/set_diastolic_blood_pressure
        public async Task<string> Post(HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            LaerdalServerModel lsModelObj = new LaerdalServerModel();
            Boolean isConnected = lsModelObj.Check();

            JObject postData = JObject.Parse(request.Content.ReadAsStringAsync().Result);
            var pressure_sys = postData["pressure"].ToString();
            int sys = Int32.Parse(pressure_sys);
            if (isConnected)
            {
                bool bp = lsModelObj.UpdateSYS_BP(sys);
                if (bp)
                {
                    return "Blood pressure updated";
                }
                else
                {
                    return "Was not able to set blood pressure to: " + "sys:" + sys;
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

                bool bp = lsModelObj.UpdateSYS_BP(sys);
                if (bp)
                {
                    return "Blood pressure updated";
                }
                else
                {
                    return "Was not able to set blood pressure to: " + "sys:" + sys;
                }
            }
        }
    }
}
