using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    // Operates on a list of condtions, mapping them to a boolean
    public delegate bool ConditionOperatorDelegate(List<Condition> conditions, GameWorld g);

    public class ConditionOperator
    {
        private string ID;
        private string Description;
        private ConditionOperatorDelegate Operator;

        public ConditionOperator(string inId, string inDesc, ConditionOperatorDelegate inOperator)
        {
            ID = inId;
            Description = inDesc;
            Operator = inOperator;
        }

        public bool IsValid(List<Condition> conditions, GameWorld world)
        {
            bool result = false;
            Console.WriteLine("Operator '" + Description + "': ");
            result = Operator(conditions, world);
            Console.WriteLine("Operator '" + Description + "' result: " + (result == true ? " - TRUE " : " - FALSE "));
            return result;
        }
    }
}
