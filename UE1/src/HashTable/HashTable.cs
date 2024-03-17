using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace UE1
{
    public class HashTable
    {
        private const double GoldenCut = 0.6180339887; // Goldener Schnitt Konstante

        private LinkedList<Stock>[] table; // Array von verketteten Listen zur Speicherung von Aktien

        // Konstruktor mit optionaler Tabellengröße
        public HashTable(int tableSize = 1000)
        {
            table = new LinkedList<Stock>[tableSize]; // Initialisierung der Tabelle mit angegebener Größe
        }

        // Aktie zur Hashtabelle hinzufügen
        public void AddStockByName(Stock stock)
        {
            if (GenerateHash(stock.Name, out uint hashKey)) // Hashwert generieren
            {
                table[hashKey] ??= new LinkedList<Stock>(); // Stock initialisieren, falls null
                table[hashKey].AddFirst(stock); // Aktie dem Stock hinzufügen
            }
        }
        
        public void AddStockByStock(Stock stock)
        {
            if (GenerateHash(stock.Symbol, out uint hashKey)) // Hashwert generieren
            {
                table[hashKey] ??= new LinkedList<Stock>(); // Stock initialisieren, falls null
                table[hashKey].AddFirst(stock); // Aktie dem Stock hinzufügen
            }
        }

        // Aktie aus der Hashtabelle entfernen
        public bool TryRemoveStockByName(string name)
        {
            if (GenerateHash(name, out uint hashKey) && table[hashKey] != null)
            {
                if (table[hashKey].Count == 1) // Wenn nur ein Element im Stock
                {
                    table[hashKey] = null; // Stock entfernen
                    return true;
                }

                foreach (Stock item in table[hashKey]) // Durch Stock iterieren
                {
                    if (item.Name == name) // Wenn Aktie gefunden
                    {
                        table[hashKey].Remove(item); // Aktie entfernen
                        break;
                    }
                }

                return true;
            }

            return false;
        }
        
        public bool TryRemoveStockBySymbol(string symbol)
        {
            if (GenerateHash(symbol, out uint hashKey) && table[hashKey] != null)
            {
                if (table[hashKey].Count == 1) // Wenn nur ein Element im Stock
                {
                    table[hashKey] = null; // Stock entfernen
                    return true;
                }

                foreach (Stock item in table[hashKey]) // Durch Stock iterieren
                {
                    if (item.Symbol == symbol) // Wenn Aktie gefunden
                    {
                        table[hashKey].Remove(item); // Aktie entfernen
                        break;
                    }
                }

                return true;
            }

            return false;
        }
        
        public bool TryGetStockByName(string name, out Stock stock)
        {
            stock = null;

            if (GenerateHash(name, out uint hashKey) && table[hashKey] != null)
            {
                foreach (Stock _stock in table[hashKey]) // Durch Stock iterieren
                {
                    if (_stock.Name == name) // Wenn Aktie gefunden
                    {
                        stock = _stock; // Aktie zuweisen
                        return true;
                    }
                }
            }

            return false;
        }
        
        public bool TryGetStockBySymbol(string symbol, out Stock stock)
        {
            stock = null;

            if (GenerateHash(symbol, out uint hashKey) && table[hashKey] != null)
            {
                foreach (Stock _stock in table[hashKey]) // Durch Stock iterieren
                {
                    if (_stock.Symbol == symbol) // Wenn Aktie gefunden
                    {
                        stock = _stock; // Aktie zuweisen
                        return true;
                    }
                }
            }

            return false;
        }

        // Hashwert für einen gegebenen Namen generieren
        private bool GenerateHash(string symbol, out uint key)
        {
            double hashKey = 0;

            for (int i = 0; i < symbol.Length; i++) // Durch Zeichen im Namen iterieren
            {
                hashKey += table.Length * (symbol[i] * GoldenCut % 1); // Hashwert berechnen
            }

            hashKey = Math.Floor(hashKey / symbol.Length); // Hashwert normalisieren

            key = (uint)hashKey;

            if (hashKey < 0 || hashKey > 1000) // Hashwert validieren
            {
                return false;
            }

            return true;
        }

        // Hash-Tabelleninhalt in eine CSV-Datei speichern
        public void SaveToFile(string filename)
        {
            string basePath = "../../../UE1/resources/saved/";
            string fullFilePath = Path.Combine(basePath, "SAVED_" + filename + ".csv");

            using StreamWriter writer = new StreamWriter(fullFilePath); // Datei zum Schreiben öffnen

            foreach (LinkedList<Stock> bucket in table) // Durch Stock iterieren
            {
                if (bucket == null)
                    continue;

                foreach (Stock stock in bucket) // Durch Aktien im Stock iterieren
                {
                    writer.WriteLine($"{stock.Name};{stock.Sin};{stock.Symbol}"); // Aktieninfo schreiben
                    
                    if (stock.Data == null)
                        continue;
                    foreach (StockData stockData in stock.Data) // Durch Aktiendaten iterieren
                    {
                        writer.WriteLine(
                            $"{stockData.date};{stockData.open};{stockData.high};{stockData.low};{stockData.close};{stockData.adjClose};{stockData.volume}"); // Aktiendaten schreiben
                    }
                }
            }

            Console.WriteLine($"Hash-Tabelle gespeichert unter {fullFilePath}");
        }

        // Hash-Tabelleninhalt aus einer CSV-Datei laden
        public void LoadFromFile(string filename)
        {
            string basePath = "../../../UE1/resources/saved/";
            string fullFilePath = Path.Combine(basePath, filename);

            if (!File.Exists(fullFilePath)) // Überprüfen, ob Datei existiert
            {
                Console.WriteLine($"Datei '{filename}' nicht gefunden.");
                return;
            }

            using StreamReader reader = new StreamReader(fullFilePath); // Datei zum Lesen öffnen

            string line;
            while ((line = reader.ReadLine()) != null) // Jede Zeile lesen
            {
                string[] parts = line.Split(';'); // Zeile nach Trennzeichen aufteilen

                if (parts.Length < 3) // Format der Zeile überprüfen
                {
                    Console.WriteLine($"Ungültige Zeile: {line}");
                    continue;
                }

                Stock newStock = new Stock(parts[0], parts[1], parts[2]); // Neues Aktienobjekt erstellen
                newStock.Data = new List<StockData>();

                // Aktiendaten lesen
                while ((line = reader.ReadLine()) != null && !string.IsNullOrWhiteSpace(line))
                {
                    parts = line.Split(';'); // Zeile nach Trennzeichen aufteilen
                    if (parts.Length != 7) // Datenformat überprüfen
                    {
                        Console.WriteLine($"Ungültige Aktiendatenzeile: {line}");
                        continue;
                    }

                    StockData data = new StockData // Aktiendatenobjekt erstellen
                    {
                        date = DateTime.Parse(parts[0]), // Datum parsen
                        open = double.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture), // Öffnungspreis parsen
                        high = double.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture), // Höchstpreis parsen
                        low = double.Parse(parts[3], System.Globalization.CultureInfo.InvariantCulture), // Tiefstpreis parsen
                        close = double.Parse(parts[4], System.Globalization.CultureInfo.InvariantCulture), // Schlusspreis parsen
                        adjClose = double.Parse(parts[5], System.Globalization.CultureInfo.InvariantCulture), // Angepasster Schlusspreis parsen
                        volume = uint.Parse(parts[6], System.Globalization.CultureInfo.InvariantCulture) // Volumen parsen
                    };

                    newStock.Data.Add(data); // Aktiendaten zum Aktienobjekt hinzufügen
                }

                AddStockByName(newStock); // Aktie zur Hashtabelle hinzufügen
            }

            Console.WriteLine($"Hash-Tabelle geladen aus {fullFilePath}");
        }
    }
}