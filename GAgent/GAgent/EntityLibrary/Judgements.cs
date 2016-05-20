using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    public struct AgentJudgement
    {
        public GameAgent JudgedEntity;

        public string Judgement;

        public string Emotion;

        public string JudgementReason;

        public Occurance JudgedMemory;
    }

    public class Occurance
    {
        public string Description;  // This describes the action of the occurance.  It will be filled out by the Event outcome

        public Dictionary<string, HashSet<GameAgent>> OccuranceRoles;  // The roles that each entity fulfilled during the occurance
    }

    // This function will return true if the role specified in the occurence satisfied the conditions supplied in the function
    public delegate bool RolePredicateDelegate(GameAgent examiner, Occurance occurance);

    // how an entity feels about a particular role in an event.  Since an event will have multiple roles, the opinion the entity
    // has of an event will be nuanced, based on their opinion of the different roles which participate in the event.
    public class RoleJudgement
    {
        public string Role;

        public RolePredicateDelegate RolePredicate; // If the role predicate is satisfied, the Judgement on this role is returned.

        public string Description; // This provides a readable description of why the judgement is passed on this role.

        public string Judgement;

        public string Emotion;  // In addition to judging the role of an event, the event should make the agent feel an emotion.
    }

    public static class JudgementLibrary // we can refactor this as a dictionary and move the judgement methods to gameentity
    {

        // We should put the Judgements into named collections, so we can assign them by name
        // and also select them randomly.
        public static List<RoleJudgement> MoreJudgements = new List<RoleJudgement>()
        {

        };
        
        public static List<RoleJudgement> DefaultJudgements = new List<RoleJudgement>()
        {
            new RoleJudgement()
            {
                Role = "Friendly",
                Description = "Feels affection for those who are friendly to them.",
                RolePredicate = (judge, occurence) =>
                    {
                        GameAgent friendlyagent = occurence.OccuranceRoles["Friendly"].FirstOrDefault();
                        GameAgent friendtarget = occurence.OccuranceRoles["Friendtarget"].FirstOrDefault();

                        bool IamTarget = friendtarget == judge;
                        bool FriendlyIsNotMe = friendlyagent != judge;
                        return IamTarget && FriendlyIsNotMe;
                    },
                Judgement = "Affection",
                Emotion = "Happy"
            },
            new RoleJudgement()
            {
                Role = "Agressor",
                Description = "Dislikes agression whenever the aggressor isn't himself",
                RolePredicate = (judge, occurence) =>
                    {
                        GameAgent agressor = occurence.OccuranceRoles["Agressor"].FirstOrDefault();
                        return agressor != judge;
                    },
                Judgement = "Dislikes",
                Emotion = "Disgust"
            },
            new RoleJudgement()
            {
                Role = "Agressor",
                Description = "Feels powerful whenever he is the agressor",
                RolePredicate = (judge, occurence) =>
                    {
                        GameAgent agressor = occurence.OccuranceRoles["Agressor"].FirstOrDefault();
                        return agressor == judge;
                    },
                Emotion = "Powerful"
            },
            new RoleJudgement()
            {
                Role = "Agressor",
                Description = "Feels afraid of the agressor when he is the victim.",
                RolePredicate = (judge, occurence) =>
                    {
                        GameAgent victim = occurence.OccuranceRoles["Victim"].FirstOrDefault();
                        return victim == judge;
                    },
                Judgement = "Afraid",
                Emotion = "Scared"
            },
            new RoleJudgement()
            {
                Role = "Agressor",
                Description = "Feels rage towards the agressor when the victim is his friend",
                RolePredicate = (judge, occurence) =>
                    {
                        GameAgent victim = occurence.OccuranceRoles["Victim"].FirstOrDefault();
                        bool IamNotTheVictim = victim != judge;
                        bool ICareAboutVictim = judge.HasJudgmentOfAgent("Affection",victim);
                        return IamNotTheVictim && ICareAboutVictim;
                    },
                Judgement = "Hatred",
                Emotion = "Rage"
            },
            new RoleJudgement()
            {
                Role = "Victim",
                Description = "Feels pity for the victim when not the agressor",
                RolePredicate = (judge, occurence) =>
                    {
                        GameAgent agressor = occurence.OccuranceRoles["Agressor"].FirstOrDefault();
                        return agressor != judge;
                    },
                Judgement = "Pity"
            },
            new RoleJudgement()
            {
                Role = "Victim",
                Description = "Feels disgust for the victim if he is the agressor",
                RolePredicate = (judge, occurence) =>
                    {
                        GameAgent agressor = occurence.OccuranceRoles["Agressor"].FirstOrDefault();
                        return agressor == judge;
                    },
                Judgement = "Disgust",
                Emotion = "Domineering"
            },
            new RoleJudgement()
            {
                Role = "Victim",
                Description = "Feels rage when the victim is his friend",
                RolePredicate = (judge, occurence) =>
                    {
                        GameAgent victim = occurence.OccuranceRoles["Victim"].FirstOrDefault();
                        bool IamNotTheVictim = victim != judge;
                        bool ICareAboutVictim = judge.HasJudgmentOfAgent("Affection", victim);
                        return IamNotTheVictim && ICareAboutVictim;
                    },
                Judgement = "Concern",
                Emotion = "Rage"
            }
        };
    }

}
