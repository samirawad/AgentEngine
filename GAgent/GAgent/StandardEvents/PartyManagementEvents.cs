﻿using System;
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
                ID = "Interview",
                ShowOutcomes = true,
                Description = (world) => { return "Recruit a new adventurer"; },
                IsValid = (world) => {
                    // Valid if the player is resting at the tavern
                    GameAgent player =  world.AllEntities.ContainsKey("player") ? world.AllEntities["player"] : null;
                    bool notExaminingParty = player != null ?
                            player.S["CurrentAction"] == "resting" ? true : false
                            : false;
                    bool atTavern = player != null ?
                        player.S["Location"] == "tavern" ? true : false
                        : false;
                    // The player is not travelling
                    bool notTravelling = player != null ?
                        player.S["Destination"] == null ? true : false
                        : false;
                    // Not currently interviewing
                    bool Not_interviewing = !world.AllRelations.Any(r => r.Relationship == "interviewing");
                    
                    /*
                     * This method of determining if an action is valid is problematic. If we only want one option to be available,
                     * we have to explicitly state that the others are invalid.  This doesn't seem ok.
                     */
                    return notExaminingParty && atTavern && notTravelling && Not_interviewing;
                }
            },
            new GameAction()
            {
                ID = "HireDecision",
                ShowOutcomes = false,
                Description = (world) => {
                    GameAgent candidate = 
                        world.AllRelations.FirstOrDefault(c =>
                        c.Relationship == "interviewing").RelationObject;
                    return "Hire " + candidate.S["Name"]; 
                },
                IsValid = (world) => {
                    return world.AllRelations.Any(r => r.Relationship == "interviewing");
                }
            },
            new GameAction()
            {
                ID = "RejectDecision",
                ShowOutcomes = false,
                Description = (world) => {
                    GameAgent candidate = 
                        world.AllRelations.FirstOrDefault(c =>
                        c.Relationship == "interviewing").RelationObject;
                    return "Reject " + candidate.S["Name"]; 
                },
                IsValid = (world) => {
                    return world.AllRelations.Any(r => r.Relationship == "interviewing");
                }
            },
            new GameAction()
            {
                ID = "ViewParty",
                ShowOutcomes = true,
                Description = (world) => { return "Examine the party"; },
                IsValid = (world) => {
                    // We can view the party when party members exist, and we aren't currently looking at the party
                    GameAgent player =  world.AllEntities.ContainsKey("player") ? world.AllEntities["player"] : null;
                    
                    // We're not currently examining the party
                    bool notExaminingParty = player != null ?
                        player.S["CurrentAction"] == "resting" ? true : false
                        : false;
                    
                    // There are actually party memebers to view
                    bool partyMembersExist = player != null ? 
                        world.AllEntities.Any(
                        a => a.Value.Tags != null ? a.Value.Tags.Contains("partymember") : false
                        ) ? true : false 
                        : false;
                    return notExaminingParty && partyMembersExist;
                }
            }
        };

        public static List<Outcome> GameEventOutcomes = new List<Outcome>()
        {
            new Outcome()
            {
                GetDescription = (source, world) => { return "View the party"; },
                IsValid = (source, world) => {
                    return source.ID == "ViewParty";
                },
                PerformOutcome = (ref GameWorld world) => {
                    GameAgent player =  world.AllEntities.ContainsKey("player") ? world.AllEntities["player"] : null;
                    StringBuilder sbOut = new StringBuilder();
                    sbOut.AppendLine("Examining the party");
                    player.S["CurrentAction"] = "viewparty";

                    // Create the view party events for each party memeber if they don't exist
                    List<GameAgent> partyMembers = world.AllEntities.Where(p => 
                        p.Value.T.ContainsKey("Conditions") ?
                        p.Value.T["Conditions"].Contains("InParty") : false)
                        .Select(p => p.Value).ToList();

                    foreach(var currPartyMember in partyMembers)
                    {
                        // Check to see if an event already exists for viewing this agent.
                        // If not, create it and the attending outcome.
                        if(!world.AllGameActions.Any(a => a.ID == "view_"+currPartyMember.S["Name"]))
                        {
                            string viewID = "view_" + currPartyMember.S["Name"];
                            world.AllGameActions.Add(new GameAction()
                            {
                                ID = viewID,
                                ShowOutcomes = true,
                                Description = (w) => { return "View " + currPartyMember.S["Name"] + ", " + currPartyMember.S["Gender"] + " " +currPartyMember.S["Class"]; },
                                IsValid = (w) =>
                                {
                                    return player.S["CurrentAction"] == "viewparty" ? true : false;
                                },
                            });

                           world.AllOutcomes.Add(new Outcome()
                            {
                                 ID = viewID,
                                 GetDescription = (s, w) => { return "view " + currPartyMember.S["Name"]; },
                                 IsValid = (s, w) =>
                                 {
                                     bool valid = s.ID == viewID ? true : false;
                                     return valid;
                                 },
                                 PerformOutcome = (ref GameWorld w) => 
                                 {
                                     StringBuilder sbDescription = new StringBuilder();
                                     sbDescription.AppendLine("Name: " + currPartyMember.S["Name"]);
                                     sbDescription.AppendLine("Gender: " + currPartyMember.S["Gender"]);
                                     sbDescription.AppendLine("Class: " + currPartyMember.S["Class"]);
                                     sbDescription.AppendLine("Personality: " + string.Join(", ", currPartyMember.T["Personality"].ToArray()));
                                     player.S["CurrentAction"] = "resting";
                                     return sbDescription.ToString();
                                 }
                            });
                        };
                    };
                    return sbOut.ToString();
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "A new adventurer is interviewed!"; },
                IsValid = (source, world) => {
                    return source.ID == "Interview";
                },
                PerformOutcome = (ref GameWorld world) => {

                    GameAgent player = world.GetAgentByID("Player");
                    GameAgent newEntity = EntityLibrary.DefaultEntities.GenerateEntity();
                    StringBuilder sbOut = new StringBuilder();
                    sbOut.AppendLine("Name: " + newEntity.S["Name"]);
                    sbOut.AppendLine("Gender: " + newEntity.S["Gender"]);
                    sbOut.AppendLine("Class: " + newEntity.S["Class"]);
                    sbOut.AppendLine("Personality: " + string.Join(", ", newEntity.T["Personality"].ToArray()));

                    world.AllRelations.Add(new GameEntityRelation()
                    {
                        RelationSubject = player, 
                        Relationship = "interviewing",
                        RelationObject = newEntity
                    });
                    
                    world.AllEntities.Add(newEntity.S["Name"], newEntity);
                    return sbOut.ToString();
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "A new adventurer is recruited!"; },
                IsValid = (source, world) => {
                    return source.ID == "HireDecision";
                },
                PerformOutcome = (ref GameWorld world) => {
                    // retrieve the agent from the relation
                    StringBuilder sbOut = new StringBuilder();
                    GameAgent player = world.GetAgentByID("Player");
                    GameAgent candidate = world.AllRelations.FirstOrDefault(c =>
                        c.RelationSubject == player &&
                        c.Relationship == "interviewing").RelationObject;
                    candidate.T["Conditions"].Add("InParty");
                    candidate.Tags = new HashSet<string>() { "partymember" };
                    // Remove the candidate from relations
                    GameEntityRelation candidateRelation = world.AllRelations.FirstOrDefault(c =>    
                        c.RelationSubject == player &&    
                        c.Relationship == "interviewing");
                    world.AllRelations.Remove(candidateRelation);
                    sbOut.AppendLine(candidate.S["Name"] + " joins the party!  Woot! Raise the roooooof.");
                    return sbOut.ToString();
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "rejected!"; },
                IsValid = (source, world) => {
                    return source.ID == "RejectDecision";
                },
                PerformOutcome = (ref GameWorld world) => {
                    // remove the agent from world and remove it's interview relation
                    StringBuilder sbOut = new StringBuilder();
                    GameAgent player = world.GetAgentByID("Player");
                    GameEntityRelation candidateRelation = world.AllRelations.FirstOrDefault(c =>
                        c.RelationSubject == player &&
                        c.Relationship == "interviewing");
                    GameAgent candidate = candidateRelation.RelationObject;
                    world.AllEntities.Remove(candidate.S["Name"]);
                    world.AllRelations.Remove(candidateRelation);
                    sbOut.AppendLine("lol u suk");
                    return sbOut.ToString();
                }
            }
        };
    }
}
