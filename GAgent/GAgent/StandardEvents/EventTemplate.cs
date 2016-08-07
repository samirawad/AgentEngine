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
            new Outcome(
                "outcomeID",
                // Description
                (world) => { 
                    return "description of the outcome";
                },
                // Validity
                (world) => { 
                    bool valid = false;
                    return valid;
                },
                // Outcome
                (ref GameWorld world) => {
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.Append("Perform the game logic here");
                    return sbResult.ToString();
                }
            ),
        };
    }
}
