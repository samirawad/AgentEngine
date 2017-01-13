using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent.Judgements
{
    public static class DefaultJudgements // we can refactor this as a dictionary and move the judgement methods to gameentity
    {

        public static List<RoleJudgement> Judgements = new List<RoleJudgement>()
        {
            // this might benefit from a selector function, like the one we're using in conditions.
            // Also, the Predicate is basically a condition to be satisfied.
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
