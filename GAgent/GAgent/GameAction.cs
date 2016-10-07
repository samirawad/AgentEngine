using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    public delegate string EventTextDelegate(GameWorld world);

    public struct GameActionParams
    {
        public string ID;
        public HashSet<string> Tags;
        public EventTextDelegate Description;
        public EventTextDelegate Detail;
        public Dictionary<string, GameAgent> AgentParams;
        public Dictionary<string, long> NumericParams;
        public Dictionary<string, string> StringParams;
        public bool ShowOutcomes;
        public Condition ValidityCondition;
    }
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
        public Dictionary<string, long> N = new Dictionary<string, long>();  // numeric values, used as parameters to be accessed by conditions and outcomes
        public Dictionary<string, string> S = new Dictionary<string, string>(); // string values, used as parameters to be accessed by conditions and outcomes

        private string _id;
        private HashSet<string> _tags;                      // I guess these would help if we want to have certain groups of actions be valid by tag?  I haven't used this yet?
        private EventTextDelegate _description;             // This is what is displayed at the selection stage.
        private EventTextDelegate _detail;                  // When an action is selected, before it is confirmed, more detail is provided here.
        private Dictionary<string, GameAgent> _agentParams; // When an action is selected, certain agents might be under consideration
        private bool _showoutcomes;
        private Condition _validitycondition;

        public GameAction(GameActionParams g)
        {
            _id = g.ID;
            _tags = g.Tags;
            _description = g.Description;
            _detail = g.Detail;
            _agentParams = g.AgentParams;
            _showoutcomes = g.ShowOutcomes;
            _validitycondition = g.ValidityCondition;
        }
        
        public bool IsValid(GameWorld world)
        {
            Console.WriteLine("Action '" + Description(world) + "':");
            bool result = _validitycondition.IsValid(world);
            Console.WriteLine("Action '" + Description(world) + "': " + (result ? " - TRUE " : " - FALSE "));
            return result;
        }

        public string Description(GameWorld world)
        {
            return _description(world);
        }

        public string ID
        {
            get { return _id; }
        }

        public string ListOutcomes(GameWorld world)
        {
            // List the possible outcomes for this action
            // We might not want to show all the outcomes, depending on the action.
            StringBuilder sbOutput = new StringBuilder();
            if (_showoutcomes)
            {
                GameOutcome[] validOutcomes = world.AllGameOutcomes.Where(o => o.IsValid(world)).ToArray();
                foreach (GameOutcome currOutcome in validOutcomes)
                {
                    sbOutput.AppendLine(currOutcome.GetDescription(world));
                }
            }
            else
            {
                return _detail != null ? _detail(world) : "";
            }
            return sbOutput.ToString();
        }

        public string SelectOutcome(GameWorld world)
        {
            /*
             *  Outcomes may chain together by ID.  Once an outcome is selected and it's effect on 
             *  the world performed, we need to ensure that no further outcomes are valid and require performing.
             */
            GameOutcome[] validOutcomes = world.AllGameOutcomes.Where(o => o.IsValid(world)).ToArray();
            Console.WriteLine("Current valid outcomes: ");
            foreach (var o in validOutcomes)
            {
                Console.WriteLine(" - " + o.GetDescription(world));
            }
            StringBuilder result = new StringBuilder();
            while(validOutcomes.Length > 0)
            {
                // When multiple outcomes are valid, one is simply selected randomly
                // In the future I was thinking about adding weights to outcomes so that
                // some are more likely than others.
                GameOutcome selected = validOutcomes[world.RND.Next(validOutcomes.Length)];

                // The outcome is performed.  This is what actually alters the gameworld state.
                result.AppendLine(selected.PerformOutcome(ref world));

                // logging crap.  move this to a dedicated logger
                Console.WriteLine("Current outcome log: ");
                Console.WriteLine(result.ToString());
                
                // Determine 
                validOutcomes = world.AllGameOutcomes.Where(o => o.IsValid(world)).ToArray();
                
                Console.WriteLine("New potential outcomes: ");
                if(validOutcomes.Length == 0)
                {
                    Console.WriteLine("No further outcomes for this event.  Continuing to next game action.");
                }
                else foreach (var o in validOutcomes)
                {
                    Console.WriteLine(" - " + o.GetDescription(world));
                }
            }
            Console.ReadKey();
            return result.ToString();
        }
    }
}
