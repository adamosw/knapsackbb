using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace pea_knapsack
{
    class Knapsack
    {
        public int NumberOfItems { get; set; }
        public int KnapsackCapacity { get; set; }
        public List<Item> ListOfItems { get; set; }

        public Knapsack()
        {
            ListOfItems = new List<Item>();
        }

        public void GetData()
        {
            ListOfItems = new List<Item>();

            string fileName, path;

            ListOfItems = new List<Item>();

            Console.Write("Podaj nazwe pliku: ");
            fileName = Console.ReadLine();

            path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" + fileName;

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string fileContent = File.ReadAllText(path);
                    string[] stringPieces = fileContent.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> stringList = stringPieces.ToList<string>();

                    int testInt = 0;
                    for(int i = 0; i < stringList.Count; i++)
                    {
                        Int32.TryParse(stringList[i], out testInt);
                        if(testInt == 0)
                        {
                            stringList.RemoveAt(i);
                        }
                    }

                    int tmpInt;

                    Int32.TryParse(stringList[0], out tmpInt);
                    NumberOfItems = tmpInt;

                    int tmpWeight, tmpProfit, index;
                    index = 1;

                    for (int i = 1; i < stringList.Count - 1; i = i + 2)
                    {
                        Int32.TryParse(stringList[i], out tmpWeight);

                        Int32.TryParse(stringList[i + 1], out tmpProfit);

                        ListOfItems.Add(new Item(tmpProfit, tmpWeight, index));

                        index = index + 1;
                    }

                    Console.Write("Podaj pojemnosc plecaka: ");

                    int tmpCapacity;
                    Int32.TryParse(Console.ReadLine(), out tmpCapacity);
                    KnapsackCapacity = tmpCapacity;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Blad wczytywania pliku.");
                Console.WriteLine(ex.Message);
            }
        }

        public void ShowItems()
        {
            Console.WriteLine("Pojemnosc pleaka: {0}", KnapsackCapacity);

            int i = 0;

            foreach (Item tmpItem in ListOfItems)
            {
                Console.WriteLine("{0}. Waga: {1}, Wartosc: {2}, Stosunek: {3}", i, tmpItem.Weight, tmpItem.Profit, tmpItem.Ratio);
                i++;
            }
        }

        public void SortItems()
        {
            int change = 0;
            Item tmpItem = new Item(0, 0, 0);

            do
            {
                change = 0;
                for (int i = 0; i < NumberOfItems - 1; i++)
                {
                    if (ListOfItems[i].Ratio < ListOfItems[i + 1].Ratio)
                    {
                        tmpItem = ListOfItems[i];

                        ListOfItems[i] = ListOfItems[i + 1];
                        ListOfItems[i + 1] = tmpItem;

                        change = change + 1;
                    }
                }
            }
            while (change != 0);

            int index = 0;
            foreach(Item item in ListOfItems)
            {
                item.Index = index;
                index++;
            }
        }

        public void SolveProblem()
        {
            Queue<Node> mainQueue = new Queue<Node>();

            Node parentNode = new Node();
            parentNode.Level = 0;

            foreach(Item tmpItem in ListOfItems)
            {
                parentNode.NodeItems.Add((Item)tmpItem.Clone());
            }

            parentNode.SetBound(KnapsackCapacity);
            parentNode.Profit = 0;
            parentNode.Weight = 0;

            List<Item> bestItems = new List<Item>();
            int bestResult;
            int bestWeight = 0;

            mainQueue.Enqueue(parentNode);
            bestResult = parentNode.Profit;

            while(mainQueue.Count != 0)
            {
                parentNode = mainQueue.Dequeue();
                parentNode.SetChildren(KnapsackCapacity);

                foreach (Node nodeChild in parentNode.NodeChildren)
                {
                    if(nodeChild.Profit >= bestResult && nodeChild.Weight <= KnapsackCapacity)
                    {
                        bestResult = nodeChild.Profit;
                        bestItems = new List<Item>();
                        foreach (Item tmpItem in nodeChild.NodeItems)
                        {
                            bestItems.Add((Item)tmpItem.Clone());
                        }
                        bestWeight = nodeChild.Weight;
                    }

                    if(nodeChild.Bound >= bestResult && nodeChild.Weight <= KnapsackCapacity && nodeChild.Level != ListOfItems.Count)
                    {
                        mainQueue.Enqueue(nodeChild);
                    }
                }
                
            }
            Console.Write("BestResult: {0}", bestResult);
            Console.WriteLine();
            Console.Write("BestWeight: {0}", bestWeight);
            Console.WriteLine();
            Console.Write("BestItems: ");
            foreach(Item tmpItem in bestItems)
            {
                if(tmpItem.Active == 0)
                    Console.Write("{0} ", tmpItem.Index);
            }
            Console.WriteLine();
        }

        public void SolveProblemTime()
        {   
            Queue<Node> mainQueue = new Queue<Node>();

            Node parentNode = new Node();
            parentNode.Level = 0;

            foreach (Item tmpItem in ListOfItems)
            {
                parentNode.NodeItems.Add((Item)tmpItem.Clone());
            }

            parentNode.SetBound(KnapsackCapacity);
            parentNode.Profit = 0;
            parentNode.Weight = 0;

            List<Item> bestItems = new List<Item>();
            int bestResult;

            mainQueue.Enqueue(parentNode);
            bestResult = parentNode.Profit;

            while (mainQueue.Count != 0)
            {
                parentNode = mainQueue.Dequeue();
                parentNode.SetChildren(KnapsackCapacity);

                foreach (Node nodeChild in parentNode.NodeChildren)
                {
                    if (nodeChild.Profit >= bestResult && nodeChild.Weight <= KnapsackCapacity)
                    {
                        bestResult = nodeChild.Profit;
                        bestItems = new List<Item>();
                        foreach (Item tmpItem in nodeChild.NodeItems)
                        {
                            bestItems.Add((Item)tmpItem.Clone());
                        }
                    }

                    if (nodeChild.Bound >= bestResult && nodeChild.Weight <= KnapsackCapacity && nodeChild.Level != ListOfItems.Count)
                    {
                        mainQueue.Enqueue(nodeChild);
                    }
                }

            }
        }

        public void CountTime(int n)
        {
            double totalTime = 0;

            for(int i = 0; i < 100; i++)
            {
                var timer = Stopwatch.StartNew();
                GenerateData(n);
                SortItems();
                SolveProblemTime();
                timer.Stop();

                var elapsedMs = timer.ElapsedMilliseconds;
                totalTime = totalTime + elapsedMs;
            }

            totalTime = totalTime / 100;

            Console.WriteLine("Total time: {0} ms", totalTime);
        }

        public void GenerateData(int n)
        {
            Random rnd = new Random();
            ListOfItems = new List<Item>();
            int tmpBenefit, tmpWeight, tmpIndex;

            NumberOfItems = n;
            KnapsackCapacity = n;

            for(int i = 0; i < n; i++)
            {
                tmpBenefit = rnd.Next(1, n);
                tmpWeight = rnd.Next(1, n);
                tmpIndex = i + 1;

                ListOfItems.Add(new Item(tmpBenefit, tmpWeight, tmpIndex));
            }
        }
    }
}
