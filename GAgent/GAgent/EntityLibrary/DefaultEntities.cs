using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GAgent.EntityLibrary
{

    public static class DefaultEntities
    {
        private static Random rnd = new Random();

        public static List<string> CharacterTypes = new List<string>() // Abbreviates their proficiences and skills
        {
            "Barbarian", "Paladin", "Mercenary", 
            "Rogue", "Bard", "Ranger", "Monk",
            "Cleric", "Mage", "Necromancer", 
        };

        public static List<string> GenderTypes = new List<string>()
        {
            "Male","Female"
        };

       /*
        *   It might be interesting to not show all personality tags right away, perhaps only the obvious outward ones.
        *   That way we could discover character flaws through gameplay instead.
        */
        public static List<List<string>> PersonalityTags = new List<List<string>>()
        {
            // Extraversion
            new List<string>() {"warm","aloof"},   
            new List<string>() {"gregarious","shy"},
            new List<string>() {"assertive","unassertive"},
            new List<string>() {"energetic","leisurely"},
            new List<string>() {"adventurous","cautious"},
            new List<string>() {"cheerful","grim"},
            // Agreeableness
            new List<string>() {"trusting","distrustful"},
            new List<string>() {"sincere","deceptive"},
            new List<string>() {"altruistic","selfish"},   
            new List<string>() {"cooperative","obstinate"},
            new List<string>() {"modest","boastful"},
            new List<string>() {"sympathetic","indifferent"},
            // Conscientiousness
            new List<string>() {"confident","hesitant"},
            new List<string>() {"orderly","disorganized"},
            new List<string>() {"responsible","irresponsible"},   
            new List<string>() {"ambitious","unambitious"},
            new List<string>() {"disciplined","undisciplined"},
            new List<string>() {"brave","cowardly"},
            // Neuroticism
            new List<string>() {"optimistic","pessimistic"}, // opinion about the future
            new List<string>() {"patient","short-tempered"},
            new List<string>() {"lighthearted","melancholy"},   
            new List<string>() {"secure","insecure"},
            new List<string>() {"strong-willed","weak-willed"},
            new List<string>() {"composed","overwrought"}, // ability to handle present stress
            // Openness
            new List<string>() {"imaginative","unimaginative"},
            new List<string>() {"cultured","uncultured"}, // appreciative of art
            new List<string>() {"reflective","unreflective"},   
            new List<string>() {"curious","incurious"},
            new List<string>() {"philosophical","realist"},

            // Political
            new List<string>() {"accepting","bigoted"}, // of other races
            new List<string>() {"untraditional","traditional"}, // of their origin culture
            new List<string>() {"irreverant","religious"}, // of religion
            new List<string>() {"unpatriotic","patriotic"}, // of the establisment 
            new List<string>() {"honorable","unscrupulous"}, // personal code

            // Physical Traits 
            new List<string>() {"beautiful","ugly"},
            new List<string>() {"tall","short"},
            new List<string>() {"strong","weak"},   
            new List<string>() {"brilliant","dim"},
            new List<string>() {"wise","foolish"},
            new List<string>() {"graceful","clumsy"},
            new List<string>() {"hardy","frail"},
            new List<string>() {"thin","fat"},
            new List<string>() {"skilled","unskilled"},
        };

        // A generalized entity generator which assembles entities from random properties and tags
        // could be used for generating everything from randomized monsters to randomized agents with personalities
        public static GameAgent GenerateEntity()
        {
            GameAgent newEntity = new GameAgent();
            newEntity.T = new Dictionary<string, HashSet<string>>();
            newEntity.T.Add("Personality", new HashSet<string>());
            // Ensure that the newly generated character has at least 3 traits.
            while(newEntity.T["Personality"].Count < 3)
            {
                foreach (List<string> currTags in PersonalityTags)
                {
                    int roll = rnd.Next(100);
                    if (roll > 90)
                    {
                        string newTrait = currTags.ToArray()[rnd.Next(currTags.Count)];
                        if (!newEntity.T["Personality"].Contains(newTrait))
                        {
                            if (roll > 97) newTrait = "profoundly " + newTrait;
                            newEntity.T["Personality"].Add(newTrait);
                        }
                    }
                }
            }
            
            // Generate name based on gender
            string gender =  GenderTypes.ToArray()[rnd.Next(GenderTypes.Count)];
            MarkovNameGenerator nameGenerator = gender == "Male" ?
                new MarkovNameGenerator(File.ReadAllLines(@"EntityLibrary\census-dist-male-first.csv"), 2, 1) :
                new MarkovNameGenerator(File.ReadAllLines(@"EntityLibrary\census-dist-female-first.csv"), 2, 1);
            string newName = nameGenerator.NextName;

            newEntity.S = new Dictionary<string, string>() { 
                {"Class", CharacterTypes.ToArray()[rnd.Next(CharacterTypes.Count)]},
                {"Gender", gender},
                {"Name", newName}
            };
            newEntity.T.Add("Conditions", new HashSet<string>());

            // Add default judgements
            newEntity.Morals = JudgementLibrary.DefaultJudgements;

            return newEntity;
        }

        public static List<GameAgent> SampleEntities = new List<GameAgent>()
        {
            new GameAgent()
            {
                 S = new Dictionary<string,string>()
                 {
                     {"Name","BigBully"}
                 },
                 T = new Dictionary<string,HashSet<string>>()
                 {
                     {"Traits", new HashSet<string>() { "Agressive" }}
                 },
                 Morals = JudgementLibrary.DefaultJudgements
            },
            new GameAgent()
            {
                 S = new Dictionary<string,string>()
                 {
                     {"Name","CuteFuzzy"}
                 },
                 T = new Dictionary<string,HashSet<string>>()
                 {
                     {"Traits", new HashSet<string>() { "Peaceful" }}
                 },
                 Morals = JudgementLibrary.DefaultJudgements
            },
            new GameAgent()
            {
                 S = new Dictionary<string,string>()
                 {
                     {"Name","InnocentBystander"}
                 },
                 Morals = JudgementLibrary.DefaultJudgements
            },
        };
    }

   
}
