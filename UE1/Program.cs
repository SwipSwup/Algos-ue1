using System;
using System.IO;
using System.Collections.Generic;

namespace UE1
{
    internal class Program
    {


        Stock ReadStockValuesFromCSV(string fileName, Stock stock)
        {


            string basePath = "../../../UE1/csv/";
            string fullFilePath = Path.Combine(basePath, fileName);

            if (!File.Exists(fullFilePath))
            {
                Console.WriteLine("File not Found: " + fullFilePath + " make sure that file is in /csv folder");

               return stock;
            }

            using (var reader = new StreamReader(fullFilePath))
            {
                //Header Zeile ignorieren
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    Stock importedStock = new Stock();
                    stock.open = double.Parse(values[1]);
                    stock.high = double.Parse(values[2]);
                    stock.date = DateTime.Parse(values[0]);
                    stock.low = double.Parse(values[3]);
                    stock.close = double.Parse(values[4]);
                    stock.adjClose = double.Parse(values[5]);
                    stock.volume = uint.Parse(values[6]);

                    return importedStock;
                }
            }

            return default;
        }

        Stock CreateStock()
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

        public static void Main(string[] args)
        {
            Program program = new Program();
            HashTable hashTable = new HashTable();


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
                        hashTable.AddStock(program.CreateStock());
                        break;

                    case "del":

                        break;

                    case "import":
                        Console.WriteLine("Enter Stockname to Insert Data");
                        string stockName = Console.ReadLine();
                        Console.WriteLine("Enter Filename for Import");
                        string fileName = Console.ReadLine();
                        program.ReadStockValuesFromCSV(fileName, hashTable.GetStock(stockName));
                        break;

                    case "search":
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
    }
}