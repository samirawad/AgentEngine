using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{

    // A selector delegate returns a selection of agents from the gameworld, accessible through a dictionary
    public delegate Dictionary<string, GameAgent> AgentSelectorDelegate(GameWorld world);

    // A condition delegate examines the selection to determine if it meets a condition.
    public delegate bool ConditonDelegate(Dictionary<string, GameAgent> selection, GameWorld g);

    // Operates on a list of condtions, mapping them to a boolean
    public delegate bool ConditionOperatorDelegate(List<Condition> conditions, GameWorld g);

    /*
     * Hopefully with this structure we can create nested condition which are easy to log.
     */

    public class Condition
    {
        private ConditionOperatorDelegate Operation;
        private ConditonDelegate ValidCondition;
        private AgentSelectorDelegate Selector;
        private List<Condition> Conditions;
        private string Description;

        public Condition(string inDescription, AgentSelectorDelegate inSelector, ConditonDelegate inDel)
        {
            Description = inDescription;
            ValidCondition = inDel;
            Selector = inSelector;
            Conditions = null;
        }

        public Condition(string inDescription, ConditionOperatorDelegate inOperation, List<Condition> inConditions)
        {
            Description = inDescription;
            Conditions = inConditions;
            Operation = inOperation;
            ValidCondition = null;
        }

        public bool IsValid(GameWorld w)
        {
            // The validity of a condtion is either the validcondtion, or the operation on the subconditions.
            Console.Write("Resolving condition: " + Description);
            bool result;
            if(ValidCondition != null)
            {
                result = ValidCondition(Selector(w), w);
            }
            else
            {
                result = Operation(Conditions, w);
            }
            
            if (result == true)
                Console.WriteLine(" - success!");
            else
                Console.WriteLine(" - failed!");

            return result;
        }
    }

    /*
     * This will be a success if we can view a meaningful breakdown of the condition logic in the logs.
     * This is important to further development, as we will be able to add new event libraries and test them easily 
     *
     */
    public class ConditionTester
    {
        public ConditionTester()
        {
            ConditionOperatorDelegate andOperator = (conditions, world) =>
            {
                // for the 'and' operator, all conditions must be valid
                return conditions.All(c => c.IsValid(world));
            };

            AgentSelectorDelegate playerSelector = (world) =>
            {
                // we could place logging in here so we can see if selectors fail
                Console.WriteLine("Selecting player.");
                return new Dictionary<string, GameAgent>(){
                        {"player", world.AllEntities["player"]}
                    };
            };

            Condition atRest = new Condition("The player is at rest", 
                playerSelector,
                (selection, world) =>
                {
                    return selection["player"].S["CurrentAction"] == "resting" ? true : false;
                });
            Condition validDestination = new Condition("The desination is valid", 
                playerSelector,
                (selection, world) =>
                {
                    return selection["player"].S["Location"] != selection["player"].S["Destination"] ? true : false;
                });
            Condition notTravelling = new Condition("The player is not currently travelling",
                playerSelector,
                (selection, world) =>
                {
                    return selection["player"].S["Destination"] == null ? true : false;
                });
            Condition travelSelected = new Condition("Travel has begun",
                null,
                (selection, world) =>
                {
                    return world.IsCurrentAction("beginTravel");
                });
            Condition canTravel = new Condition("The player can travel: all must be true", 
                andOperator, 
                new List<Condition>(){
                atRest, validDestination, notTravelling, travelSelected
            });

            bool validTravel = canTravel.IsValid(new GameWorld());

            //Condition validForTravel = 
        }
    }

}
