using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    public class GameAgent
    {
        public Dictionary<string, long> N = new Dictionary<string,long>();  // numeric values

        public Dictionary<string, string> S = new Dictionary<string,string>() ; // string values

        public Dictionary<string, HashSet<string>> T = new Dictionary<string,HashSet<string>>(); // tagsets

        public HashSet<string> Tags = new HashSet<string>();

        public List<RoleJudgement> Morals = new List<RoleJudgement>();
        
        private List<Occurance> Memories = new List<Occurance>();

        public List<AgentJudgement> Judgements = new List<AgentJudgement>();

        public List<AgentJudgement> AddMemory(Occurance newMemory)
        {
            List<AgentJudgement> result = JudgeMemory(newMemory);
            Memories.Add(newMemory);
            return result;
        }

        public bool HasMemories()
        {
            return Memories.Count > 0;
        }

        // Search the memories of this agent to determine if this judgement exists
        public bool HasJudgmentOfAgent(string judgement, GameAgent agent)
        {
            bool result = Judgements.Any(j => j.JudgedEntity == agent && j.Judgement == judgement);
            return result;
        }

        public List<AgentJudgement> JudgeMemory(Occurance memoryToJudge)
        {
            List<AgentJudgement> result = new List<AgentJudgement>();
            foreach(string currRole in memoryToJudge.OccuranceRoles.Keys)
            {
                List<AgentJudgement> newJudgements = JudgeRole(memoryToJudge, currRole);
                result.AddRange(newJudgements);
                Judgements.AddRange(newJudgements);
            }
            return result;
        }

        public List<AgentJudgement> JudgeRole(Occurance memoryToJudge, string roleToJudge)
        {
            List<AgentJudgement> AgentJudgements = new List<AgentJudgement>();
            // For every moral that this agent posesses, jude the role in the memory
            foreach(RoleJudgement currJudgement in Morals.Where(m => m.Role == roleToJudge))
            {
                if(currJudgement.RolePredicate(this, memoryToJudge))
                {
                    foreach (GameAgent currEntity in memoryToJudge.OccuranceRoles[roleToJudge])
                    {
                        AgentJudgements.Add(new AgentJudgement()
                        {
                            JudgedEntity = currEntity,
                            Judgement = currJudgement.Judgement,
                            JudgementReason = currJudgement.Description,
                            Emotion = currJudgement.Emotion,
                            JudgedMemory = memoryToJudge
                        });
                    }
                }
            }
            return AgentJudgements;
        }

        public string GetOpinions()
        {
            StringBuilder sbResult = new StringBuilder();
            foreach (AgentJudgement currAgentJudgement in Judgements)
            {
                sbResult.AppendLine("------------------------------------------------------");
                sbResult.AppendLine("Memory: " + currAgentJudgement.JudgedMemory.Description);
                sbResult.AppendLine("Emotion: " + currAgentJudgement.Emotion);
                sbResult.AppendLine("Feels towards " + currAgentJudgement.JudgedEntity.S["Name"] + ": " + currAgentJudgement.Judgement);
                sbResult.AppendLine("Reason: " + currAgentJudgement.JudgementReason);
            }
            
            return sbResult.ToString();
        }
    }
}
