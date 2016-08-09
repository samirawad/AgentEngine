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
            return atRest && notAtCurrentLocation && notTravelling;
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
        };

        public static List<Outcome> GameEventOutcomes = new List<Outcome>() { 
            new Outcome("StartAdventure",
                (world) => { return "The player will embark upon his adventure!"; } ,
                (world) => {
                    bool doingInit = world.IsCurrentAction("Init") ? true : false;
                    return doingInit;
                },
                (ref GameWorld world) => {
                    world.AllEntities.Add("player", new GameAgent() { 
                        // Initialize the player
                        S = new Dictionary<string,string>() {
                            {"Name","player"},
                            {"Location", "tavern"},
                            {"Destination",null},
                            {"CurrentAction","resting"}
                        }
                    });
                    return "the adventurer arrives in town, entering the tavern.";
                }),
                new Outcome("MonsterEncounter", new HashSet<string> { "TravelEncounter" },
                (world) => { return "Monster encounter"; },
                (world) => {
                    bool stillTravelling = world.CurrentOutcome != null ? world.CurrentOutcome.HasTag("TravelEncounter") : false;
                    bool startTravelling = world.IsCurrentAction("GoDungeon");
                    bool hasNotArrivedYet = !world.IsCurrentOutcome("DungeonArrival");
                    return (startTravelling || stillTravelling) && hasNotArrivedYet;
                },
                (ref GameWorld world) => {
                    return "The party encounters a monster on the way to the dungeon";
                }),
                new Outcome("NPCEncounter",new HashSet<string> { "TravelEncounter" },
                (world) => { return "NPC encounter."; },
                (world) => {
                    bool stillTravelling = world.CurrentOutcome != null ? world.CurrentOutcome.HasTag("TravelEncounter") : false;
                    bool startTravelling = world.IsCurrentAction("GoDungeon");
                    bool hasNotArrivedYet = !world.IsCurrentOutcome("DungeonArrival");
                    return (startTravelling || stillTravelling) && hasNotArrivedYet;
                },
                (ref GameWorld world) => {
                    return "The party encounters an NPC on the way to the dungeon";
                }),
                new Outcome("VistaEncounter",new HashSet<string> { "TravelEncounter" },
                (world) => { return "Vista encounter."; },
                (world) => {
                    bool stillTravelling = world.CurrentOutcome != null ? world.CurrentOutcome.HasTag("TravelEncounter") : false;
                    bool startTravelling = world.IsCurrentAction("GoDungeon");
                    bool hasNotArrivedYet = !world.IsCurrentOutcome("DungeonArrival");
                    return (startTravelling || stillTravelling) && hasNotArrivedYet;
                },
                (ref GameWorld world) => {
                    return "The party encounters a beautiful vista on the way to the dungeon";
                }),
                new Outcome("DungeonArrival",
                (world) => { return "Dungeon arrival"; },
                (world) => {
                    // the last outcome was a travel event.
                    bool stillTravelling = world.CurrentOutcome != null ? world.CurrentOutcome.HasTag("TravelEncounter") : false;
                    return stillTravelling;
                },
                (ref GameWorld world) => {
                    GameAgent player = world.AllEntities["player"];
                    player.S["Location"] = "dungeon";
                    player.S["Destination"] = null;
                    return "The party arrives at the dungeon";
                })
        };
    }
}
