using MECHENG_313_A2.Serial;
using MECHENG_313_A2.ViewModels;
using MECHENG_313_A2.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MECHENG_313_A2.Tasks
{
    internal class Task2 : IController
    {
        MockSerialInterface fakeArduino = new MockSerialInterface();
        FiniteStateMachine fsm = new FiniteStateMachine();
        
        public virtual TaskNumber TaskNumber => TaskNumber.Task2;

        protected ITaskPage _taskPage;
        private void iAction() // initialize actions except its super scuffed cos idk how timestamped actions are supposed to go wtf is a lambda expression
        {
            TimestampedAction G = (timestamp) => fakeArduino.SetState(TrafficLightState.Green);
            TimestampedAction Y = (timestamp) => fakeArduino.SetState(TrafficLightState.Yellow);
            TimestampedAction R = (timestamp) => fakeArduino.SetState(TrafficLightState.Red);
            TimestampedAction B = (timestamp) => fakeArduino.SetState(TrafficLightState.None);
            TimestampedAction DN = (timestamp) => DoNothing(); //this fills in empty spots so that the compiler doesn't freak out when theres no action to do




            fsm.AddAction("G","tick",Y);
            fsm.AddAction("Y", "tick", R);
            fsm.AddAction("R", "tick", G);
            fsm.AddAction("C", "tick", B); 
            fsm.AddAction("B", "tick", Y);

            fsm.AddAction("R", "config", Y);
            fsm.AddAction("C", "config", R);
            fsm.AddAction("B", "config", R);

            //extra cases so that it doesn't try and execute actions that aren't there when trying to config from g/y
            fsm.AddAction("G", "config", DN);
            fsm.AddAction("Y", "config", DN);
        }
        public void ConfigLightLength(int redLength, int greenLength)
        {
            // TODO: Implement this
        }
        

        public async Task<bool> EnterConfigMode()
        {
            
            // TODO: Implement this
            fsm.ProcessEvent("config");
            _taskPage.AddLogEntry(fsm.GetCurrentState());
            _taskPage.SerialPrint(DateTime.Now, fsm.GetCurrentState() + "\n");

            if (fsm.GetCurrentState() == "C")
            {
                _taskPage.SetTrafficLightState(TrafficLightState.Yellow);
            }
            
            return fsm.GetCurrentState() == "C";
        }

        public void ExitConfigMode()
        {
            // TODO: Implement this
            
            string cState;
            cState = fsm.GetCurrentState();
            if (cState == "C" || cState == "B")
            {
                fsm.ProcessEvent("config");
            }
            _taskPage.AddLogEntry(fsm.GetCurrentState());
            _taskPage.SerialPrint(DateTime.Now, fsm.GetCurrentState() + "\n");
            _taskPage.SetTrafficLightState(TrafficLightState.Red);
        }

        public async Task<string[]> GetPortNames()
        {
            // TODO: Implement this
            return await fakeArduino.GetPortNames();
        }

        public async Task<string> OpenLogFile()
        {
            // TODO: Implement this

            // Help notes: to read a file named "log.txt" under the LocalApplicationData directory,
            // you may use the following code snippet:
            // string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "log.txt");
            // string text = File.ReadAllText(filePath);
            //
            // You can also create/write to file(s) through System.IO.File. 
            // See https://learn.microsoft.com/en-us/xamarin/xamarin-forms/data-cloud/data/files?tabs=windows, and
            // https://learn.microsoft.com/en-us/dotnet/api/system.io.file?view=netstandard-2.0 for more details.

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "log.txt");
            File.Create(filePath).Close();                          //create the file 🧙

            return filePath;
        }

        public async Task<bool> OpenPort(string serialPort, int baudRate)
        {
            // TODO: Implement this
            return await fakeArduino.OpenPort(serialPort,baudRate);
        }

        public void RegisterTaskPage(ITaskPage taskPage)
        {
            _taskPage = taskPage;
        }
        public void DoNothing()
        {
            // does nothing when trying to change from green/yellow to config
        }
        public void Start()
        {
            iAction(); //populate the actions
            fsm.iTable(); //populate the states
            //PrintNStates();
            _taskPage.AddLogEntry(fsm.GetCurrentState());
            _taskPage.SerialPrint(DateTime.Now, fsm.GetCurrentState() + "\n");
            _taskPage.SetTrafficLightState(TrafficLightState.Green);
            // TODO: Implement this
        }

        public void Tick()
        {
            // TODO: Implement this
            fsm.ProcessEvent("tick");
            _taskPage.AddLogEntry(fsm.GetCurrentState());
            _taskPage.SerialPrint(DateTime.Now, fsm.GetCurrentState() + "\n");

            // !!! To condense need to find way to convert string to TrafficLightState !!!
            switch(fsm.GetCurrentState())
                {
                case "G":
                    _taskPage.SetTrafficLightState(TrafficLightState.Green);
                    break;
                case "Y":
                    _taskPage.SetTrafficLightState(TrafficLightState.Yellow);
                    break;
                case "R":
                    _taskPage.SetTrafficLightState(TrafficLightState.Red);
                    break;
                case "C":
                    _taskPage.SetTrafficLightState(TrafficLightState.Yellow);
                    break;
                default:
                    _taskPage.SetTrafficLightState(TrafficLightState.None);
                    break;
            }

        }
    }
}
