using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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
            {"R", 0},
            {"Y", 1 },
            {"G", 2 },
            {"C", 3}

        };

        Dictionary<string, int> eventMap = new Dictionary<string, int>
        {
            {"event1", 0},  //populate these when we know what events will occur
            {"event2", 1 }

        };

        public void AddAction(string state, string eventTrigger, TimestampedAction action)
        {
            // TODO: basically identical to setnextstate except populating the action list.
            int x, y;
            stateMap.TryGetValue(state, out y);
            eventMap.TryGetValue(eventTrigger, out x);
            FST[x, y].actions.Add(action);
        }

        public string GetCurrentState()
        {
            // TODO: Implement this
            return stateP;
        }

        public string ProcessEvent(string eventTrigger)
        {
            // TODO: Implement this
            return null;
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
