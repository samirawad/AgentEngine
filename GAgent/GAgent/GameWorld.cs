using GAgent.StandardEvents;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    // Relationships between objects are stored using this struct.  Querying and altering these relationships
    // is one of the main implementations of game state.


    // Behold, the smallest game engine evah
    public class GameWorld
    {
        private char[] Alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public Random RND = new Random();

        // Every game turn, an action must be selected by the player.  We can reference it here.
        public GameAction CurrentAction = null;

        public GameOutcome CurrentOutcome = null;

        public string LastOutcomeLog;

        public Dictionary<char, GameAction> CurrentValidEvents = new Dictionary<char, GameAction>();

        public Dictionary<string, GameAgent> AllAgents = new Dictionary<string, GameAgent>();

        public List<GameAgentRelationship> AllRelations = new List<GameAgentRelationship>();

        public List<GameAction> AllGameActions = new List<GameAction>();

        public List<GameOutcome> AllGameOutcomes = new List<GameOutcome>();

        public GameWorld()
        {
            // TODO: Create starting entities with their relations.
            // an entity library which generates and adds entities to the world?

            //foreach (GameAgent currEntity in EntityLibrary.DefaultEntities.SampleEntities)
            //{
            //    AllEntities.Add(currEntity.S["Name"], currEntity);
            //}
            //AllGameActions.AddRange(SampleMemoryEvent.GameEvents);
            //AllOutcomes.AddRange(SampleMemoryEvent.GameEventOutcomes);

            // Load the Game Event libraries we're using
            //AllGameActions.AddRange(TravelingEvents.GameEvents);
            //AllOutcomes.AddRange(TravelingEvents.GameEventOutcomes);
            //AllGameActions.AddRange(PartyManagementEvents.GameEvents);
            //AllOutcomes.AddRange(PartyManagementEvents.GameEventOutcomes);
            //AllGameActions.AddRange(DungeonEvents.GameEvents);
            //AllOutcomes.AddRange(DungeonEvents.GameEventOutcomes);

            // 1. Initialize the world from the setup file
            // 2. Read all actions and outcomes
            Console.WriteLine("break here");
            AllGameOutcomes.AddRange(TravelActions.Outcomes);
            AllGameActions.AddRange(TravelActions.Actions);
            
        }

        public GameAgent GetAgentByID(string agentID)
        {
            return AllAgents.ContainsKey(agentID) ? AllAgents[agentID] : null;
        }

        // Return a string describing which events are currently valid, while populating the CurrentValidEvents dictionary.
        public string GetValidEvents()
        {
            StringBuilder sbResult = new StringBuilder();
            int keyIndex = 0;
            CurrentValidEvents.Clear();
            foreach (GameAction currEvent in AllGameActions)
            {
                if (currEvent.IsValid(this))
                {
                    CurrentValidEvents.Add(Alphabet[keyIndex], currEvent);
                    sbResult.AppendLine(Alphabet[keyIndex] + ": " + currEvent.Description(this));
                    keyIndex++;
                }
            }
            return sbResult.ToString();
        }

        public string ListEventOutcomes(char eventKey)
        {
            return CurrentValidEvents[eventKey].ListOutcomes(this);
        }

        public bool IsEventValid(char eventKey)
        {
            bool isValid = CurrentValidEvents.ContainsKey(eventKey);
            return isValid;
        }

        public bool IsCurrentAction(string actionID)
        {
            if(CurrentAction == null)
            {
                return false;
            }
            else
            {
                return CurrentAction.ID == actionID;
            }
            
        }

        public bool IsCurrentOutcome(string outcomeID)
        {
            if(CurrentOutcome == null)
            {
                return false;
            }
            else
            {
                return CurrentOutcome.ID == outcomeID;
            }
        }

        public string DoGameAction(char eventKey)
        {
            CurrentAction = CurrentValidEvents[eventKey];
            LastOutcomeLog = CurrentValidEvents[eventKey].SelectOutcome(this);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return LastOutcomeLog;
        }
    }

}
