using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent.Judgements
{
    /*
     *  The judgement of a game agent is based on:
     *      - Who/what they are.  These are prejudices.
     *      - What they've done. Judgements on memories.
     */
    public struct AgentJudgement
    {
        public GameAgent JudgedEntity;

        public string Judgement;

        public string Emotion;

        public string JudgementReason;

        public Occurance JudgedMemory;
    }

}
