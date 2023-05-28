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
        bool conT = false;

        private void TickTock(Object source, ElapsedEventArgs e)
        {

            if (fsm.GetCurrentState() == "G")//green length
            {
                count++;

                if (count == (gTime / 100))
                {
                    Tick();
                    count = 0;
                }
            }
            else if (fsm.GetCurrentState() == "R") // 
            {
                count++;
                if (count == (rTime / 100))
                {

                    if (conT)
                    {
                        fsm.ProcessEvent("config");
                        _taskPage.AddLogEntry(LogWriter("Event Trigger: Entering Config"));
                        _taskPage.SerialPrint(DateTime.Now, "Entering Config  Current State: " + fsm.GetCurrentState() + "\n");
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

                    else
                    {
                        Tick();

                        count = 0;
                    }

                }
            }
            else if ((fsm.GetCurrentState() == "B") || (fsm.GetCurrentState() == "C"))
            {
                count++;
                if (count == 10)
                {
                   
                    if (conT)
                    {
                        fsm.ProcessEvent("config");
                        _taskPage.AddLogEntry(LogWriter("Event Trigger: Exiting Config"));
                        _taskPage.SerialPrint(DateTime.Now, "Exiting Config  Current State: " + fsm.GetCurrentState() + "\n");
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
                    else
                    {
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

        // Allow you to queue the config mode at any state but will keep ticking until it reaches end of red
        public override async Task<bool> EnterConfigMode()
        {
            conT = true;
            await Task.Run(() => _taskPage.AddLogEntry(LogWriter("Waiting to enter Config")));
            await Task.Run(() => _taskPage.SerialPrint(DateTime.Now, "Waiting to enter Config   Current State: " + fsm.GetCurrentState() + "\n"));
            while (fsm.GetCurrentState() != "C")
            {
                await Task.Delay(100);
            }
            return fsm.GetCurrentState() == "C";
        }

        public override void Start()
        {
            iAction(); //populate the actions
            fsm.iTable(); //populate the states
            _taskPage.AddLogEntry("starting task 3");
            _taskPage.AddLogEntry(LogWriter("Event Trigger: Start"));
            _taskPage.SerialPrint(DateTime.Now, "Event Trigger:Start   Current State: " + fsm.GetCurrentState() + "\n");
            _taskPage.SetTrafficLightState(TrafficLightState.Green);
            timer = new Timer();
            timer.Elapsed += TickTock;
            timer.Interval = 100;
            timer.Start();
        }


    }
}
