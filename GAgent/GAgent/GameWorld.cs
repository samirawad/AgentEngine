using GAgent.StandardEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    // Relationships between objects are stored using this struct.  Querying and altering these relationships
    // is one of the main implementations of game state.
    public struct GameEntityRelation
    {
        GameEntity RelationSubject;

        GameEntity RelationObject;

        string Relationship;
    }

    // Behold, the smallest game engine evah
    public class GameWorld
    {
        private char[] Alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public Random RND = new Random();

        public string LastOutcome;

        public Dictionary<char, GameAction>  CurrentValidEvents = new Dictionary<char,GameAction>();

        public List<GameEntity> AllEntities = new List<GameEntity>();

        public List<GameEntityRelation> AllRelations = new List<GameEntityRelation>();

        public List<GameAction> AllGameActions = new List<GameAction>();

        public List<Outcome> AllOutcomes = new List<Outcome>();

        public GameWorld()
        {
            // TODO: Create starting entities with their relations.
            // an entity library which generates and adds entities to the world?

            AllEntities.AddRange(EntityLibrary.DefaultEntities.SampleEntities);
            //AllGameActions.AddRange(SampleMemoryEventLib.GameEvents);
            //AllOutcomes.AddRange(SampleMemoryEventLib.GameEventOutcomes);
            
            // Load the Game Action libraries we're using
            AllGameActions.AddRange(TravelingLib.GameEvents);
            AllOutcomes.AddRange(TravelingLib.GameEventOutcomes);
            AllGameActions.AddRange(TravelEncountersLib.GameEvents);
            AllOutcomes.AddRange(TravelEncountersLib.GameEventOutcomes);
        }

        
        // Return a string describing which events are currently valid, while populating the CurrentValidEvents dictionary.
        public string GetValidEvents()
        {
            StringBuilder sbResult = new StringBuilder();
            int keyIndex = 0;
            CurrentValidEvents.Clear();
            foreach (GameAction currEvent in AllGameActions)
            {
                if(currEvent.IsValid(this))
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
            return CurrentValidEvents[eventKey].ListOutcomes(this, AllOutcomes, AllEntities);
        }

        public bool IsEventValid(char eventKey)
        {
            bool isValid = CurrentValidEvents.ContainsKey(eventKey);
            return isValid;
        }

        public string DoEvent(char eventKey)
        {
            LastOutcome = CurrentValidEvents[eventKey].SelectOutcome(this);
            return LastOutcome;
        }
    }

}
