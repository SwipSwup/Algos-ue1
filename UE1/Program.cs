using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace UE1
{
    internal class Program
    {
        private static HashTable hashTable = new HashTable();

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter 'add': Eine Aktie hinzufügen");
                Console.WriteLine("Enter 'del': Eine Aktie löschen");
                Console.WriteLine("Enter 'import': Kurswerte aus CSV importieren");
                Console.WriteLine("Enter 'search': Aktie suchen");
                Console.WriteLine("Enter 'plot': Schlusskurse der letzten 30 Tage als Grafik ausgeben");
                Console.WriteLine("Enter 'save': Hashtabelle in Datei abspeichern");
                Console.WriteLine("Enter 'load': Hashtabelle aus Datei laden");
                Console.WriteLine("Enter 'quit': Programm beenden");
                Console.WriteLine("Bitte wählen Sie eine Aktion aus:");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "add":
                        hashTable.AddStock(CreateStock());
                        break;
                    case "del":
                        break;
                    case "import":
                        ImportStock();
                        break;
                    case "search":
                        SearchStock();
                        break;
                    case "plot":
                        break;
                    case "save":
                        break;
                    case "load":
                        break;
                    case "quit":
                        return;
                    default:
                        Console.WriteLine("Ungültiger Befehl!");
                        break;
                }
            }
        }

        private static void ImportStock()
        {
            Console.WriteLine("Enter Stockname to Insert Data");
            string stockName = Console.ReadLine();
            Console.WriteLine("Enter Filename for Import");
            string fileName = Console.ReadLine();
            if (hashTable.TryGetStock(stockName, out Stock? s))
            {
                if (!TryReadStockValuesFromCSV(fileName, (Stock)s))
                {
                    Console.WriteLine("Cant read Stock");
                }
            }
        }

        private static bool TryReadStockValuesFromCSV(string fileName, Stock stock)
        {
            string basePath = "../../../UE1/resources/";
            string fullFilePath = Path.Combine(basePath, fileName);

            if (!File.Exists(fullFilePath))
            {
                Console.WriteLine("File not Found: " + fullFilePath + " make sure that file is in /resources folder");

                return false;
            }

            using (StreamReader reader = new StreamReader(fullFilePath))
            {
                reader.ReadLine();

                stock.data = new List<StockData>();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    stock.data.Add(new StockData
                    {
                        date = DateTime.Parse(values[0]),
                        open = double.Parse(values[1]),
                        high = double.Parse(values[2]),
                        low = double.Parse(values[3]),
                        close = double.Parse(values[4]),
                        adjClose = double.Parse(values[5]),
                        volume = uint.Parse(values[6])
                    });
                }
            }

            return true;
        }

        private static Stock CreateStock()
        {
            Stock newStock = new Stock();

            Console.WriteLine("Enter Stockname: ");
            string userInput = Console.ReadLine();
            newStock.name = userInput;

            Console.WriteLine("Enter SIN: ");
            userInput = Console.ReadLine();
            newStock.sin = userInput;

            Console.WriteLine("Enter Symbol: ");
            userInput = Console.ReadLine();
            newStock.symbol = userInput;

            return newStock;
        }

        private static void SearchStock()
        {


            Console.WriteLine("(1) Search by Stockname ");
            Console.WriteLine("(2) Search by SIN ");
            Console.WriteLine("Enter 1 or 2 to choose: ");
            string userInput = Console.ReadLine();

            if (userInput == "1")
            {
                Console.WriteLine("Enter Stockname: ");
                userInput = Console.ReadLine();
                hashTable.TryGetStock(userInput, out Stock? s);

                Stock stock = (Stock)s;
                Console.WriteLine(stock.data[0]);
            }
            else if (userInput == "2")
            {
                Console.WriteLine("(2) Search by SIN ");
            }
            else
            {
                Console.WriteLine("Enter a number between 1-2");
            }


        }


    }
}