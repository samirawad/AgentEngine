using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent.StandardEvents
{
    public static class ExpandoTestEvents
    {
        public static List<GameAction> GameEvents = new List<GameAction>() { 
            new GameAction()
            {
                ID = "id",
                ShowOutcomes = false,
                Description = (world) => { return "This action initializes the world"; },
                IsValid = (world) => {
                    return false;
                }
            },
        };

        public static List<Outcome> GameEventOutcomes = new List<Outcome>() { 
            new Outcome()
            {
                GetDescription = (source, world) => { return "Initializing the world"; },
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
