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
            Console.WriteLine("Geben Sie den Aktiennamen für das Diagramm ein:");
            string name = Console.ReadLine();

            Stopwatch watch = Stopwatch.StartNew(); 
            if (hashTable.TryGetStock(name, out Stock stock)) // Versuche Aktie aus Hashtable zu erhalten
            {
                stock.Plot(100, 30); // Plot-Funktion für Aktie aufrufen
            }

            watch.Stop(); 
            Console.WriteLine("Aktie erstellen: " + watch.Elapsed); // Ausgabe der Zeit zur Aktienerstellung
        }

        public static void SaveHashtableToFile()
        {
            Console.WriteLine("Geben Sie den Dateinamen zum Speichern ein:");
            string saveFilename = Console.ReadLine();
            Stopwatch watch = Stopwatch.StartNew(); 
            hashTable.SaveToFile(saveFilename); // Hashtable in Datei speichern
            watch.Stop(); 
            Console.WriteLine("Hashtable speichern: " + watch.Elapsed); // Ausgabe der Zeit zum Speichern der Hashtable
        }

        public static void LoadHashtableFromFile()
        {
            Console.WriteLine("Geben Sie den Dateinamen zum Laden ein:");
            string loadFilename = Console.ReadLine();
            Stopwatch watch = Stopwatch.StartNew(); 
            hashTable.LoadFromFile(loadFilename); // Hashtable aus Datei laden
            watch.Stop(); 
            Console.WriteLine("Hashtable laden: " + watch.Elapsed); // Ausgabe der Zeit zum Laden der Hashtable
        }

        public static void DeleteStock()
        {
            Console.WriteLine("Geben Sie den Aktiennamen zum Löschen ein:");
            string stockName = Console.ReadLine();
            Stopwatch watch = Stopwatch.StartNew(); 
            if (!hashTable.TryRemoveStockByName(stockName)) // Versuche Aktie aus Hashtable zu entfernen
            {
                Console.WriteLine(
                    "Aktie kann nicht entfernt werden");
            }

            watch.Stop(); 
            Console.WriteLine("Aktie löschen: " + watch.Elapsed); // Ausgabe der Zeit zum Löschen der Aktie
        }

        private static void ImportStock()
        {
            // Benutzereingabe für Aktiennamen und Dateinamen für den Datenimport
            Console.WriteLine("Geben Sie den Aktiennamen für den Datenimport ein:");
            string stockName = Console.ReadLine();
            Console.WriteLine("Geben Sie den Dateinamen für den Import ein:");
            string fileName = Console.ReadLine();

            Stopwatch watch = Stopwatch.StartNew(); 
            if (hashTable.TryGetStock(stockName, out Stock s)) // Versuche Aktie aus Hashtable zu erhalten
            {
                if (!TryReadStockValuesFromCSV(fileName, s)) // Versuche Aktiendaten aus CSV-Datei zu lesen
                {
                    Console.WriteLine(
                        "Aktie kann nicht gelesen werden");
                }
            }
            else
            {
                Console.WriteLine("Aktie " + stockName + " nicht gefunden"); // Ausgabe falls Aktie nicht gefunden wurde
            }

            watch.Stop(); 
            Console.WriteLine("Aktie importieren: " + watch.Elapsed); // Ausgabe der Zeit zum Importieren der Aktie
        }

        private static bool TryReadStockValuesFromCSV(string fileName, Stock stock)
        {
            // Pfad zur CSV-Datei
            string basePath = "../../../UE1/resources/";
            string fullFilePath = Path.Combine(basePath, fileName);

            if (!File.Exists(fullFilePath)) // Überprüfe, ob die Datei existiert
            {
                Console.WriteLine("Datei nicht gefunden: " + fullFilePath +
                                  " Stellen Sie sicher, dass die Datei im /resources Ordner liegt");
                return false;
            }

            using StreamReader reader = new StreamReader(fullFilePath); // Datei zum Lesen öffnen
            reader.ReadLine(); // Überspringe Kopfzeile

            List<StockData> data = new List<StockData>(); // Liste für Aktiendaten
            while (!reader.EndOfStream) // Solange nicht das Ende der Datei erreicht ist
            {
                string line = reader.ReadLine(); // Lese eine Zeile
                string[] values = line.Split(','); // Teile die Zeile an Kommas auf

                // Aktiendaten zur Liste hinzufügen
                data.Add(new StockData
                {
                    date = DateTime.Parse(values[0]), // Datum parsen
                    open = double.Parse(values[1],
                        System.Globalization.CultureInfo.InvariantCulture), // Öffnungspreis parsen
                    high = double.Parse(values[2],
                        System.Globalization.CultureInfo.InvariantCulture), // Höchstpreis parsen
                    low = double.Parse(values[3],
                        System.Globalization.CultureInfo.InvariantCulture), // Tiefstpreis parsen
                    close = double.Parse(values[4],
                        System.Globalization.CultureInfo.InvariantCulture), // Schlusspreis parsen
                    adjClose = double.Parse(values[5],
                        System.Globalization.CultureInfo.InvariantCulture), // Angepasster Schlusspreis parsen
                    volume = uint.Parse(values[6], System.Globalization.CultureInfo.InvariantCulture) // Volumen parsen
                });

                stock.Data = data; // Aktiendaten zur Aktie hinzufügen
            }

            return true; // Erfolgreich gelesen
        }

        private static Stock CreateStock()
        {
            Console.WriteLine("Geben Sie den Aktiennamen ein:");
            string name = Console.ReadLine();

            Console.WriteLine("Geben Sie die SIN ein:");
            string sin = Console.ReadLine();

            Console.WriteLine("Geben Sie das Symbol ein:");
            string symbol = Console.ReadLine();

            return new Stock(name, sin, symbol); // Aktie erstellen und zurückgeben
        }

        private static void SearchStock()
        {
            Console.WriteLine("Geben Sie den Aktiennamen ein:");
            string userInput = Console.ReadLine();

            Stopwatch watch = Stopwatch.StartNew(); 
            if (hashTable.TryGetStock(userInput, out Stock s)) // Versuche Aktie aus Hashtable zu erhalten
            {
                StockData data = s.Data[s.Data.Count - 1]; // Letzte Aktiendaten abrufen
                Console.WriteLine("Aktiennamen: " + s.Name);
                Console.WriteLine("SIN: " + s.Sin);
                Console.WriteLine("Symbol: " + s.Symbol);

                // Ausgabe der letzten Aktiendaten
                Console.WriteLine("Datum: " + data.date.ToString("dd/MM/yyyy"));
                Console.WriteLine("Öffnen: " + data.open);
                Console.WriteLine("Hoch: " + data.high);
                Console.WriteLine("Tief: " + data.low);
                Console.WriteLine("Schließen: " + data.close);
                Console.WriteLine("adjClose: " + data.adjClose);
                Console.WriteLine("Volumen: " + data.volume);
            }
            else
            {
                Console.WriteLine("Aktie " + userInput + " existiert nicht"); // Ausgabe falls Aktie nicht gefunden wurde
            }

            watch.Stop(); 
            Console.WriteLine("Aktie suchen: " + watch.Elapsed); // Ausgabe der Zeit zur Suche der Aktie
        }
    }
}