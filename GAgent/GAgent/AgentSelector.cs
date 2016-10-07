using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    // A selector delegate returns a selection of agents from the gameworld, accessible through a dictionary
    public delegate Dictionary<string, GameAgent> AgentSelectorDelegate(GameWorld world);

    public class AgentSelector
    {
        private string ID;
        private string Description;
        private AgentSelectorDelegate Selector;

        public AgentSelector(string inId, string inDesc, AgentSelectorDelegate inSelector)
        {
            ID = inId;
            Description = inDesc;
            Selector = inSelector;
        }

        public Dictionary<string, GameAgent> GetAgents(GameWorld world)
        {
            Dictionary<string, GameAgent> result = null;
            //Console.WriteLine("Selector '" + Description + "': ");
            result = Selector(world);
            Console.WriteLine("    Selector '" + Description + "': " + (result != null ? " - " + result.Count + " values " : " - RETURNED NULL"));
            return result;
        }
    }
}
