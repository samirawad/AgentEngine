using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    // A condition delegate examines the selection to determine if it meets a condition.
    public delegate bool ConditonDelegate(Dictionary<string, GameAgent> selection, GameWorld g);

    public class Condition
    {
        private ConditonDelegate ValidCondition;
        private AgentSelector Selector;
        private string Description;
        private string ID;
        private bool _debug = false;

        public Condition(string id, string inDescription, AgentSelector inSelector, ConditonDelegate inDel)
        {
            ID = id;
            Description = inDescription;
            ValidCondition = inDel;
            Selector = inSelector;
        }

        public Condition(string id, string inDescription, AgentSelector inSelector, ConditonDelegate inDel, bool Debug)
        {
            ID = id;
            Description = inDescription;
            ValidCondition = inDel;
            Selector = inSelector;
            _debug = Debug;
        }

        public bool IsValid(GameWorld w)
        {
            // The validity of a condtion is either the validcondtion, or the operation on the subconditions.
            // Console.WriteLine("Condtion '" + Description + "': ");
            bool result;
            if(Selector != null)
            {
                result = ValidCondition(Selector.GetAgents(w), w);
            }
            else
            {
                result = ValidCondition(null, w);
            }
            if(_debug) Console.WriteLine("    Condtion '" + Description + "': " + (result == true ? " - TRUE " : " - FALSE "));
            return result;
        }
    }
}
