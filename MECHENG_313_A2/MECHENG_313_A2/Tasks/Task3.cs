using MECHENG_313_A2.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using static MECHENG_313_A2.Tasks.FiniteStateMachine;
using System.Runtime.CompilerServices;

namespace MECHENG_313_A2.Tasks
{
    internal class Task3 : Task2
    {
        public override TaskNumber TaskNumber => TaskNumber.Task3;
        static Timer timer;
        int count = 0;
        
        public  void ConfigLightLength(int redLength, int greenLength)
        {
            

        }

        private void tickTock(Object source, ElapsedEventArgs e)
        {
           
            if(fsm.GetCurrentState() == "G")//green length
            {
                count++;
                
                if (count == (gTime / 100))
                {
                    Tick();
                    count = 0;
                }
            }else if(fsm.GetCurrentState() == "R")
            {
                count++;
                if (count == (rTime/100))
                {
                    Tick();
                    count = 0;
                }
            }
            else
            {
                count++;
                if (count == 10)
                {
                    Tick();
                    count = 0;
                }
            }
            
        }
        // TODO: Implement this
       
        // Allow you to queue the config mode at any state but will keep ticking until it reaches red
        public override async Task<bool> EnterConfigMode()
        {
            if (fsm.GetCurrentState() != "R")
            {
                _taskPage.AddLogEntry(LogWriter("Waiting to enter Config"));
                _taskPage.SerialPrint(DateTime.Now, fsm.GetCurrentState() + "\n");
                while (fsm.GetCurrentState() != "R")
                {
                    await Task.Delay(100);
                }
            }
            fsm.ProcessEvent("config");
            _taskPage.AddLogEntry(LogWriter("Entering Config"));
            _taskPage.SerialPrint(DateTime.Now, fsm.GetCurrentState() + "\n");
            return true;
        }

        public override void Start()
        {
            iAction(); //populate the actions
            fsm.iTable(); //populate the states
            _taskPage.AddLogEntry("starting task 3");
            _taskPage.AddLogEntry(LogWriter("Event Trigger: Start"));
            _taskPage.SerialPrint(DateTime.Now, fsm.GetCurrentState() + "\n");
            _taskPage.SetTrafficLightState(TrafficLightState.Green);
            timer = new Timer();
            timer.Elapsed += tickTock;
            timer.Interval = 100;
            timer.Start();
        }
        

    }

    
}
