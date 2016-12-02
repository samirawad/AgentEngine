using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent.Judgements
{

    public class Occurance
    {
        public string Description;  // This describes the action of the occurance.  It will be filled out by the Event outcome

        public Dictionary<string, HashSet<GameAgent>> OccuranceRoles;  // The roles that each entity fulfilled during the occurance
    }

    /*
     * A note about the idea of occurence roles.  We might have a situation where agents might have different views about the roles of an occurence.  If for example, 
     * Agent a is replacing an object on a pedestal, He might look like a theif to one agent, but a custodian to another depending on the knowledge the agent possesses.
     */
}
