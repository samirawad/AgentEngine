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
     * 
     * Determining if an action is available, or an outcome is possible is broken down into two parts.
     * - A set of entities which must be selected for checking
     * - Checking the status / relationships of these entities.
     */
    public class GameAction
    {
        public string ID;

        public HashSet<String> Tags;

        public EventTextDelegate Description; // This is what is displayed at the selection stage.

        public EventTextDelegate Detail; // When an action is selected, before it is confirmed, more detail is provided here.

        public List<String> RequiredEntities; // These agents are required for the action to be valid

        public bool ShowOutcomes;

        public EventIsValidDelegate IsValidDel;
        
        public bool IsValid(GameWorld world)
        {
            bool allAgentsExist = RequiredEntities != null ? RequiredEntities.All(r => world.AllEntities.ContainsKey(r)) : true ;
            return allAgentsExist && IsValidDel(world);
        }

        public string ListOutcomes(GameWorld world)
        {
            // List the possible outcomes for this action
            // We might not want to show all the outcomes, depending on the action.
            StringBuilder sbOutput = new StringBuilder();
            if (ShowOutcomes)
            {
                Outcome[] validOutcomes = world.AllOutcomes.Where(o => o.IsValid(world)).ToArray();
                foreach (Outcome currOutcome in validOutcomes)
                {
                    sbOutput.AppendLine(currOutcome.GetDescription(world));
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
            /*
             *  Outcomes may chain together by ID.  Once an outcome is selected and it's effect on 
             *  the world performed, we need to ensure that no further outcomes are valid and require performing.
             */
            Outcome[] validOutcomes = world.AllOutcomes.Where(o => o.IsValid(world)).ToArray();
            Console.WriteLine("Current valid outcomes: ");
            foreach (var o in validOutcomes)
            {
                Console.WriteLine(" - " + o.GetDescription(world));
            }
            StringBuilder result = new StringBuilder();
            while(validOutcomes.Length > 0)
            {
                Outcome selected = validOutcomes[world.RND.Next(validOutcomes.Length)];
                result.AppendLine(selected.PerformOutcome(ref world));
                Console.WriteLine("Current outcome log: ");
                Console.WriteLine(result.ToString());
                validOutcomes = world.AllOutcomes.Where(o => o.IsValid(world)).ToArray();
                Console.WriteLine("New potential outcomes: ");
                if(validOutcomes.Length == 0)
                {
                    Console.WriteLine("No further outcomes for this event.  Continuing to next game action.");
                }
                else foreach (var o in validOutcomes)
                {
                    Console.WriteLine(" - " + o.GetDescription(world));
                }
                Console.ReadKey();
            }
            return result.ToString();
        }
    }
}
