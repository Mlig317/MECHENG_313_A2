using MECHENG_313_A2.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using static MECHENG_313_A2.Tasks.FiniteStateMachine;

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
            timer.Interval = 100;
            timer.Start();
        }
        

    }

    
}
