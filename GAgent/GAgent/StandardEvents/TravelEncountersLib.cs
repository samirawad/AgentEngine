using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent.StandardEvents
{
    public static class TravelEncountersLib
    {
        public static List<GameAction> GameEvents = new List<GameAction>() { 
            new GameAction()
            {
                ID = "Encounter",
                ShowOutcomes = false,
                Description = "An encounter occurs...",
                IsValid = (world) => {
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
                    bool hasEncounter = player != null ?
                        player.S["Encounter"] == "true" ? true : false : false;
                    return hasEncounter;
                }
            },
        };

        public static List<Outcome> GameEventOutcomes = new List<Outcome>() { 
            new Outcome()
            {
                GetDescription = (source, entities) => { return "Friendly encounter."; },
                IsValid = (source, entities) => {
                    bool valid = source.ID == "Encounter" ? true : false;
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
                    if (!player.S.ContainsKey("Location"))
                    {
                        player.S.Add("Location", player.S["Destination"]);
                    }
                    else
                    {
                        player.S["Location"] =  player.S["Destination"];
                    }
                    player.S["Destination"] = null;
                    player.S["Encounter"] = "false";
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.AppendLine("The adventurer has an encounter with a FRIENDLY NPC!");
                    sbResult.AppendLine("He is now at: " + player.S["Location"]);
                    return sbResult.ToString();
                }
            },
            new Outcome()
            {
                GetDescription = (source, entities) => { return "A hostile encounter."; },
                IsValid = (source, entities) => {
                    bool valid = source.ID == "Encounter" ? true : false;
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
                    if (!player.S.ContainsKey("Location"))
                    {
                        player.S.Add("Location", player.S["Destination"]);
                    }
                    else
                    {
                        player.S["Location"] =  player.S["Destination"];
                    }
                    player.S["Destination"] = null;
                    player.S["Encounter"] = "false";
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.AppendLine("The adventurer has an encounter with a HOSTILE MONSTER!");
                    sbResult.AppendLine("He is now at: " + player.S["Location"]);
                    return sbResult.ToString();
                }
            },
        };
    }
}
