using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LaerdalSimulatorAPI;

namespace NodeManikinBridge.Models
{
    public class LaerdalServerModel
    {
        public IManikin Manikin { get; set; }

        /// <summary>
        /// The name of the manikin which is connected to
        /// </summary>
        public string ManikinName { get; private set; }

        /// <summary>
        /// A manikin parameter which will represent the state of the pulse oximeter.
        /// </summary> 
        public IParameterAppValue<int> Capnometer { get; private set; }
        /// <summary>
        /// A manikin parameter which will represent the state of the .
        /// </summary> 
        public IParameterAppValue<int> PulseOximeter { get; private set; }
        /// <summary>
        /// Pauses the simulation
        /// </summary>
        public IParameterAppEventBool PauseEvent { get; private set; }
        /// <summary>
        /// Starts the simulation
        /// </summary>
        public IParameterAppEventBool StartEvent { get; private set; }

        /// <summary>
        /// Timer which gets the parameters from the server
        /// </summary>
        System.Threading.Timer updateTimer;

        public LaerdalServerModel()
        {        
            // Create the object through which we communicate with the manikin
            Manikin = Tools.CreateManikin();

            // Initialize the object
            Manikin.Initialize();

            Manikin.StartDiscovery();
        }


        public event Action<bool> LostConnection;

        void lostConnection(bool shutdown)
        {
            System.Diagnostics.Debug.Write(shutdown, "shut");
            if (LostConnection != null)
                LostConnection(shutdown);
        }

        /// <summary>
        /// Returns true if this object is already initialized, otherwise false.
        /// </summary>
        public bool AreParametersSetup { get; private set; }

        /// <summary>
        /// Sets up the parameters
        /// </summary>
        public void SetupParameters()
        {
            // Initialize parameters
            Capnometer = Manikin.GetParameterAppValue<int>("canDisplaySaO2");
            PulseOximeter = Manikin.GetParameterAppValue<int>("PulseOximeter");
            PauseEvent = Manikin.GetParameterAppEventBool("Pause Simulation");
            StartEvent = Manikin.GetParameterAppEventBool("Start Simulation");
            updateTimer = new System.Threading.Timer(new System.Threading.TimerCallback(ProcessDataFromServer), this, 0, 100);

            AreParametersSetup = true;
        }


        /// <summary>
        /// Called in order to get the parameter values from the server.
        ///  Update frequency is quite fast, between 10-50 msec, in order to get smooth update of curves (if there are any).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProcessDataFromServer(object sender)
        {
            Manikin.ProcessDataFromServer();
        }


        /// <summary>
        /// Connect to a manikin server
        /// </summary>
        /// <param name="server">The server name</param>
        /// <param name="error">The error output, if the connection fails</param>
        /// <returns>Returns true if the connection succeeded, otherwise false</returns>
        public bool Connect(string server, out string error)
        {
            ManikinName = server;

            // Try to connect to the manikin using the obtained URI
            if (!Manikin.Connect(server, lostConnection))
            {
                // Store the error
                error = String.Format("Unable to connect to server", server);
                return false;
            }
            error = null;
            return true;
        }

        public bool UpateHeartRate(int diastolic, int systolic)
        {
            return true;
        }

        public bool Check()
        {
            return Manikin.CheckConnection();
        }

    }
}