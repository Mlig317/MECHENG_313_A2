using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Forms.Internals.Profile;

namespace MECHENG_313_A2.Tasks
{
    public class FiniteStateMachine : IFiniteStateMachine
    {
        //finitestatemachine uses a finite state table (a 2d array) of structs to transition between states. rows correspond to states, columns to events
        //using a dictionary to map input string states/events to array indexes. Imo this is faster than labelling each cell struct with current state and event 
        //and having to index over whole array to find element. (drawback, need to update dictionary with each new state)
        private string stateP;
        private struct stateTrans
        {
            public string nState;
            public List<TimestampedAction> actions;
        }
        stateTrans[,] FST = new stateTrans[2, 4];

        Dictionary<string, int> stateMap = new Dictionary<string, int>
        {
            {"G", 0},       //states that will be changed by events
            {"Y", 1 },
            {"R", 2 },
            {"Y'", 3},
            {"B'", 4}

        };
        

        Dictionary<string, int> eventMap = new Dictionary<string, int>
        {
            {"tick", 0},    //events that will change the state
            {"config", 1 }

        };

        public void AddAction(string state, string eventTrigger, TimestampedAction action)
        {
            // TODO: basically identical to setnextstate except populating the action list.
            int x, y;
            stateMap.TryGetValue(state, out y);
            eventMap.TryGetValue(eventTrigger, out x);

            FST[x, y].actions.Add(action);

        }
        /// <summary>
        /// this should initialize the finite state table with the rules of the finite state machine as set out in task  2. I'm puttin them here as
        /// it seems best practice to separate them like this
        /// 
        /// rules are set out in the table but i'm treating C state as two states (B and Y')
        /// start state is assumed to be green on (cheeky bit of ladder logic below)
        /// order goes G -> Y -> R -> G
        ///                        -> Y' -> B -> Y'
        ///                              -> R|-> R
        /// </summary>
        public void iTable()
        {
            //current state, next state, event trigger
            SetNextState("G", "Y", "tick");
            SetNextState("Y", "R", "tick");
            SetNextState("R", "G", "tick");
            SetNextState("Y'", "B", "tick");
            SetNextState("B", "Y'", "tick");
            SetNextState("R", "Y'", "config");
            SetNextState("Y'", "R", "config");
            SetNextState("B", "R", "config");
        }

        public string GetCurrentState()
        {
            // TODO: Implement this
            return stateP;
        }
        

        public string ProcessEvent(string eventTrigger)
        {
            // TODO: Implement this
            string nextState;
            int x, y;
            stateMap.TryGetValue(stateP, out y);
            eventMap.TryGetValue(eventTrigger, out x);
            nextState = FST[x, y].nState;
            for (int i = 0; i < FST[x, y].actions.Count; i++)
            {
                 
                ThreadPool.QueueUserWorkItem((state)=> FST[x, y].actions[i](DateTime.Now));//when the task is executed, grab the current time and pass it to the action
            }
            SetCurrentState(nextState);
            return nextState;
        }

        public void SetCurrentState(string state)
        {
            // TODO: Implement this
            stateP = state;
        }

        public void SetNextState(string state, string nextState, string eventTrigger)
        {
            // x = events, y = states. Use a dictionary to map the strings to array indicies.
            int x, y;
            stateMap.TryGetValue(state, out y);
            eventMap.TryGetValue(eventTrigger, out x);
            FST[x,y].nState = nextState;
        }

       
    }
}
