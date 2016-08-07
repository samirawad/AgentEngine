using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent.StandardEvents
{
    public static class TravelingEvents
    {
        private static bool ReadyToTravel(GameWorld world, string currentLocation)
        {
            GameAgent player = world.AllEntities["player"];
            bool atRest = player.S["CurrentAction"] == "resting" ? true : false;
            bool notAtCurrentLocation = player.S["Location"] != currentLocation ? true : false;
            bool notTravelling = player.S["Destination"] == null ? true : false;
            bool notAtDungeon = player.S["Location"] != "dungeon";
            return atRest && notAtCurrentLocation && notTravelling && notAtDungeon;
        }

        public static List<GameAction> GameEvents = new List<GameAction>() { 
            new GameAction()
            {
                ID = "Init",
                ShowOutcomes = true,
                Description = (world) => { return "Begin the adventure!"; },
                IsValidDel = (world) => { 
                    // Initialization action. Valid if the player hasn't yet been created.
                    return !world.AllEntities.ContainsKey("player");
                }
            },
            new GameAction()
            {
                ID = "GoDungeon",
                ShowOutcomes = true,
                Description = (world) => { return "Go to the Dungeon"; },
                RequiredEntities = new List<string>(){
                    "player"
                },
                IsValidDel = (world) => {
                    return ReadyToTravel(world, "dungeon");
                }
            },
            new GameAction()
            {
                ID = "GoTavern",
                ShowOutcomes = true,
                Description = (world) => { return "Go to the Tavern"; },
                RequiredEntities = new List<string>(){
                    "player"
                },
                IsValidDel = (world) => { 
                    return ReadyToTravel(world, "tavern");
                }
            },
           new GameAction()
            {
                ID = "GoMarket",
                ShowOutcomes = true,
                Description = (world) => { return "Go to the Market"; },
                RequiredEntities = new List<string>(){
                    "player"
                },
                IsValidDel = (world) => { 
                    return ReadyToTravel(world, "market");
                }
            },
            new GameAction()
            {
                ID = "GoTemple",
                ShowOutcomes = true,
                Description = (world) => { return "Go to the Temple"; },
                RequiredEntities = new List<string>(){
                    "player"
                },
                IsValidDel = (world) => { 
                    return ReadyToTravel(world, "temple");
                }
            },
            new GameAction()
            {
                ID = "Travel",
                ShowOutcomes = true,
                Detail = (world) => { return "an encounter might occur..."; },
                Description = (world) => { return "The adventurer travels towards his destination..."; },
                IsValidDel = (world) => { 
                    // Is valid if the player has a current destination
                    GameAgent player =  world.AllEntities.ContainsKey("player") ? world.AllEntities["player"] : null;
                    if (player == null) return false;
                    bool atRest = player.S["CurrentAction"] == "resting" ? true : false;
                    bool isTravelling = player != null ?
                        player.S["Destination"] != null ? true : false : false;
                    bool noEncounter = player != null ?
                        player.S["Encounter"] != "true" ? true : false : false;
                    return isTravelling && noEncounter;
                }
            },
        };

        public static List<Outcome> GameEventOutcomes = new List<Outcome>() { 
            new Outcome()
            {
                GetDescription = (source, world) => { return "The player will embark upon his adventure!"; } ,
                IsValid = (world) => {
                    bool valid = world.IsCurrentEvent("Init") ? true : false;
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    world.AllEntities.Add("player", new GameAgent() { 
                        // Initialize the player
                        S = new Dictionary<string,string>() {
                            {"Name","player"},
                            {"Location", "tavern"},
                            {"Destination",null},
                            {"Encounter",null},
                            {"CurrentAction","resting"}
                        }
                    });
                    return "the adventurer arrives in town, entering the tavern.";
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "...A possible encounter?";},
                IsValid = (world) => {
                    bool valid = world.IsCurrentEvent("Travel");
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameAgent player =  world.AllEntities.ContainsKey("player") ? world.AllEntities["player"] : null;
                    if (!player.S.ContainsKey("Encounter"))
                    {
                        player.S.Add("Encounter", "true");
                    }
                    else
                    {
                        player.S["Encounter"] = "true";                    }
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.AppendLine("The adventurer has an encounter on the way to his destination!");
                    return sbResult.ToString();
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "...A safe journey?"; },
                IsValid = (world) => {
                    bool isTravelling = world.IsCurrentEvent("Travel");
                    return isTravelling;
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameAgent player =  world.AllEntities.ContainsKey("player") ? world.AllEntities["player"] : null;
                    if (!player.S.ContainsKey("Location"))
                    {
                        player.S.Add("Location", player.S["Destination"]);
                    }
                    else
                    {
                        player.S["Location"] =  player.S["Destination"];
                    }
                    player.S["Destination"] = null;
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.AppendLine("The adventurer has an uneventful trip to his destination!");
                    sbResult.AppendLine("He is now at: " + player.S["Location"]);
                    return sbResult.ToString();
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "Player will travel to the dungeon."; },
                IsValid = (world) => {
                    bool valid = world.IsCurrentEvent("GoDungeon");
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameAgent player =  world.AllEntities.ContainsKey("player") ? world.AllEntities["player"] : null;
                    if (!player.S.ContainsKey("Destination"))
                    {
                        player.S.Add("Destination", "dungeon");
                    }
                    else
                    {
                        player.S["Destination"] = "dungeon";
                    }
                    return "The party embarks on their journey to the dungeon";
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "Player will travel to the tavern."; },
                IsValid = (world) => {
                    bool valid = world.IsCurrentEvent("GoTavern");
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameAgent player =  world.AllEntities.ContainsKey("player") ? world.AllEntities["player"] : null;
                    if (!player.S.ContainsKey("Destination"))
                    {
                        player.S.Add("Destination", "tavern");
                    }
                    else
                    {
                        player.S["Destination"] = "tavern";
                    }
                    return "The adventurer embarks on his journey to the tavern";
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "Player will travel to the temple."; },
                IsValid = (world) => {
                    bool valid = world.IsCurrentEvent("GoTemple");
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameAgent player =  world.AllEntities.ContainsKey("player") ? world.AllEntities["player"] : null;
                    if (!player.S.ContainsKey("Destination"))
                    {
                        player.S.Add("Destination", "temple");
                    }
                    else
                    {
                        player.S["Destination"] = "temple";
                    }
                    return "The adventurer embarks on his journey to the temple";
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "Player will travel to the market."; } ,
                IsValid = (world) => {
                    bool valid = world.IsCurrentEvent("GoMarket");
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameAgent player =  world.AllEntities.ContainsKey("player") ? world.AllEntities["player"] : null;
                    if (!player.S.ContainsKey("Destination"))
                    {
                        player.S.Add("Destination", "market");
                    }
                    else
                    {
                        player.S["Destination"] = "market";
                    }
                    return "The adventurer embarks on his journey to the market";
                }
            },
        };
    }
}
