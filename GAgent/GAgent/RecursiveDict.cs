using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    public struct RecursiveDict
    {
        public Dictionary<string, RecursiveDict> P; // for properties
        public List<RecursiveDict> L; // for lists of properties
        public Dictionary<string, double> N; // for numeric values
        public Dictionary<string, string> S; // for string values
    }

    // Well, I guess this could be my database?
    // Honestly, I should just use JSON as a store then.  Easy to manipulate and store
    public class recursiveDictTester
    {
        public recursiveDictTester()
        {

            RecursiveDict myRD = new RecursiveDict()
            {
                L =
                {
                    new RecursiveDict()
                    {
                        S = { {"StringProperty1", "stringvalue1" } }
                    },
                    new RecursiveDict()
                    {
                        N = { {"Numberprop1", 1 } }
                    },
                },
                S =
                {
                    {"Name", "AgentName" },
                },
                N =
                {
                    {"Age", 38 }
                },
                P =
                {
                    { "Tools", new RecursiveDict()
                        {
                            S =
                            {
                                { "ToolName", "Hammer" }
                            },
                            N =
                            {
                                { "ToolWeight", 200 }
                            }
                        }
                    }
                }
            };
            var myToolWeight = myRD.P["Tools"].N["ToolWeight"];
        }
    }
}
