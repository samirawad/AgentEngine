using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent.StandardEvents
{
    public static class TravelActions
    {
        public static AgentSelector playerSelector = new AgentSelector("selector_player", "Selects the player",
            w =>
            {
                return new Dictionary<string, GameAgent>() { { "player", w.AllAgents.ContainsKey("player") ? w.AllAgents["player"] : null } };
            });

        public static Condition playerDoesntExist = new Condition("condition_playerexists", "the player does not exist",
            playerSelector,
            (selector, world) =>
            {
                return selector["player"] == null;
            });

        public static Condition notTravelling = new Condition("condition_nottraveling", "player is not travelling",
            null,
            (selector, world) =>
            {
                return !isTravelling.IsValid(world);
            });

        public static Condition isTravelling = new Condition("condition_nottraveling", "player is travelling",
            playerSelector,
            (selector, world) =>
            {
                // player is travelling if the current action contains a destination parameter
                return 
                    world.CurrentAction != null ? 
                        world.CurrentAction.S != null ?
                            world.CurrentAction.S.ContainsKey("Destination") 
                        : false 
                    : false;
            });

        public static Condition selectDestination = new Condition("condition_travelselected", "player is selecting a destination",
            null,
            (selector, world) =>
            {
                return world.IsCurrentAction("action_selectdestination");
            });

        public static Condition readyToTravel = new Condition("condition_travelselected", "player is ready to begin travelling",
            null,
            (selector, world) =>
            {
                // player is ready to travel if they aren't currently travelling and they haven't selected a destination
                return 
                    notTravelling.IsValid(world) && 
                    !selectDestination.IsValid(world) &&
                    !playerDoesntExist.IsValid(world);
            });

        public static List<GameAction> Actions = new List<GameAction>()
        {
           // While it's possible to add new game objects in this fashion, it's probably better to do it in
           // some routine that sets up the whole world.
           new GameAction(new GameActionParams(){
               ID = "action_init",
               Description = w => {return "Begin Initialization action";},
               ValidityCondition = playerDoesntExist
           }),
           new GameAction(new GameActionParams(){
                ID = "action_selectdestination",
                Description = w => {return "Establish a destination";},
                ValidityCondition = readyToTravel
            }),
           new GameAction(new GameActionParams(){
                ID = "action_travel_dungeon",
                Description = w => {return "Travel to the dungeon";},
                ValidityCondition = selectDestination,
                StringParams = new Dictionary<string,string>(){ 
                    { "Destination", "dungeon" } 
                }
           }),
           new GameAction(new GameActionParams(){
                ID = "action_travel_temple",
                Description = w => {return "Travel to the temple";},
                ValidityCondition = selectDestination,
                StringParams = new Dictionary<string,string>(){ 
                    { "Destination", "temple" } 
                }
           }),
           new GameAction(new GameActionParams(){
                ID = "action_travel_market",
                Description = w => {return "Travel to the market";},
                ValidityCondition = selectDestination,
                StringParams = new Dictionary<string,string>(){ 
                    { "Destination", "market" } 
                }
           })

        };

        public static List<GameOutcome> Outcomes = new List<GameOutcome>()
        {
            new GameOutcome(new OutcomeParams()
            {
                OutcomeID = "outcome_startadv",
                DescriptionFunction = (world) => { return "The player will embark upon his adventure!"; } ,
                ValidityCondition = new Condition("cond_outcome_startadv", "the init action was executed",
                    null,
                    (selector, world) => {
                        return world.IsCurrentAction("action_init") ? true : false;
                    }),
                OutcomeFunction = (ref GameWorld world) => {
                    world.AllAgents.Add("player", new GameAgent() { 
                        // Initialize the player
                        S = new Dictionary<string,string>() {
                            {"Name","player"},
                            {"Location", "tavern"},
                            {"Destination", null},
                            {"CurrentAction","resting"}
                        }
                    });
                    return "the adventurer arrives in town, entering the tavern.";
                }
            }),
            new GameOutcome(new OutcomeParams(){
                OutcomeID = "outcome_selecttravel",
                DescriptionFunction = (world) => { return "The player decides upon a travel destination"; },
                ValidityCondition = selectDestination,
                OutcomeFunction = (ref GameWorld world) => {
                    return "The player considers his destination.";
                }
            }),
            new GameOutcome(new OutcomeParams(){
                OutcomeID = "outcome_traveling",
                DescriptionFunction = (world) => { return "The player continues on his journey"; },
                ValidityCondition = isTravelling,
                OutcomeFunction = (ref GameWorld world) => {
                    string dest = world.CurrentAction.S["Destination"];
                    return "The player continues on his journey to: " + dest;
                }
            })

        };


    }
}
