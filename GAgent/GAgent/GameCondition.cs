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
            // The validity of a condtion is either the validcondtion, or the operation on the subconditions.
            // Console.WriteLine("Condtion '" + Description + "': ");
            bool result;
            if(Selector != null)
            {
                Dictionary<string, List<GameAgent>> selected = Selector.GetAgents(w);
                if(selected == null)
                {
                    result = false;
                }
                else
                {
                    result = ValidCondition(Selector.GetAgents(w), w);
                }
            }
            else
            {
                result = ValidCondition(null, w);
            }
            return result;
        }
    }
}
