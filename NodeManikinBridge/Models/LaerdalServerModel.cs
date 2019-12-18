using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ComponentModel;
using System.Threading;
using LaerdalSimulatorAPI;

namespace NodeManikinBridge.Models
{

    public class LaerdalServerModel
    {
        public IManikin Manikin { get; set; }

        public string ServerIP { get; set; }

        public string ManikinName { get; private set; }

        public IParameterAppValue<int> PulseOximeter { get; private set; }
        /// <summary>
        /// Pauses the simulation
        /// </summary>
        public IParameterAppEventBool PauseEvent { get; private set; }
        /// <summary>
        /// Starts the simulation
        /// </summary>
        public IParameterAppEventBool StartEvent { get; private set; }

        public IParameterAppValue<int> SysBloodPressure { get; private set; }

        public IParameterAppValue<int> DiaBloodPressure { get; private set; }

        IParameterModelDouble overrideHeartRate;
        public IParameterModelDouble RespiratoryRate { get; private set; }

        public IParameterModelDouble overrideRespiratoryRate;

        public IParameterModelDouble heartRate { get; private set; }
        public IParameterAppValue<int> Diastolic_mmHg { get; private set; }
        public IParameterAppValue<int> Systolic_mmHg { get; private set; }
        public IParameterAppValue<double> etCO2_Value_mmHg { get; private set; }
        public IParameterAppValue<double> TBlood_Celcius { get; private set; }
        public IParameterAppValue<double> TPeri_Celcius { get; private set; }
        public IParameterModelDouble PCWP { get; private set; }
        public IParameterModelDouble PAPSystolic { get; private set; }
        public IParameterModelDouble PAPDiastolic { get; private set; }
        public IParameterModelEnum EYES { get; private set; }
        public IParameterModelDouble CVP { get; private set; }
        public IParameterAppValue<double> Value_LiterPerMinute { get; private set; }
        public IParameterModelEnum LeftUpperAnteriorLungs { get; private set; }
        public IParameterModelEnum LeftUpperPosteriorLungs { get; private set; }
        public IParameterModelEnum RightUpperAnteriorLungs { get; private set; }
        public IParameterModelEnum RightUpperPosteriorLungs { get; private set; }
        public IParameterModelInteger SVLeftLowerPosteriorLungs { get; private set; }
        public IParameterModelInteger SVLeftLowerAnteriorLungs { get; private set; }
        public IParameterModelInteger SVRightLowerPosteriorLungs { get; private set; }
        public IParameterModelInteger SVRightLowerAnteriorLungs { get; private set; }
        public IParameterModelInteger SVRightUpperPosteriorLungs { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        System.Threading.Timer updateTimer;

        PropertyChangedEventHandler handler = (sender, args) =>
        {
            IParameter p = sender as IParameter;
            string name = p.Name;
            System.Diagnostics.Debug.WriteLine("Changed value of " + name);
        };

        public LaerdalServerModel()
        {        
            // Create the object through which we communicate with the manikin
            Manikin = Tools.CreateManikin();


            // Initialize the object
            Manikin.Initialize();

            Manikin.StartDiscovery();

            //Manikin.
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

        void OnHeartRateChanged(object obj, PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("HeartRate"));
        }

        void OnPauseEvent(object obj, PropertyChangedEventArgs args)
        {
            if (PauseEvent.Value == true)
                PauseResumeText = "Resume session";
            else
                PauseResumeText = "Pause session";
        }


        string pauseResumeText = "(nothing)";
        /// <summary>
        /// Used for showing the pause/resume text on the button
        /// </summary>
        public string PauseResumeText
        {
            get
            {
                return pauseResumeText;
            }
            set
            {
                pauseResumeText = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PauseResumeText"));
            }
        }

 
        /// <summary>
        /// Sets up the parameters
        /// </summary>
        public void SetupParameters()
        {
            // Initialize parameters
            PauseEvent = Manikin.GetParameterAppEventBool("Pause Simulation");
            StartEvent = Manikin.GetParameterAppEventBool("Start Simulation");
            PulseOximeter = Manikin.GetParameterAppValue<int>("PulseOximeter");

            etCO2_Value_mmHg = Manikin.GetParameterAppValue<double>("Laerdal.Response.etCO2.Value_mmHg");
            TBlood_Celcius = Manikin.GetParameterAppValue<double>("Laerdal.Response.Temp.TBlood_Celcius");
            TPeri_Celcius = Manikin.GetParameterAppValue<double>("Laerdal.Response.Temp.TPeri_Celcius");

            heartRate = Manikin.GetParameterModelDouble("heartRate");
            EYES = Manikin.GetParameterModelEnum("overrideSoundLeftUpperLobeAnteriorLungs"); ;
            Diastolic_mmHg = Manikin.GetParameterAppValue<int>("Laerdal.Response.BloodPressure.Diastolic_mmHg");
            Systolic_mmHg = Manikin.GetParameterAppValue<int>("Laerdal.Response.BloodPressure.Systolic_mmHg");
            Value_LiterPerMinute = Manikin.GetParameterAppValue<double>("Laerdal.Response.CardiacOutput.Value_LiterPerMinute");

            SVLeftLowerPosteriorLungs = Manikin.GetParameterModelInteger("soundVolumeLeftLowerLobePosteriorLungs");
            SVLeftLowerAnteriorLungs = Manikin.GetParameterModelInteger("soundVolumeLeftLowerLobeAnteriorLungs");
            SVRightLowerPosteriorLungs = Manikin.GetParameterModelInteger("soundVolumeRightLowerLobePosteriorLungs");
            SVRightLowerAnteriorLungs = Manikin.GetParameterModelInteger("soundVolumeRightLowerLobeAnteriorLungs");
            SVRightUpperPosteriorLungs = Manikin.GetParameterModelInteger("soundVolumeRightUpperLobePosteriorLungs");

            overrideHeartRate = Manikin.GetParameterModelDouble("override HeartRate");


            RespiratoryRate = Manikin.GetParameterModelDouble("override RespiratoryRate");
            LeftUpperAnteriorLungs = Manikin.GetParameterModelEnum("overrideSoundLeftUpperLobeAnteriorLungs");
            LeftUpperPosteriorLungs = Manikin.GetParameterModelEnum("overrideSoundLeftUpperLobePosteriorLungs");
            RightUpperAnteriorLungs = Manikin.GetParameterModelEnum("overrideSoundRightUpperLobeAnteriorLungs");
            RightUpperPosteriorLungs = Manikin.GetParameterModelEnum("overrideSoundRightUpperLobePosteriorLungs");


            PCWP = Manikin.GetParameterModelDouble("override PCWP");
            PAPSystolic = Manikin.GetParameterModelDouble("override PAPSystolic");
            PAPDiastolic = Manikin.GetParameterModelDouble("override PAPDiastolic");
            CVP = Manikin.GetParameterModelDouble("override CVP");

            updateTimer = new System.Threading.Timer(new System.Threading.TimerCallback(ProcessDataFromServer), this, 0, 100);

            AreParametersSetup = true;
            OnPauseEvent(null, null);


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
            ServerIP = server;
            //Manikin.StartDiscovery();
            System.Threading.Thread.Sleep(3000);

            IEnumerable<ManikinInfo> servers = Manikin.GetDiscoveryList();

            List<string> list = new List<string>();
            foreach (ManikinInfo server1 in servers)
            {
                System.Diagnostics.Debug.Write(server1, "server1");

                if (server1.AddressList.Count() == 0)
                    continue; // need the IP address

                IPAddress address = server1.AddressList.First().Item1;
                System.Diagnostics.Debug.Write(address, "address");

                string serverName = server1.ComputerName + " (" + address.ToString() + ")";

                list.Add(serverName);
            }

            var test = Manikin.Connect(server, lostConnection);
            // Try to connect to the manikin using the obtained URI
            if (!Manikin.Connect(server, lostConnection))
            {
                // Store the error
                error = String.Format("Unable to connect to server", server);
                return false;
            }
            error = null;
            return Manikin.IsConnected;
        }

        public double HeartRate
        {
            get
            {
                return heartRate.Value;
            }
            set
            {
                overrideHeartRate.Value = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("HeartRate"));
            }
        }


