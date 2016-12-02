using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    // A selector delegate returns a selection of agents from the gameworld, accessible through a dictionary
    public delegate Dictionary<string, List<GameAgent>> AgentSelectorDelegate(GameWorld world);

    public class AgentSelector
    {
        private string ID;
        private string Description;
        private AgentSelectorDelegate Selector;
        private bool _debug = false;

        public AgentSelector(string inId, string inDesc, AgentSelectorDelegate inSelector)
        {
            ID = inId;
            Description = inDesc;
            Selector = inSelector;
        }

        public AgentSelector(string inId, string inDesc, AgentSelectorDelegate inSelector, bool inDebug)
        {
            ID = inId;
            Description = inDesc;
            Selector = inSelector;
            _debug = inDebug;
        }

        public Dictionary<string, List<GameAgent>> GetAgents(GameWorld world)
        {
            Dictionary<string, List<GameAgent>> result = Selector(world);
            if(_debug) Console.WriteLine("    Selector '" + Description + "': " + (result != null ? " - " + result.Count + " values " : " - RETURNED NULL"));
            return result;
        }
    }
}
