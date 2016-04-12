using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent.StandardEvents
{
    public static class TravelingLib
    {
        public static List<GameAction> GameEvents = new List<GameAction>() { 
            new GameAction()
            {
                ID = "Init",
                ShowOutcomes = true,
                Description = "Begin the adventure!",
                IsValid = (world) => { 
                    // Initialization action. Valid if the player hasn't yet been created.
                    return !world.AllEntities.Any(e => e.S["Name"] == "player");
                }
            },
            new GameAction()
            {
                ID = "GoTavern",
                ShowOutcomes = true,
                Description = "Go to the Tavern",
                IsValid = (world) => { 
                    // Vaild if player exists, not at the current location, and not currently travelling
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
                    bool notAtCurrentLocation = player != null ?
                        player.S["Location"] != "tavern" ? true : false
                        : false;
                    bool notTravelling = player != null ?
                        player.S["Destination"] == null ? true : false
                        : false;
                    return notAtCurrentLocation && notTravelling;
                }
            },
            new GameAction()
            {
                ID = "Recruit",
                ShowOutcomes = true,
                Description = "Recruit a new adventurer",
                IsValid = (world) => {
                    // Valid if the player is at the tavern
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
                    bool atTavern = player != null ?
                        player.S["Location"] == "tavern" ? true : false
                        : false;
                    bool notTravelling = player != null ?
                        player.S["Destination"] == null ? true : false
                        : false;
                    return atTavern && notTravelling;
                }
            },
            new GameAction()
            {
                ID = "GoMarket",
                ShowOutcomes = true,
                Description = "Go to the Market",
                IsValid = (world) => { 
                    // Vaild if player exists, not at the current location, and not currently travelling
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
                    bool notAtCurrentLocation = player != null ?
                        player.S["Location"] != "market" ? true : false
                        : false;
                    bool notTravelling = player != null ?
                        player.S["Destination"] == null ? true : false
                        : false;
                    return notAtCurrentLocation && notTravelling;
                }
            },
            new GameAction()
            {
                ID = "GoTemple",
                ShowOutcomes = true,
                Description = "Go to the Temple",
                IsValid = (world) => { 
                    // Vaild if player exists, not at the current location, and not currently travelling
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
                    bool notAtCurrentLocation = player != null ?
                        player.S["Location"] != "temple" ? true : false
                        : false;
                    bool notTravelling = player != null ?
                        player.S["Destination"] == null ? true : false
                        : false;
                    return notAtCurrentLocation && notTravelling;
                }
            },
            new GameAction()
            {
                ID = "Travel",
                ShowOutcomes = false,
                Detail = "an encounter might occur...",
                Description = "The adventurer travels towards his destination...",
                IsValid = (world) => { 
                    // Is valid if the player has a current destination
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
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
                Description = "The player will embark upon his adventure!",
                IsValid = (source, parent, entities) => {
                    bool valid = source.ID == "Init" ? true : false;
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    world.AllEntities.Add(new GameEntity() { 
                        // Initialize the player
                        S = new Dictionary<string,string>() {
                            {"Name","player"},
                            {"Location", "tavern"},
                            {"Destination",null},
                            {"Encounter",null}
                        }
                    });
                    return "the adventurer arrives in town, entering the tavern.";
                }
            },
            new Outcome()
            {
                Description = "...A possible encounter?",
                IsValid = (source, parent,entities) => {
                    bool valid = source.ID == "Travel" ? true : false;
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
                    if (!player.S.ContainsKey("Encounter"))
                    {
                        player.S.Add("Encounter", "true");
                    }
                    else
                    {
                        player.S["Encounter"] = "true";
                    }
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.AppendLine("The adventurer has an encounter on the way to his destination!");
                    return sbResult.ToString();
                }
            },
            new Outcome()
            {
                Description = "...A safe journey?",
                IsValid = (source, parent,entities) => {
                    bool valid = source.ID == "Travel" ? true : false;
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
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.AppendLine("The adventurer has an uneventful trip to his destination!");
                    sbResult.AppendLine("He is now at: " + player.S["Location"]);
                    return sbResult.ToString();
                }
            },
            new Outcome()
            {
                Description = "Player will travel to the tavern.",
                IsValid = (source, parent, entities) => {
                    bool valid = source.ID == "GoTavern" ? true : false;
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
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
                Description = "A new adventurer is recruited!",
                IsValid = (source, parent, entities) => {
                    return source.ID == "Recruit";
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameEntity newEntity = EntityLibrary.DefaultEntities.GenerateEntity();
                    StringBuilder sbOut = new StringBuilder();
                    sbOut.AppendLine("Gender: " + newEntity.S["Gender"]);
                    sbOut.AppendLine("Class: " + newEntity.S["Class"]);
                    sbOut.AppendLine("Personality: " + string.Join(", ", newEntity.T["Personality"].ToArray()));
                    return sbOut.ToString();
                }

            },
            new Outcome()
            {
                Description = "Player will travel to the temple.",
                IsValid = (source, parent, entities) => {
                    bool valid = source.ID == "GoTemple" ? true : false;
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
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
                Description = "Player will travel to the market.",
                IsValid = (source, parent,entities) => {
                    bool valid = source.ID == "GoMarket" ? true : false;
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
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
