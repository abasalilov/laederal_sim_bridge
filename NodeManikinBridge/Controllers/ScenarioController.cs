using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NodeManikinBridge.Models;
using System.Diagnostics;
using System.ComponentModel;

namespace NodeManikinBridge.Controllers
{
    public class ScenarioController : ApiController
    {

        string current = "C:/Users/aleks_windows/Laerdal Simulation/Other files/Profiles/currently_used_profile.ricfg";
        string simMan3g = "C:/Users/aleks_windows/Laerdal Simulation/Other files/ProfilesSimMan3G Trauma Module 1.ricfg";
        string Asthma = "C:/Users/aleks_windows/Laerdal Simulation/Themes/Adult/Respiration/Asthma.thx";
        string Health = "C:/Users/aleks_windows/Laerdal Simulation/Themes/Health patient.thx";
        string Cardiac = "C:/Users/aleks_windows/Laerdal Simulation/Themes/Adult/Circulation/Cardiac Arrest.thx";

        // GET: api/Scenario
        public IEnumerable<string> Get()
        {

            LaerdalServerModel lsModelObj = new LaerdalServerModel();
            Boolean isConnected = lsModelObj.Check();
            //TODO: GET FROM SESSION, DON'T MAKE NEW CONNECTION
            if (!isConnected)
            {
                string error;
                string address = "10.0.0.136";
                Boolean connected = lsModelObj.Connect(address, out error);
                System.Diagnostics.Debug.Write(connected, "note");
            }

            bool scenarioIsActivated = lsModelObj.UploadScenario(Asthma, current);
            var result = scenarioIsActivated ? "Scenario has been Activated" : "Scenario not activated";
            return new string[] { "scenarioIsActivated" };
        }

        // GET: api/Scenario/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Scenario
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Scenario/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Scenario/5
        public void Delete(int id)
        {
        }
    }
}
