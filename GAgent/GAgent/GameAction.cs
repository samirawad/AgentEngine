using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    // Holds the function which determines if this event is currently valid
    public delegate bool EventIsValidDelegate(GameWorld world);

    public delegate string EventTextDelegate(GameWorld world);

    /*
     * The idea behind a game action is that it's selectable.  It's available based on the state of the game world, and representes a decision point 
     * for the player.
     */
    public class GameAction
    {
        public string ID;

        public HashSet<String> Tags;

        public EventTextDelegate Description; // This is what is displayed at the selection stage.

        public EventTextDelegate Detail; // When an action is selected, before it is confirmed, more detail is provided here.

        public bool ShowOutcomes;

        // Is it possible to take this action now?
        public EventIsValidDelegate IsValid;

        public string ListOutcomes(GameWorld world, List<Outcome> AllOutcomes, Dictionary<string, GameAgent> AllEntities)
        {
            // List the possible outcomes for this action
            // We might not want to show all the outcomes, depending on the action.
            StringBuilder sbOutput = new StringBuilder();
            if (ShowOutcomes)
            {
                Outcome[] validOutcomes = AllOutcomes.Where(o => o.IsValid(this, AllEntities)).ToArray();
                foreach (Outcome currOutcome in validOutcomes)
                {
                    sbOutput.AppendLine(currOutcome.GetDescription(this, AllEntities));
                }
            }
            else
            {
                return Detail != null ? Detail(world) : "";
            }
            return sbOutput.ToString();
        }

        public string SelectOutcome(GameWorld world)
        {
            Outcome[] validOutcomes = world.AllOutcomes.Where(o => o.IsValid(this, world.AllEntities)).ToArray();
            if (validOutcomes.Length > 0)
            {
                Outcome selected = validOutcomes[world.RND.Next(validOutcomes.Length)];
                return selected.PerformOutcome(ref world);
            }
            else
            {
                return "No possible outcomes for this event.";
            }
        }
    }
}
