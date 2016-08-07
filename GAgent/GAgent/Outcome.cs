using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    // An outcome effects a change in gamestate. This delegate determines if the outcome is valid under the current gamestate
    public delegate bool OutcomeIsValidDelegate(GameWorld world);

    // The description of the outcome may be blank, or different depending upon the game state
    public delegate string OutcomeDescriptionDelegate(GameWorld world);

    // Holds the function which alters gamestate, it will be run if the outcome is valid
    public delegate string PerformOutcomeDelegate(ref GameWorld world);

    public class Outcome
    {
        public Outcome(string inID, OutcomeDescriptionDelegate descriptionDelegate, OutcomeIsValidDelegate isOutcomeValidDelegate, PerformOutcomeDelegate performOutcomeDelegate)
        {
            _ID = inID;
            _Tags = new HashSet<string>();
            _GetDescription = descriptionDelegate;
            _IsValid = isOutcomeValidDelegate;
            _PerformOutcome = performOutcomeDelegate;
        }

        public Outcome(string inID, HashSet<string> inTags, OutcomeDescriptionDelegate descriptionDelegate, OutcomeIsValidDelegate isOutcomeValidDelegate, PerformOutcomeDelegate performOutcomeDelegate)
        {
            _ID = inID;
            _Tags = inTags;
            _GetDescription = descriptionDelegate;
            _IsValid = isOutcomeValidDelegate;
            _PerformOutcome = performOutcomeDelegate;
        }

        private string _ID;
        private HashSet<string> _Tags;
        private OutcomeIsValidDelegate _IsValid;
        private PerformOutcomeDelegate _PerformOutcome;
        private OutcomeDescriptionDelegate _GetDescription;

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
            return _IsValid(world);
        }

        public string PerformOutcome(ref GameWorld world)
        {
            string result = _PerformOutcome(ref world);
            world.CurrentOutcome = this;
            return result;
        }

    }
}
