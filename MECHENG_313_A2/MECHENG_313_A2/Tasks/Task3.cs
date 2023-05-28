using MECHENG_313_A2.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using static MECHENG_313_A2.Tasks.FiniteStateMachine;

namespace MECHENG_313_A2.Tasks
{
    internal class Task3 : Task2
    {
        public override TaskNumber TaskNumber => TaskNumber.Task3;
        static Timer timer;
        int count = 0;
        bool conT = false;
        
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
            }else if((fsm.GetCurrentState() == "R") || (fsm.GetCurrentState() == "B") || (fsm.GetCurrentState() == "C"))
            {
                count++;
                if (count == (rTime/100))
                {
                    if (conT)
                    {
                        fsm.ProcessEvent("config");
                        _taskPage.AddLogEntry(LogWriter());
                        _taskPage.SerialPrint(DateTime.Now, fsm.GetCurrentState() + "\n");
                        conT = false;
                        count = 0;

                        switch (fsm.GetCurrentState())
                        {
                            case "R":
                                _taskPage.SetTrafficLightState(TrafficLightState.Red);
                                break;
                            case "C":
                                _taskPage.SetTrafficLightState(TrafficLightState.Yellow);
                                break;
                            case "B":
                                _taskPage.SetTrafficLightState(TrafficLightState.None);
                                break;
                        }
                    }
                    else {
                        Tick();

                        count = 0;
                    }
                    
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
       
        // Allow you to queue the config mode at any state but will keep ticking until it reaches end of red
        public override async Task<bool> EnterConfigMode()
        {
           conT = true;
            while (fsm.GetCurrentState() != "C")
            {
                await Task.Delay(100);
            }
            return fsm.GetCurrentState() == "C";
        }
        public override  void ExitConfigMode()
        {
            // TODO: Implement this

            conT = true;
        }
        public override void Start()
        {
            iAction(); //populate the actions
            fsm.iTable(); //populate the states
            _taskPage.AddLogEntry("starting task 3");
            _taskPage.AddLogEntry(LogWriter());
            _taskPage.SerialPrint(DateTime.Now, fsm.GetCurrentState() + "\n");
            _taskPage.SetTrafficLightState(TrafficLightState.Green);
            timer = new Timer();
            timer.Elapsed += tickTock;
            timer.Interval = 100;
            timer.Start();
        }
        

    }

    
}
