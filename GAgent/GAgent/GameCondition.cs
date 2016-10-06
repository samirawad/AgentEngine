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
            Console.WriteLine(Description);
            result = Operator(conditions, world);
            Console.WriteLine(result == true ? " - success!" : " - failure!");
            return result;
        }
    }

    public class AgentSelector
    {
        private string ID;
        private string Description;
        private AgentSelectorDelegate Selector;

        public AgentSelector(string inId, string inDesc, AgentSelectorDelegate inSelector)
        {
            ID = inId;
            Description = inDesc;
            Selector = inSelector;
        }

        public Dictionary<string, GameAgent> GetAgents(GameWorld world)
        {
            Dictionary<string, GameAgent> result = null;
            Console.WriteLine(Description);
            result = Selector(world);
            Console.WriteLine(result != null ? " - success!" : " - failure!");
            return result;
        }
    }

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
            Console.Write("Resolving condition: " + Description);
            bool result;
            if (ValidCondition != null)
            {
                result = ValidCondition(Selector.GetAgents(w), w);
            }
            else
            {
                result = Operation.IsValid(Conditions, w);
            }
            Console.WriteLine(result == true ? " - success! " : " - failure! ");
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
            ConditionOperator andOperator = new ConditionOperator("and", "All conditions must be valid",
                (conditions, world) =>
                {
                    // for the 'and' operator, all conditions must be valid
                    return conditions.All(c => c.IsValid(world));
                });

            AgentSelector playerSelector = new AgentSelector("playerSelector", "Selects the player from the game world", 
                (world) =>
                {
                    // we could place logging in here so we can see if selectors fail
                    Console.WriteLine("Selecting player.");
                    return new Dictionary<string, GameAgent>(){
                            {"player", world.AllEntities["player"]}
                        };
                });
            

            Condition atRest = new Condition("atrest", "The player is at rest",
                playerSelector,
                (selection, world) =>
                {
                    return selection["player"].S["CurrentAction"] == "resting" ? true : false;
                });
            Condition validDestination = new Condition("validdest", "The desination is valid",
                playerSelector,
                (selection, world) =>
                {
                    return selection["player"].S["Location"] != selection["player"].S["Destination"] ? true : false;
                });
            Condition notTravelling = new Condition("notTravelling", "The player is not currently travelling",
                playerSelector,
                (selection, world) =>
                {
                    return selection["player"].S["Destination"] == null ? true : false;
                });
            Condition travelSelected = new Condition("travelSelected", "Travel has begun",
                null,
                (selection, world) =>
                {
                    return world.IsCurrentAction("beginTravel");
                });
            Condition worldStateGood = new Condition("goodWorldState", "I can check the last action",
                null,
                (selection, world) =>
                {
                    return world.CurrentAction.Description(world).Contains("butts"); // this is weird. 
                });
            Condition canTravel = new Condition("travelOperator", "The player can travel: all must be true",
                andOperator,
                new List<Condition>(){
                atRest, validDestination, notTravelling, travelSelected
            });

            bool validTravel = canTravel.IsValid(new GameWorld());

            //Condition validForTravel = 
        }
    }

}
