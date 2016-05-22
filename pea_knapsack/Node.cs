using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pea_knapsack
{
    class Node
    {
        public int Profit { get; set; }
        public int Weight { get; set; }
        public int Bound { get; set; }
        public int Level { get; set; }

        public List<Item> NodeItems;
        public List<Node> NodeChildren;

        public Node()
        {
            NodeItems = new List<Item>();
            NodeChildren = new List<Node>();
        }

        public void SetChildren(int knapsackCapacity)
        {
            Node tmpNode = new Node();
            tmpNode.Profit = Profit + NodeItems[Level].Profit;
            tmpNode.Weight = Weight + NodeItems[Level].Weight;
            tmpNode.Bound = Bound;
            tmpNode.Level = Level + 1;
            foreach (Item tmpItem in NodeItems)
            {
                tmpNode.NodeItems.Add((Item)tmpItem.Clone());
            }

            NodeChildren.Add(tmpNode);

            tmpNode = new Node();
            tmpNode.Profit = Profit;
            tmpNode.Weight = Weight;

            foreach (Item tmpItem in NodeItems)
            {
                tmpNode.NodeItems.Add((Item)tmpItem.Clone());
            }

            tmpNode.NodeItems[Level].Active = 1;
            tmpNode.SetBound(knapsackCapacity);
            tmpNode.Level = Level + 1;

            NodeChildren.Add(tmpNode);
        }

        public void SetBound(int knapsackCapacity)
        {
            int tmpWeight = 0;
            int tmpBound = 0;
            int difference = 0;

            foreach(Item tmpItem in NodeItems)
            {
                if(tmpItem.Active == 0)
                {
                    if (tmpWeight <= knapsackCapacity)
                    {
                        tmpBound = tmpBound + tmpItem.Profit;
                        tmpWeight = tmpWeight + tmpItem.Weight;
                    }

                    if (tmpWeight > knapsackCapacity)
                    {
                        tmpBound = tmpBound - tmpItem.Profit;
                        tmpWeight = tmpWeight - tmpItem.Weight;

                        difference = knapsackCapacity - tmpWeight;

                        tmpBound = tmpBound + ((difference * tmpItem.Profit) / tmpItem.Weight);
                        break;
                    }
                }
            }
            Bound = tmpBound;
        }
    }
}
