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

        public static Condition notTravelling = new Condition("condition_nottraveling", "player is not travelling",
            playerSelector,
            (selector, world) =>
            {
                return selector["player"] != null ? selector["player"].S["Destination"] == null ? true : false : false;
            });

        public static Condition travelSelected = new Condition("condition_travelselected", "player selected traveling",
            null,
            (selector, world) =>
            {
                return world.IsCurrentAction("action_travel");
            });

        public static List<GameAction> Actions = new List<GameAction>()
        {
           // While it's possible to add new game objects in this fashion, it's probably better to do it in
           // some routine that sets up the whole world.
           new GameAction(new GameActionParams(){
               ID = "action_init",
               Description = w => {return "Begin Initialization action";},
               ValidityCondition = new Condition("cond_init", "the player does not exist",
                   playerSelector,
                   (selector, world) => {
                       return selector["player"] == null;
                   })
           }),
           new GameAction(new GameActionParams(){
                ID = "action_travel",
                Description = w => {return "Establish a destination";},
                ValidityCondition = notTravelling
           }),
           new GameAction(new GameActionParams(){
                ID = "action_travel_dungeon",
                Description = w => {return "Travel to the dungeon";},
                ValidityCondition = travelSelected,
                StringParams = new Dictionary<string,string>(){ 
                    { "destination", "dungeon" } 
                }
           }),
           new GameAction(new GameActionParams(){
                ID = "action_travel_temple",
                Description = w => {return "Travel to the temple";},
                ValidityCondition = travelSelected,
                StringParams = new Dictionary<string,string>(){ 
                    { "destination", "temple" } 
                }
           }),
           new GameAction(new GameActionParams(){
                ID = "action_travel_market",
                Description = w => {return "Travel to the market";},
                ValidityCondition = travelSelected,
                StringParams = new Dictionary<string,string>(){ 
                    { "destination", "market" } 
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
                ValidityCondition = new Condition("cond_outcome_travelselected", "player chose to travel",
                    null,
                    (selector, world) => {
                        return world.IsCurrentAction("action_travel");
                    }),
                OutcomeFunction = (ref GameWorld world) => {
                    return "The player considers a travel destination";
                }
            })
        };


    }
}
