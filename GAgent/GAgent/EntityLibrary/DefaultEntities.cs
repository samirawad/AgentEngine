using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

       
        public static List<List<string>> PersonalityTags = new List<List<string>>()
        {
            // Extraversion
            new List<string>() {"warm","aloof"},   
            new List<string>() {"gregarious","shy"},
            new List<string>() {"assertive","unassertive"},
            new List<string>() {"energetic","leisurely"},
            new List<string>() {"adventurous","complacent"},
            new List<string>() {"cheerful","grim"},
            // Agreeableness
            new List<string>() {"trusting","distrustful"},
            new List<string>() {"candid","guarded"},
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
            new List<string>() {"resolute","wavering"},
            new List<string>() {"composed","overwrought"}, // ability to handle present stress
            // Openness
            new List<string>() {"imaginative","unimaginative"},
            new List<string>() {"cultured","uncultured"},
            new List<string>() {"reflective","unreflective"},   
            new List<string>() {"curious","incurious"},
            new List<string>() {"philosophical","realist"},

            // Political
            new List<string>() {"accepting","bigoted"}, // of other races
            new List<string>() {"unconventional","traditional"}, // of their origin culture
            new List<string>() {"irreverant","religious"}, // of religion
            new List<string>() {"unpatriotic","patriotic"}, // of the establisment 

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
        public static GameEntity GenerateEntity()
        {
            GameEntity newEntity = new GameEntity();
            newEntity.T = new Dictionary<string, HashSet<string>>();
            newEntity.T.Add("Personality", new HashSet<string>());
            foreach(List<string> currTags in PersonalityTags)
            {
                int roll = rnd.Next(100);
                if (roll > 90)
                {
                    string newTrait = currTags.ToArray()[rnd.Next(currTags.Count)];
                    if (roll > 97) newTrait = "profoundly " + newTrait;
                    newEntity.T["Personality"].Add(newTrait);
                }           
            }
            newEntity.S = new Dictionary<string, string>() { 
                {"Class", CharacterTypes.ToArray()[rnd.Next(CharacterTypes.Count)]},
                {"Gender", GenderTypes.ToArray()[rnd.Next(GenderTypes.Count)]}
            };
            return newEntity;
        }

        public static List<GameEntity> SampleEntities = new List<GameEntity>()
        {
            new GameEntity()
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
            new GameEntity()
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
            new GameEntity()
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
