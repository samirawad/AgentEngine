using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    class Selector
    {
        public Object Item;
        public List<Selector> Selected;
        public List<Selector> Selection;

        private int Min;
        private int Max;
        private Random rnd;

        public Selector(Object inItem, int min, int max)
        {
            Min = min;
            Max = max;
            Item = inItem;
            rnd = new Random();
        }

        public void Select()
        {
            if (Selection != null)
            {
                Selected = new List<Selector>();
                Queue<Selector> selectQueue = new Queue<Selector>(Selection);
                int toTake = rnd.Next(Min, Max + 1);
                toTake = toTake > selectQueue.Count ? selectQueue.Count : toTake;
                for (int i = 0; i < toTake; i++)
                {
                    //Randomize
                    for (int j = 0; j < rnd.Next(selectQueue.Count); j++)
                    {
                        selectQueue.Enqueue(selectQueue.Dequeue());
                    }
                    Selected.Add(selectQueue.Dequeue());
                }
                Selected.ForEach(p => p.Select());
            }
        }
    }
}
