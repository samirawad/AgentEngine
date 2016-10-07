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
        private ConditionOperator Operation;
        private ConditonDelegate ValidCondition;
        private AgentSelector Selector;
        private List<Condition> Conditions;
        private string Description;
        private string ID;

        public Condition(string id, string inDescription, AgentSelector inSelector, ConditonDelegate inDel)
        {
            ID = id;
            Description = inDescription;
            ValidCondition = inDel;
            Selector = inSelector;
            Conditions = null;
        }

        public Condition(string id, string inDescription, ConditionOperator inOperation, List<Condition> inConditions)
        {
            ID = id;
            Description = inDescription;
            Conditions = inConditions;
            Operation = inOperation;
            ValidCondition = null;
        }

        public bool IsValid(GameWorld w)
        {
            // The validity of a condtion is either the validcondtion, or the operation on the subconditions.
            // Console.WriteLine("Condtion '" + Description + "': ");
            bool result;
            if (ValidCondition != null)
            {
                if(Selector != null)
                {
                    result = ValidCondition(Selector.GetAgents(w), w);
                }
                else
                {
                    result = ValidCondition(null, w);
                }
            }
            else
            {
                result = Operation.IsValid(Conditions, w);
            }
            Console.WriteLine("    Condtion '" + Description + "': " + (result == true ? " - TRUE " : " - FALSE "));
            return result;
        }
    }
}