        //LeftUpperAnteriorLungs = Manikin.GetParameterModelEnum("overrideSoundLeftUpperLobeAnteriorLungs");

        public void DecrementHeartRate()
        {
            HeartRate = HeartRate - 1.0;
        }        //LeftUpperPosteriorLungs = Manikin.GetParameterModelEnum("overrideSoundLeftUpperLobePosteriorLungs");

        public void IncrementHeartRate()
        {
            HeartRate = HeartRate + 1.0;
        }
        public Boolean checkHealth()
        {

            Boolean first_check = Manikin.IsConnected;
            System.Diagnostics.Debug.Write(first_check, "first_check");

            if (!first_check)
            {
                string error;
                Connect("", out error);
            }

            while (!Manikin.IsConnected)
            {
                string address = "10.100.2.166";
                string error;
                Boolean connected = Connect(address, out error);
                System.Threading.Thread.Sleep(500);
            }

            if (!AreParametersSetup)
            {
                SetupParameters();
            }
            System.Diagnostics.Debug.Write(AreParametersSetup, "AreParametersSetup");
            System.Diagnostics.Debug.Write(Manikin.IsConnected, "Manikin.IsConnected");

            Boolean final_check = AreParametersSetup && Manikin.IsConnected;
            PauseEvent.Value = false;
            Manikin.ResumeAllTrends();
            return final_check;
;
        }

