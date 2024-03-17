using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace UE1
{
    internal class Program
    {
        private static HashTable hashTable = new();

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
                    // 00:00:00.0003859
                    case "add":
                        Stock stock = CreateStock();
                        Stopwatch watch = Stopwatch.StartNew();
                        hashTable.AddStock(stock);
                        watch.Stop();
                        Console.WriteLine("Create Stock: " + watch.Elapsed);
                        break;
                    // 00:00:00.0003670
                    case "del":
                        DeleteStock();
                        break;
                    // 00:00:00.0044810
                    case "import":
                        ImportStock();
                        break;
                    // 00:00:00.0015249
                    case "search":
                        SearchStock();
                        break;
                    // 00:00:00.0643547
                    case "plot":
                        PlotStock();
                        break;
                    // 00:00:00.0028552
                    case "save":
                        SaveHashtableToFile();
                        break;
                    case "load":
                        LoadHashtableFromFile();
                        break;
                    case "quit":
                        return;
                    default:
                        Console.WriteLine("Ungültiger Befehl!");
                        break;
                }
            }
        }

        public static void PlotStock()
        {
            Console.WriteLine("Enter stockname to plot:");
            string name = Console.ReadLine();

            Stopwatch watch = Stopwatch.StartNew();
            if (hashTable.TryGetStock(name, out Stock stock))
            {
                stock.Plot(100, 30);
            }
            watch.Stop();
            Console.WriteLine("Create Stock: " + watch.Elapsed);
        }

        public static void SaveHashtableToFile()
        {
            Console.WriteLine("Enter filename to save:");
            string saveFilename = Console.ReadLine();
            Stopwatch watch = Stopwatch.StartNew();
            hashTable.SaveToFile(saveFilename);
            watch.Stop();
            Console.WriteLine("Save hashtable: " + watch.Elapsed);
        }

        public static void LoadHashtableFromFile()
        {
            Console.WriteLine("Enter filename to load:");
            string loadFilename = Console.ReadLine();
            Stopwatch watch = Stopwatch.StartNew();
            hashTable.LoadFromFile(loadFilename);
            watch.Stop();
            Console.WriteLine("Load hashtable: " + watch.Elapsed);
        }

        public static void DeleteStock()
        {
            Console.WriteLine("Enter Stockname to delete;");
            string stockName = Console.ReadLine();
            Stopwatch watch = Stopwatch.StartNew();
            if (!hashTable.TryRemoveStock(stockName))
            {
                Console.WriteLine("Cant remove stock");
            }

            watch.Stop();
            Console.WriteLine("Delete stock: " + watch.Elapsed);
        }

        private static void ImportStock()
        {
            Console.WriteLine("Enter Stockname to Insert Data");
            string stockName = Console.ReadLine();
            Console.WriteLine("Enter Filename for Import");
            string fileName = Console.ReadLine();

            Stopwatch watch = Stopwatch.StartNew();
            if (hashTable.TryGetStock(stockName, out Stock s))
            {
                if (!TryReadStockValuesFromCSV(fileName, s))
                {
                    Console.WriteLine("Cant read Stock");
                }
            }
            else
            {
                Console.WriteLine("Cant find stock " + stockName);
            }

            watch.Stop();
            Console.WriteLine("Import stock: " + watch.Elapsed);
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

            using StreamReader reader = new StreamReader(fullFilePath);
            reader.ReadLine();

            List<StockData> data = new List<StockData>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                data.Add(new StockData
                {
                    date = DateTime.Parse(values[0]),
                    open = double.Parse(values[1], System.Globalization.CultureInfo.InvariantCulture),
                    high = double.Parse(values[2], System.Globalization.CultureInfo.InvariantCulture),
                    low = double.Parse(values[3], System.Globalization.CultureInfo.InvariantCulture),
                    close = double.Parse(values[4], System.Globalization.CultureInfo.InvariantCulture),
                    adjClose = double.Parse(values[5], System.Globalization.CultureInfo.InvariantCulture),
                    volume = uint.Parse(values[6], System.Globalization.CultureInfo.InvariantCulture)
                });

                stock.Data = data;
            }

            return true;
        }

        private static Stock CreateStock()
        {
            Console.WriteLine("Enter Stockname: ");
            string name = Console.ReadLine();

            Console.WriteLine("Enter SIN: ");
            string sin = Console.ReadLine();

            Console.WriteLine("Enter Symbol: ");
            string symbol = Console.ReadLine();

            return new Stock(name, sin, symbol);
        }

        private static void SearchStock()
        {
            Console.WriteLine("Enter Stockname: ");
            string userInput = Console.ReadLine();

            Stopwatch watch = Stopwatch.StartNew();
            if (hashTable.TryGetStock(userInput, out Stock s))
            {
                StockData data = s.Data[s.Data.Count - 1];
                Console.WriteLine("Stockname: " + s.Name);
                Console.WriteLine("SIN: " + s.Sin);
                Console.WriteLine("Symbol: " + s.Symbol);

                Console.WriteLine("Date: " + data.date.ToString("dd/MM/yyyy"));
                Console.WriteLine("Open: " + data.open);
                Console.WriteLine("High: " + data.high);
                Console.WriteLine("Low: " + data.low);
                Console.WriteLine("Close: " + data.close);
                Console.WriteLine("adjClose: " + data.adjClose);
                Console.WriteLine("Volume: " + data.volume);
            }
            else
            {
                Console.WriteLine("Stock " + userInput + " doesn't exist");
            }

            watch.Stop();
            Console.WriteLine("Search stock: " + watch.Elapsed);
        }
    }
}