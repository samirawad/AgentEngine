using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAgent;

namespace GAgent.StandardEvents
{
    // This is an example of an event library.
    public static class SampleMemoryEvent
    {
        public static List<GameAction> GameEvents = new List<GameAction>()
        {
            new GameAction()
            {
                ID = "AgressiveEvent00",
                Description = (world) => { return "A sample agression event involving 3 entities."; },
                IsValid = (world) => {
                    //bool noMemoriesExist = !world.AllEntities.Any(e => e.Memories.Count > 0);
                    //return noMemoriesExist;
                    return true;
                }
            },
            new GameAction()
            {
                ID = "FriendEvent00",
                Description = (world) => { return "A sample friendly event involving 2 entities."; },
                IsValid = (world) => {
                    //bool noMemoriesExist = !world.AllEntities.Any(e => e.Memories.Count > 0);
                    //return noMemoriesExist;'
                    return true;
                }
            },
            new GameAction()
            {
                ID = "CollectMemories",
                Description = (world) => { return "Collect the memories of entities"; },
                IsValid = (world) => { 
                    // valid if entities have memories
                    bool AnEntityHasMemories = world.AllEntities.Any(e => e.Value.HasMemories());
                    bool noSelectorEvents = !world.AllGameActions.Any(e => e.ID.Contains("view")); // perhaps game actions and events require tags
                    return AnEntityHasMemories && noSelectorEvents;
                }
            },
        };

        public static List<Outcome> GameEventOutcomes = new List<Outcome>()
        {
            new Outcome()
            {
                GetDescription = (source, world) => { return "Collect memories"; },
                IsValid = (source, world) => {
                    bool valid = source.ID == "CollectMemories" ? true : false;
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    // generate actions for each entity
                    List<GameAgent> actorsToView = world.AllEntities // Checking for npc entities by the existence of a name key.  probably not the best way
                        .Where(e => e.Value.S.ContainsKey("Name"))
                        .Select(e => e.Value)
                        .ToList();
                    foreach(GameAgent currEntity in actorsToView)
                    {
                        string entityID = "view" + currEntity.S["Name"];
                        if (!world.AllGameActions.Any(a => a.ID == entityID))
                        {
                            world.AllGameActions.Add(new GameAction()
                                {
                                     ID = entityID,
                                     Description = (w) => { return "View " + currEntity.S["Name"]; },
                                     IsValid = (w) => { return true; }
                                });
                        }

                        if(!world.AllOutcomes.Any(o => o.ID == entityID)) // we can programmatically add new outcomes during execution.  This is powerful but potentially messy I think
                        {
                            world.AllOutcomes.Add(new Outcome()
                            {
                                 ID = entityID,
                                 GetDescription = (s, w) => { return "view memories of" + currEntity.S["Name"]; },
                                 IsValid = (s, w) =>
                                 {
                                     bool valid = s.ID == entityID ? true : false;
                                     return valid;
                                 },
                                 PerformOutcome = (ref GameWorld w) => 
                                 {
                                     StringBuilder sbResult = new StringBuilder();
                                     sbResult.AppendLine("the memories of " + currEntity.S["Name"] + ": ");
                                     // this foolishnes needs to be collected as a method and moved to GameEntity
                                     sbResult.AppendLine(currEntity.GetOpinions());
                                     return sbResult.ToString();
                                 }
                            });
                        }
                    }
                    return "entities collected";
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "A friendly occurance"; },
                IsValid = (source, world) => {
                    return source.ID == "FriendEvent00" ? true : false;
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameAgent niceagent = world.AllEntities["CuteFuzzy"];
                    GameAgent targetagent = world.AllEntities["InnocentBystander"];

                    Occurance newOccurance = new Occurance()
                    {
                        Description = 
                        niceagent.S["Name"] + " is nice to " + targetagent.S["Name"] + "!  How friendly.",

                        OccuranceRoles = new Dictionary<string, HashSet<GameAgent>>()
                        {
                            {"Friendly", new HashSet<GameAgent>() { niceagent }},
                            {"Friendtarget", new HashSet<GameAgent>() { targetagent }},
                        }
                    };

                    niceagent.AddMemory(newOccurance);
                    targetagent.AddMemory(newOccurance);

                    return newOccurance.Description;
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "An aggresive occurence observed by multiple parties."; },
                IsValid = (source, world) => {
                    bool valid = source.ID == "AgressiveEvent00" ? true : false;
                    return valid;
                },
                PerformOutcome = (ref GameWorld world) => {
                    // Select the participants of the outcome
                    GameAgent agressor = world.AllEntities["BigBully"];
                    GameAgent defender = world.AllEntities["CuteFuzzy"];
                    GameAgent witness = world.AllEntities["InnocentBystander"];

                    // Generate the outcome and assign to all entities as a memory
                    Occurance newOccurnace = new Occurance()
                    {
                        Description =
                        agressor.S["Name"] +
                        " is mean to " + defender.S["Name"] +
                        " while " + witness.S["Name"] + " observes.",

                        OccuranceRoles = new Dictionary<string, HashSet<GameAgent>>()
                        {
                            {"Agressor", new HashSet<GameAgent>() { agressor }},
                            {"Victim", new HashSet<GameAgent>() { defender }}
                        }
                    };
                    agressor.AddMemory(newOccurnace);
                    defender.AddMemory(newOccurnace);
                    witness.AddMemory(newOccurnace);

                    // Perform any changes to gamestate.
                    // TODO: gamestate changes here.  not required for this sample occurance
                    return newOccurnace.Description;
                }
            }
        };

    }

}