             //Diastolic_mmHg.Value = 140;
            //Systolic_mmHg.Value = 90;

        public bool UpdateSYS_BP(int sys_mmHg)
        {
            IParameterAppValue<int> S = Manikin.GetParameterAppValue<int>("Laerdal.Response.BloodPressure.Systolic_mmHg");
            checkHealth();

            while (S.Value < sys_mmHg)
            {
                if (S.Value != sys_mmHg)
                {
                    Systolic_mmHg.Value = sys_mmHg;
                    System.Threading.Thread.Sleep(100);
                }
            }

            return S.Value == sys_mmHg;
        }

        public bool UpdateDIA_BP(int dia_mmHg)
        {
            IParameterAppValue<int> D = Manikin.GetParameterAppValue<int>("Laerdal.Response.BloodPressure.Diastolic_mmHg");
            checkHealth();

            while (D.Value < dia_mmHg)
            {
                if (D.Value != dia_mmHg)
                {
                    Diastolic_mmHg.Value = dia_mmHg;
                    System.Threading.Thread.Sleep(100);
                }
            }

            return D.Value == dia_mmHg;
        }


        public bool UpdateBP(int dia_mmHg, int sys_mmHg)
        {
            IParameterAppValue<int> D = Manikin.GetParameterAppValue<int>("Laerdal.Response.BloodPressure.Diastolic_mmHg");
            IParameterAppValue<int> S = Manikin.GetParameterAppValue<int>("Laerdal.Response.BloodPressure.Systolic_mmHg");
            checkHealth();
            
            while (D.Value < dia_mmHg || S.Value < sys_mmHg)
            {
                if (D.Value != dia_mmHg)
                {
                    Diastolic_mmHg.Value = dia_mmHg;
                    System.Threading.Thread.Sleep(100);
                }

                if (S.Value != sys_mmHg)
                {
                    Systolic_mmHg.Value = sys_mmHg;
                    System.Threading.Thread.Sleep(100);
                }
            }

            return D.Value == dia_mmHg && S.Value == sys_mmHg;
        }

        public bool UpdateHeartRate(int goal_hr)
        {
            checkHealth();

            while (heartRate.Value != goal_hr)
            {
                if (heartRate.Value < goal_hr)
                {
                    System.Diagnostics.Debug.Write(heartRate.Value, "~~~Increase HR~~~");
                    IncrementHeartRate();
                }

                if (heartRate.Value > goal_hr)
                {
                    System.Diagnostics.Debug.Write(heartRate.Value, "~~~Decrease HR~~~");
                    DecrementHeartRate();
                }
            }

            System.Diagnostics.Debug.Write(HeartRate, "HeartRate");

            return heartRate.Value == goal_hr;
        }

        public bool Check()
        {
            return Manikin.CheckConnection();
        }

        public void trigger_state_severe_asthma()
        {
            Manikin.PauseAllTrends();
            UpdateHeartRate(110);
            System.Threading.Thread.Sleep(100);

            RespiratoryRate.Value = 22;
            System.Threading.Thread.Sleep(100);

            LeftUpperAnteriorLungs.Value = 5;
            LeftUpperPosteriorLungs.Value = 5;
            RightUpperAnteriorLungs.Value = 5;
            RightUpperPosteriorLungs.Value = 5;

            SVLeftLowerPosteriorLungs.Value = 80;
            SVLeftLowerAnteriorLungs.Value = 80;
            SVRightLowerPosteriorLungs.Value = 80;
            SVRightLowerAnteriorLungs.Value = 80;
            SVRightLowerAnteriorLungs.Value = 80;
            SVRightUpperPosteriorLungs.Value = 80;
            
            System.Threading.Thread.Sleep(100);

            etCO2_Value_mmHg.Value = 45;
            System.Threading.Thread.Sleep(100);

            TBlood_Celcius.Value = 99;
            System.Threading.Thread.Sleep(100);

            TPeri_Celcius.Value = 97;
            System.Threading.Thread.Sleep(100);

            PCWP.Value = 15;
            System.Threading.Thread.Sleep(100);

            PAPSystolic.Value = 42;
            PAPDiastolic.Value = 20;
            CVP.Value = 20;
            Value_LiterPerMinute.Value = 5.6;

            bool bp = UpdateBP(140, 90);
            System.Diagnostics.Debug.Write(bp, "bloodpressure");
            Manikin.ResumeAllTrends();
            PauseEvent.Value = false;
        }

