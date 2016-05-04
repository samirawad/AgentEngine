using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    // An outcome effects a change in gamestate. This delegate determines if the outcome is valid under the current gamestate
    public delegate bool OutcomeIsValidDelegate(GameAction sourceEvent, List<GameEntity> entities);

    // The description of the outcome may be blank, or different depending upon the game state
    public delegate string OutcomeDescriptionDelegate(GameAction sourceEvent, List<GameEntity> entities);

    // Holds the function which alters gamestate, it will be run if the outcome is valid
    public delegate string PerformOutcomeDelegate(ref GameWorld world);

    public class Outcome
    {
        public string ID;

        public OutcomeDescriptionDelegate GetDescription;

        public OutcomeIsValidDelegate IsValid;

        public PerformOutcomeDelegate PerformOutcome;
    }
}
