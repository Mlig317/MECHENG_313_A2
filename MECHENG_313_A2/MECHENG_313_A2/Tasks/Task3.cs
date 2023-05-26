using MECHENG_313_A2.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace MECHENG_313_A2.Tasks
{
    internal class Task3 : Task2
    {
        public override TaskNumber TaskNumber => TaskNumber.Task3;
        static Timer timer;
        

        public void ConfigLightLength(int redLength, int greenLength)
        {
            // TODO: Implement this

        }

        private void tickTock(Object source, ElapsedEventArgs e)
        {
            Tick();
        }
        // TODO: Implement this

        public override void Start()
        {
            iAction(); //populate the actions
            fsm.iTable(); //populate the states
            _taskPage.AddLogEntry("starting task 3");
            _taskPage.AddLogEntry(fsm.GetCurrentState());
            _taskPage.SerialPrint(DateTime.Now, fsm.GetCurrentState() + "\n");
            _taskPage.SetTrafficLightState(TrafficLightState.Green);
            timer = new Timer();
            timer.Elapsed += tickTock;
            timer.Interval = 1000;
            timer.Start();
        }
        

    }

    
}
