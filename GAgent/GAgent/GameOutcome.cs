using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    // The description of the outcome may be blank, or different depending upon the game state
    public delegate string OutcomeDescriptionDelegate(GameWorld world);

    // Holds the function which alters gamestate, it will be run if the outcome is valid
    public delegate string PerformOutcomeDelegate(ref GameWorld world);

    public struct OutcomeParams
    {
        public string OutcomeID;
        public HashSet<string> Tags;
        public OutcomeDescriptionDelegate DescriptionFunction;
        public Condition ValidityCondition;
        public PerformOutcomeDelegate OutcomeFunction;
    }

    public class GameOutcome
    {
        public GameOutcome(OutcomeParams o)
        {
            _ID = o.OutcomeID;
            _Tags = o.Tags != null ? o.Tags : new HashSet<string>();
            _GetDescription = o.DescriptionFunction;
            _IsValidCondition = o.ValidityCondition;
            _PerformOutcome = o.OutcomeFunction;
        }

        private string _ID;
        private HashSet<string> _Tags;
        private Condition _IsValidCondition;
        private PerformOutcomeDelegate _PerformOutcome;
        private OutcomeDescriptionDelegate _GetDescription;
        
        public Dictionary<string, GameAgent> AgentParams; // When an outcome is selected, certain agents might be under consideration
        public Dictionary<string, long> N = new Dictionary<string, long>();  // numeric values, used as parameters to be accessed by conditions and outcomes
        public Dictionary<string, string> S = new Dictionary<string, string>(); // string values, used as parameters to be accessed by conditions and outcomes

        public string ID { 
            get { return _ID; }
        }

        public HashSet<string> Tags
        {
            get { return _Tags; }
        }

        public bool HasTag(string toFind)
        {
            if(_Tags == null)
            {
                return false;
            }
            else
            {
                return _Tags.Contains(toFind);
            }
        }

        public string GetDescription(GameWorld world)
        {
            return _GetDescription(world);
        }

        public bool IsValid(GameWorld world)
        {
            // It is invalid for an outcome to call itself.
            if(world.CurrentOutcome == this)
            {
                //Console.WriteLine("Outcome '" + GetDescription(world) + "' tried to call itself!");
                return false;
            }
            else
            {
                Console.WriteLine("Outcome '" + GetDescription(world) + "':");
                bool result = _IsValidCondition.IsValid(world);
                Console.WriteLine("Outcome '" + GetDescription(world) + "': " + (result ? " - TRUE " : " - FALSE "));
                return result;
            }
            
        }

        public string PerformOutcome(ref GameWorld world)
        {
            string result = _PerformOutcome(ref world);
            world.CurrentOutcome = this;
            return result;
        }

    }
}
