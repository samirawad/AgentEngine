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
}
