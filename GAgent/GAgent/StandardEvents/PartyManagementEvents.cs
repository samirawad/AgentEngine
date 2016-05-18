using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent.StandardEvents
{
    public static class PartyManagementEvents
    {
        public static List<GameAction> GameEvents = new List<GameAction>() 
        { 
            new GameAction()
            {
                ID = "Recruit",
                ShowOutcomes = true,
                Description = (world) => { return "Recruit a new adventurer"; },
                IsValid = (world) => {
                    // Valid if the player is at the tavern
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
                    bool atTavern = player != null ?
                        player.S["Location"] == "tavern" ? true : false
                        : false;
                    // The player is not travelling
                    bool notTravelling = player != null ?
                        player.S["Destination"] == null ? true : false
                        : false;
                    return atTavern && notTravelling;
                }
            },
            new GameAction()
            {
                ID = "ViewParty",
                ShowOutcomes = false,
                Description = (world) => { return "Examine the party"; },
                IsValid = (world) => {
                    // We can view the party when party members exist, and we aren't currently looking at the party
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
                    bool notExaminingParty = player != null ?
                        player.S["CurrentAction"] == null ? true : false
                        : false;
                    bool partyMembersExist = player != null ?
                        world.AllEntities.Any(a => a.T.ContainsKey("Conditions")) ? true : false // need a better way to check for existence of party memebers
                        : false;
                    return notExaminingParty;
                }
            }
        };

        public static List<Outcome> GameEventOutcomes = new List<Outcome>()
        {
            new Outcome()
            {
                GetDescription = (source, entities) => { return "View the party"; },
                IsValid = (source, entities) => {
                    return source.ID == "ViewParty";
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameEntity player = world.AllEntities.FirstOrDefault(e => e.S["Name"] == "player");
                    StringBuilder sbOut = new StringBuilder();
                    sbOut.AppendLine("Examining the party");
                    player.S["CurrentAction"] = "viewparty";

                    // Create the view party events for each party memeber if they don't exist
                    foreach(var currPartyMember in world.AllEntities.Where(p => p.T["Conditions"].Contains("InParty")))
                    {
                        // Check to see if an event already exists for viewing this agent.
                        // If not, create it and the attending outcome.
                        if(!world.AllGameActions.Any(a => a.ID == "view_"+currPartyMember.N["Name"]))
                        {
                            string viewID = "view_" + currPartyMember.N["Name"];
                            world.AllGameActions.Add(new GameAction()
                            {
                                ID = viewID,
                                Description = (w) => { return "view " + currPartyMember.N["Name"]; },
                                IsValid = (w) =>
                                {
                                    return player.S["CurrentAction"] == "viewparty" ? true : false;
                                },
                            });

                           world.AllOutcomes.Add(new Outcome()
                            {
                                 ID = viewID,
                                 GetDescription = (source, entities) => { return "view " + currPartyMember.S["Name"]; },
                                 IsValid = (source, entities) =>
                                 {
                                     bool valid = source.ID == viewID ? true : false;
                                     return valid;
                                 },
                                 PerformOutcome = (ref GameWorld w) => 
                                 {
                                     StringBuilder sbResult = new StringBuilder();
                                     sbOut.AppendLine("Name: " + currPartyMember.S["Name"]);
                                     sbOut.AppendLine("Gender: " + currPartyMember.S["Gender"]);
                                     sbOut.AppendLine("Class: " + currPartyMember.S["Class"]);
                                     sbOut.AppendLine("Personality: " + string.Join(", ", currPartyMember.T["Personality"].ToArray()));
                                     player.S["CurrentAction"] = null;
                                     return sbResult.ToString();
                                 }
                            });
                        };
                    };
                    return sbOut.ToString();
                }
            },
            new Outcome()
            {
                GetDescription = (source, entities) => { return "A new adventurer is recruited!"; },
                IsValid = (source, entities) => {
                    return source.ID == "Recruit";
                },
                PerformOutcome = (ref GameWorld world) => {
                    
                    GameEntity newEntity = EntityLibrary.DefaultEntities.GenerateEntity();
                    StringBuilder sbOut = new StringBuilder();
                    sbOut.AppendLine("Name: " + newEntity.S["Name"]);
                    sbOut.AppendLine("Gender: " + newEntity.S["Gender"]);
                    sbOut.AppendLine("Class: " + newEntity.S["Class"]);
                    sbOut.AppendLine("Personality: " + string.Join(", ", newEntity.T["Personality"].ToArray()));

                    // Add the new entity to the party
                    newEntity.T["Conditions"].Add("InParty");
                    world.AllEntities.Add(newEntity);
                    return sbOut.ToString();
                }
            },
        };
    }
}
