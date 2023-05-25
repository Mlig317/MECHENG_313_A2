using MECHENG_313_A2.Serial;
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

            


            fsm.AddAction("G","tick",Y);
            fsm.AddAction("Y", "tick", R);
            fsm.AddAction("R", "tick", G);
            fsm.AddAction("Y'", "tick", B); 
            fsm.AddAction("B", "tick", Y);

            fsm.AddAction("R", "config", Y);
            fsm.AddAction("Y'", "tick", R);
            fsm.AddAction("B", "tick", R);
        }
        public void ConfigLightLength(int redLength, int greenLength)
        {
            // TODO: Implement this
        }
        

        public async Task<bool> EnterConfigMode()
        {
            // TODO: Implement this
            fsm.ProcessEvent("config");
            return fsm.GetCurrentState() == "Y'";
            
            
        }

        public void ExitConfigMode()
        {
            // TODO: Implement this
            string cState;
            cState = fsm.GetCurrentState();
            if (cState == "Y'" || cState == "B")
            {
                fsm.ProcessEvent("config");
            }
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

        public void Start()
        {
            iAction(); //populate the actions
            fsm.iTable(); //populate the states
            // TODO: Implement this
        }

        public void Tick()
        {
            // TODO: Implement this
            fsm.ProcessEvent("tick");
        }
    }
}