        public void trigger_state_asthma_worsen()
        {
            Manikin.PauseAllTrends();
            UpdateHeartRate(48);
            System.Threading.Thread.Sleep(100);

            bool bp = UpdateBP(80, 60);
            System.Diagnostics.Debug.Write(bp, "bloodpressure");

            RespiratoryRate.Value = 40;
            System.Threading.Thread.Sleep(100);

            SVLeftLowerPosteriorLungs.Value = 0;
            SVLeftLowerAnteriorLungs.Value = 0;
            SVRightLowerPosteriorLungs.Value = 0;
            SVRightLowerAnteriorLungs.Value = 0;
            SVRightUpperPosteriorLungs.Value = 0;
            Value_LiterPerMinute.Value = 3.4;

            etCO2_Value_mmHg.Value = 65;
            System.Threading.Thread.Sleep(100);

            TBlood_Celcius.Value = 99;
            System.Threading.Thread.Sleep(100);

            TPeri_Celcius.Value = 97;
            PCWP.Value = 9;
            PAPSystolic.Value = 25;
            PAPDiastolic.Value = 12;
            CVP.Value = 6;

            Manikin.ResumeAllTrends();
            PauseEvent.Value = false;
        }

        public bool UpdateEyeStatus(bool status)
        {
            if(status){
                EYES.Value = 133;
            }
            System.Threading.Thread.Sleep(100);
            var i = Manikin.GetParameterModelEnum("Laerdal.Response.Eyes.EyelidStatus");

            return EYES.Value == 133;
        }



        public bool UpdateRespiratoryRate(int goal_rr)
        {
            RespiratoryRate.Value = goal_rr;
            System.Threading.Thread.Sleep(100);
            var r = Manikin.GetParameterModelDouble("override RespiratoryRate");
            int check_r = Int32.Parse(r.ToString());
            return check_r == goal_rr;
        }


        public void trigger_intervention()
        {
            Manikin.PauseAllTrends();
            UpdateHeartRate(100);
            bool bp = UpdateBP(124, 70);
            System.Diagnostics.Debug.Write(bp, "bloodpressure");

            RespiratoryRate.Value = 20;

            SVLeftLowerPosteriorLungs.Value = 50;
            SVLeftLowerAnteriorLungs.Value = 50;
            SVRightLowerPosteriorLungs.Value = 50;
            SVRightLowerAnteriorLungs.Value = 50;
            SVRightUpperPosteriorLungs.Value = 50;

            etCO2_Value_mmHg.Value = 65;
            TBlood_Celcius.Value = 99;
            TPeri_Celcius.Value = 97;
            PCWP.Value = 15;
            PAPSystolic.Value = 25;
            PAPDiastolic.Value = 12;
            CVP.Value = 6;

            Manikin.ResumeAllTrends();
            PauseEvent.Value = false;
        }

        public void trigger_mild_asthma()
        {
            Manikin.PauseAllTrends();
            UpdateHeartRate(90);
            bool bp = UpdateBP(138, 84);
            System.Diagnostics.Debug.Write(bp, "bloodpressure");

            RespiratoryRate.Value = 18;

            SVLeftLowerPosteriorLungs.Value = 0;
            SVLeftLowerAnteriorLungs.Value = 0;

            etCO2_Value_mmHg.Value = 30;
            TBlood_Celcius.Value = 99;
            TPeri_Celcius.Value = 97;
            PCWP.Value = 9;
            PAPSystolic.Value = 25;
            PAPDiastolic.Value = 12;
            CVP.Value = 6;

            Manikin.ResumeAllTrends();
            PauseEvent.Value = false;
        }

        public void trigger_healthy_state()
        {
            Manikin.ResetParameters();
            UpdateHeartRate(90);
            System.Threading.Thread.Sleep(100);

            RespiratoryRate.Value = 12;
            System.Threading.Thread.Sleep(100);

            LeftUpperAnteriorLungs.Value = 1;
            LeftUpperPosteriorLungs.Value = 1;
            RightUpperAnteriorLungs.Value = 1;
            RightUpperPosteriorLungs.Value = 1;

            SVLeftLowerPosteriorLungs.Value = 50;
            SVLeftLowerAnteriorLungs.Value = 50;
            SVRightLowerPosteriorLungs.Value = 50;
            SVRightLowerAnteriorLungs.Value = 50;
            SVRightLowerAnteriorLungs.Value = 50;
            SVRightUpperPosteriorLungs.Value = 50;

            System.Threading.Thread.Sleep(100);

            etCO2_Value_mmHg.Value = 34;
            System.Threading.Thread.Sleep(100);

            TBlood_Celcius.Value = 99;
            System.Threading.Thread.Sleep(100);

            TPeri_Celcius.Value = 97;
            System.Threading.Thread.Sleep(100);

            PCWP.Value = 15;
            System.Threading.Thread.Sleep(100);

            PAPSystolic.Value = 25;
            PAPDiastolic.Value = 12;
            CVP.Value = 20;
            Value_LiterPerMinute.Value = 5.6;

            bool bp = UpdateBP(120, 80);
        }

    }
}