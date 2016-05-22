using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace pea_knapsack
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice;
            int numberOfItems = 0;

            Knapsack  mainKnapsack = new Knapsack();

            do
            {
                Console.WriteLine("PEA - Laboratorium - Zadanie 1");
                Console.WriteLine("Dyskretny problem plecakowy - Metoda podzialu i ograniczen");
                Console.WriteLine("1. Wczytaj dane.");
                Console.WriteLine("2. Wyswietl przedmioty.");
                Console.WriteLine("3. Sortuj przedmioty.");
                Console.WriteLine("4. Uruchom algorytm.");
                Console.WriteLine("5. Wygeneruj dane.");
                Console.WriteLine("6. Mierzenie czasu.");
                Console.WriteLine("7. Koniec.");

                Console.Write("Podaj swoj wybor: ");
                Int32.TryParse(Console.ReadLine(), out choice);

                switch (choice)
                {
                    case 1:
                        mainKnapsack.GetData();
                        mainKnapsack.ShowItems();
                        break;
                    case 2:
                        mainKnapsack.ShowItems();
                        break;
                    case 3:
                        mainKnapsack.SortItems();
                        mainKnapsack.ShowItems();
                        break;
                    case 4:
                        mainKnapsack.SortItems();
                        mainKnapsack.SolveProblem();
                        break;
                    case 5:
                        Console.Write("Podaj liczbe przedmiotow: ");
                        Int32.TryParse(Console.ReadLine(), out numberOfItems);
                        mainKnapsack.GenerateData(numberOfItems);
                        break;
                    case 6:
                        Console.Write("Podaj liczbe przedmiotow: ");
                        Int32.TryParse(Console.ReadLine(), out numberOfItems);
                        mainKnapsack.CountTime(numberOfItems);
                        break;
                    case 7:
                        break;
                    default:
                        break;
                }
            }
            while (choice != 7);
        }
    }
}
