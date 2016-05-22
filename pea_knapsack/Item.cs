using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pea_knapsack
{
    class Item : ICloneable
    {
        public int Profit { get; set; }
        public int Weight { get; set; }
        public double Ratio { get; set; }
        public int Index { get; set; }
        public int Active { get; set; }

        public Item(int profit, int weight, int index)
        {
            Profit = profit;
            Weight = weight;
            Ratio = profit / (double)weight;
            Index = index;
            Active = 0;
        }

        public Object Clone()
        {
            return MemberwiseClone();
        }
    }
}
