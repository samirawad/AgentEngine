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
            new Outcome(new OutcomeParams() {
                OutcomeID = "outcomeID",
                DescriptionFunction = (w) => {
                    return "description of the outcome";
                },
                ValidityFunction = (w) => {
                    bool valid = false;
                    return valid;
                },
                OutcomeFunction = (ref GameWorld w) => {
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.Append("Perform the game logic here");
                    return sbResult.ToString();
                }
            }),
        };
    }
}
