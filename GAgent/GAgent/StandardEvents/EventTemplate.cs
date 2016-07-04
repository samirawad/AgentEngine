using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent.StandardEvents
{
    public static class EventTemplate
    {
        public static List<GameAction> GameEvents = new List<GameAction>() { 
            new GameAction()
            {
                ID = "id",
                ShowOutcomes = false,
                Description = (world) => { return "description of the event"; },
                IsValidDel = (world) => {
                    return false; // implement validity logic here;
                }
            },
        };

        public static List<Outcome> GameEventOutcomes = new List<Outcome>() { 
            new Outcome()
            {
                GetDescription = (source, world) => { return "description of the outcome"; },
                IsValid = (source, world) => {
                    bool valid = false; // function that determines if the outcome is valid
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.Append("Perform the game logic here");
                    return sbResult.ToString();
                }
            }
        };
    }
}
