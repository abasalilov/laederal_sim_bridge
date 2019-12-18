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
        // GET: api/Scenario
        public IEnumerable<string> Get()
        {

            LaerdalServerModel lsModelObj = new LaerdalServerModel();

            Boolean isConnected = lsModelObj.Check();
            //TODO: GET FROM SESSION, DON'T MAKE NEW CONNECTION
            if (!isConnected)
            {
                string error;
                string address = "10.100.2.166";
                Boolean connected = lsModelObj.Connect(address, out error);
                System.Threading.Thread.Sleep(500);

                lsModelObj.SetupParameters();
                System.Diagnostics.Debug.Write(connected, "connected");
                System.Threading.Thread.Sleep(500);
                lsModelObj.trigger_state_severe_asthma();

                System.Threading.Thread.Sleep(500);
  
                lsModelObj.PauseEvent.Value = false;
                var result = "Scenario has been Activated" ;
                System.Diagnostics.Debug.Write(result, "phase 1 initiated");

                return new string[] { result };
            }
            else
            {

            System.Threading.Thread.Sleep(500);
            lsModelObj.trigger_state_severe_asthma();

            System.Threading.Thread.Sleep(500);

            lsModelObj.PauseEvent.Value = false;
            var result = "Scenario has been Activated";
            System.Diagnostics.Debug.Write(result, "phase 1 initiated");

            return new string[] { result };
            }
     
        }

        // GET: api/Scenario/5
        public IEnumerable<string> Get(int id)
        {

            string[] scenarios = new string[] { "trigger_state_severe_asthma", "trigger_state_asthma_worsen", "trigger_intervention", "trigger_mild_asthma", "trigger_healthy_state" };
            LaerdalServerModel lsModelObj = new LaerdalServerModel();

            Boolean isConnected = lsModelObj.Check();
            //TODO: GET FROM SESSION, DON'T MAKE NEW CONNECTION
            if (!isConnected)
            {
                string error;
                string address = "10.100.2.166";
                Boolean connected = lsModelObj.Connect(address, out error);
                System.Threading.Thread.Sleep(500);

                lsModelObj.SetupParameters();
                System.Diagnostics.Debug.Write(connected, "connected");
                System.Threading.Thread.Sleep(500);
                if (id == 1)
                {
                    lsModelObj.trigger_state_severe_asthma();
                }
                if (id == 2)
                {
                    lsModelObj.trigger_state_asthma_worsen();
                }
                if (id == 3)
                {
                    lsModelObj.trigger_intervention();
                }
                if (id == 4)
                {
                    lsModelObj.trigger_mild_asthma();
                }
                if (id == 5)
                {
                    lsModelObj.trigger_healthy_state();
                }
                

                System.Threading.Thread.Sleep(500);

                lsModelObj.PauseEvent.Value = false;
                var result = "Scenario function " + scenarios[id - 1] + " been Activated";
                System.Diagnostics.Debug.Write(result, "result");

                return new string[] { result };
            }
            else
            {

                System.Threading.Thread.Sleep(500);
                if (id == 1)
                {
                    lsModelObj.trigger_state_severe_asthma();
                }
                if (id == 2)
                {
                    lsModelObj.trigger_state_asthma_worsen();
                }
                if (id == 3)
                {
                    lsModelObj.trigger_intervention();
                }
                if (id == 4)
                {
                    lsModelObj.trigger_mild_asthma();
                }
                if (id == 5)
                {
                    lsModelObj.trigger_healthy_state();
                }
                
                System.Threading.Thread.Sleep(500);

                var result = "Scenario function " + scenarios[id - 1] + " been Activated";
                System.Diagnostics.Debug.Write(result, "result");

                return new string[] { result };
            }
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
