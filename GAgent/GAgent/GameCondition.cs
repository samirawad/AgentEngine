using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    // A condition delegate examines the selection to determine if it meets a condition.
    public delegate bool ConditonDelegate(Dictionary<string, List<GameAgent>> selection, GameWorld g);

    public class Condition
    {
        private ConditonDelegate ValidCondition;
        private AgentSelector Selector;
        private string Description;
        private string ID;

        public Condition(string id, string inDescription, AgentSelector inSelector, ConditonDelegate inDel)
        {
            ID = id;
            Description = inDescription;
            ValidCondition = inDel;
            Selector = inSelector;
        }

        public bool IsValid(GameWorld w)
        {
            return Selector != null ? ValidCondition(Selector.GetAgents(w), w) : ValidCondition(null, w);
        }
    }
}
