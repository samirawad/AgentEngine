using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent.StandardEvents
{
    public static class DungeonEvents
    {
        #region Events
        public static List<GameAction> GameEvents = new List<GameAction>() {
            new GameAction()
            {
                ID = "DungeonStart",
                ShowOutcomes = false,
                Description = (world) => { return "The party arrives at the dungeon and begins their exploration"; },
                RequiredEntities = new List<string>(){
                    "player"
                },
                IsValidDel = (world) => {
                    GameAgent player =  world.AllEntities.ContainsKey("player") ? world.AllEntities["player"] : null;
                    bool atDungeon = player.S["Location"] == "dungeon";
                    return atDungeon;
                }
            },
        };
        #endregion

        #region Event Outcomes
        public static List<Outcome> GameEventOutcomes = new List<Outcome>() {
            new Outcome()
            {
                GetDescription = (source, world) => { return "A possible encounter..."; },
                IsValid = (source, world) => {
                    return source.ID == "DungeonStart";
                },
                PerformOutcome = (ref GameWorld world) => {
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.Append("The party encounters a monster!");
                    return sbResult.ToString();
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "A possible encounter..."; },
                IsValid = (source, world) => {
                    return source.ID == "DungeonStart";
                },
                PerformOutcome = (ref GameWorld world) => {
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.Append("The party encounters an NPC!");
                    return sbResult.ToString();
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "A possible encounter..."; },
                IsValid = (source, world) => {
                    return source.ID == "DungeonStart";
                },
                PerformOutcome = (ref GameWorld world) => {
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.Append("The party encounters an interesting dungeon feature!");
                    return sbResult.ToString();
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "A possible encounter..."; },
                IsValid = (source, world) => {
                    return source.ID == "DungeonStart";
                },
                PerformOutcome = (ref GameWorld world) => {
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.Append("The party has an interaction!");
                    return sbResult.ToString();
                }
            },
        };
        #endregion

    }
}
