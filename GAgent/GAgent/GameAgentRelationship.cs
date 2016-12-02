using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    public class GameAgentRelationship
    {
        public struct GameAgentRelation
        {
            public GameAgent RelationSubject;

            public GameAgent RelationObject;

            public string Relationship;
        }

        public GameAgentRelation Relation1;

        public GameAgentRelation Relation2;

        public GameAgentRelationship(GameAgentRelation r1, GameAgentRelation r2)
        {
            Relation1 = r1;
            Relation2 = r2;
        }
    }

    public class relTester
    {
        public relTester()
        {

        }
    }
}
