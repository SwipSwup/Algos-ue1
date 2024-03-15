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
                        hashTable.DeleteStock();
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
                        Console.WriteLine("Enter filename to save:");
                        string saveFilename = Console.ReadLine();
                        hashTable.SaveToFile(saveFilename);
                        break;
                    case "load":
                        Console.WriteLine("Enter filename to load:");
                        string loadFilename = Console.ReadLine();
                        hashTable.LoadFromFile(loadFilename);
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
                Stock stock = (Stock)s;
                if (!TryReadStockValuesFromCSV(fileName, ref stock))
                {
                    Console.WriteLine("Cant read Stock");
                }
            }
        }

        private static bool TryReadStockValuesFromCSV(string fileName, ref Stock stock)
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
                        open = double.Parse(values[1], System.Globalization.CultureInfo.InvariantCulture),
                        high = double.Parse(values[2], System.Globalization.CultureInfo.InvariantCulture),
                        low = double.Parse(values[3], System.Globalization.CultureInfo.InvariantCulture),
                        close = double.Parse(values[4], System.Globalization.CultureInfo.InvariantCulture),
                        adjClose = double.Parse(values[5], System.Globalization.CultureInfo.InvariantCulture),
                        volume = uint.Parse(values[6], System.Globalization.CultureInfo.InvariantCulture)
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
            Console.WriteLine("Enter Stockname: ");
            string userInput = Console.ReadLine();

            if (hashTable.TryGetStock(userInput, out Stock? s))
            {
                Console.WriteLine(s);
                Console.WriteLine(s?.data[0]);
            }
        }
    }
}