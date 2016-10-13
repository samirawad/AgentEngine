using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent.Judgements
{
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
}
