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
    public class set_open_eyesController : ApiController
    {
        // GET: api/set_open_eyes
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/set_open_eyes/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/set_open_eyes
        public async Task<string> Post(HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            LaerdalServerModel lsModelObj = new LaerdalServerModel();
            Boolean isConnected = lsModelObj.Check();

            JObject postData = JObject.Parse(request.Content.ReadAsStringAsync().Result);
            var eye_status = postData["open"].ToString();
            if (isConnected)
            {
                bool eye = lsModelObj.UpdateEyeStatus(eye_status == "true");
                if (eye)
                {
                    return "Eye lid status updated";
                }
                else
                {
                    return "Was not able to update eye lid status";
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

                bool eye = lsModelObj.UpdateEyeStatus(eye_status == "true");
                if (eye)
                {
                    return "Eye lid status updated";
                }
                else
                {
                    return "Was not able to update eye lid status";
                }

            }
        }
        // PUT: api/set_open_eyes/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/set_open_eyes/5
        public void Delete(int id)
        {
        }
    }
}
