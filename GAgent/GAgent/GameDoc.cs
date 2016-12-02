using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    public class GameDoc
    {
        public Dictionary<string, GameDoc> P; // for properties
        public List<GameDoc> L; // for lists of properties
        public Dictionary<string, double> N; // for numeric values
        public Dictionary<string, string> S; // for string values
    }

    public class GameDocTester
    {
        public GameDocTester()
        {

        }
    }

}
