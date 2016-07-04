﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent.StandardEvents
{
    public static class TravelingEvents
    {
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
                ID = "GoTavern",
                ShowOutcomes = true,
                Description = (world) => { return "Go to the Tavern"; },
                IsValidDel = (world) => { 
                    // Vaild if player exists, not at the current location, and not currently travelling
                    GameAgent player =  world.AllEntities.ContainsKey("player") ? world.AllEntities["player"] : null;
                    if (player == null) return false;
                    bool atRest = player.S["CurrentAction"] == "resting" ? true : false;
                    bool notAtCurrentLocation = player != null ?
                        player.S["Location"] != "tavern" ? true : false
                        : false;
                    bool notTravelling = player != null ?
                        player.S["Destination"] == null ? true : false
                        : false;
                    return atRest && notAtCurrentLocation && notTravelling;
                }
            },

           new GameAction()
            {
                ID = "GoMarket",
                ShowOutcomes = true,
                Description = (world) => { return "Go to the Market"; },
                IsValidDel = (world) => { 
                    // Vaild if player exists, not at the current location, and not currently travelling
                    GameAgent player =  world.AllEntities.ContainsKey("player") ? world.AllEntities["player"] : null;
                    if (player == null) return false;
                    bool atRest = player.S["CurrentAction"] == "resting" ? true : false;
                    bool notAtCurrentLocation = player != null ?
                        player.S["Location"] != "market" ? true : false
                        : false;
                    bool notTravelling = player != null ?
                        player.S["Destination"] == null ? true : false
                        : false;
                    return atRest && notAtCurrentLocation && notTravelling;
                }
            },
            new GameAction()
            {
                ID = "GoTemple",
                ShowOutcomes = true,
                Description = (world) => { return "Go to the Temple"; },
                IsValidDel = (world) => { 
                    // Vaild if player exists, not at the current location, and not currently travelling
                    GameAgent player =  world.AllEntities.ContainsKey("player") ? world.AllEntities["player"] : null;
                    if (player == null) return false;
                    bool atRest = player.S["CurrentAction"] == "resting" ? true : false;
                    bool notAtCurrentLocation = player != null ?
                        player.S["Location"] != "temple" ? true : false
                        : false;
                    bool notTravelling = player != null ?
                        player.S["Destination"] == null ? true : false
                        : false;
                    return atRest && notAtCurrentLocation && notTravelling;
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
                IsValid = (source, world) => {
                    bool valid = source.ID == "Init" ? true : false;
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
                IsValid = (source, world) => {
                    bool valid = source.ID == "Travel" ? true : false;
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
                IsValid = (source, world) => {
                    bool isTravelling = source.ID == "Travel" ? true : false;
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
                GetDescription = (source, world) => { return "Player will travel to the tavern."; },
                IsValid = (source, world) => {
                    bool valid = source.ID == "GoTavern" ? true : false;
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
                IsValid = (source, world) => {
                    bool valid = source.ID == "GoTemple" ? true : false;
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
                IsValid = (source, world) => {
                    bool valid = source.ID == "GoMarket" ? true : false;
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
