using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace UE1
{
    public class HashTable
    {
        private const double GoldenCut = 0.6180339887;

        private LinkedList<Stock>[] table;

        public HashTable(int tableSize = 1000)
        {
            table = new LinkedList<Stock>[tableSize];
        }

        public void AddStock(Stock stock)
        {
            if (GenerateHash(stock.Name, out uint hashKey))
            {
                table[hashKey] ??= new LinkedList<Stock>();
                table[hashKey].AddFirst(stock);
            }
        }

        public bool TryRemoveStock(string name)
        {
            if (GenerateHash(name, out uint hashKey) && table[hashKey] != null)
            {
                if (table[hashKey].Count == 1)
                {
                    table[hashKey] = null;
                    return true;
                }

                foreach (Stock item in table[hashKey])
                {
                    if (item.Name == name)
                    {
                        table[hashKey].Remove(item);
                        break;
                    }
                }

                return true;
            }

            return false;
        }

        public bool TryGetStock(string name, out Stock stock)
        {
            stock = null;

            if (GenerateHash(name, out uint hashKey) && table[hashKey] != null)
            {
                foreach (Stock _stock in table[hashKey])
                {
                    if (_stock.Name == name)
                    {
                        stock = _stock;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool GenerateHash(string name, out uint key)
        {
            double hashKey = 0;

            for (int i = 0; i < name.Length; i++)
            {
                hashKey += table.Length * (name[i] * GoldenCut % 1);
            }

            hashKey = Math.Floor(hashKey / name.Length);

            key = (uint)hashKey;

            if (hashKey < 0 || hashKey > 1000)
            {
                return false;
            }

            return true;
        }

        // TODO: Fix Error, David?
        public void SaveToFile(string filename)
        {
            string basePath = "../../../UE1/resources/saved/";
            string fullFilePath = Path.Combine(basePath, "SAVED_" + filename + ".csv");

            using StreamWriter writer = new StreamWriter(fullFilePath);

            foreach (LinkedList<Stock> bucket in table)
            {
                if (bucket == null)
                    continue;

                foreach (Stock stock in bucket)
                {
                    writer.WriteLine($"{stock.Name};{stock.Sin};{stock.Symbol}");

                    // GIVES ERROR HERE
                    if (stock.Data == null)
                        continue;
                    foreach (StockData stockData in stock.Data)
                    {
                        writer.WriteLine(
                            $"{stockData.date};{stockData.open};{stockData.high};{stockData.low};{stockData.close};{stockData.adjClose};{stockData.volume}");
                    }
                }
            }

            Console.WriteLine($"Hash table saved to {fullFilePath}");
        }

        // TODO: Test
        public void LoadFromFile(string filename)
        {
            string basePath = "../../../UE1/resources/saved/";
            string fullFilePath = Path.Combine(basePath, filename);

            if (!File.Exists(fullFilePath))
            {
                Console.WriteLine($"File '{filename}' not found.");
                return;
            }

            using StreamReader reader = new StreamReader(fullFilePath);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(';');

                if (parts.Length < 3)
                {
                    Console.WriteLine($"Invalid line: {line}");
                    continue;
                }

                Stock newStock = new Stock(parts[0], parts[1], parts[2]);
                newStock.Data = new List<StockData>();

                // Read stock data
                while ((line = reader.ReadLine()) != null && !string.IsNullOrWhiteSpace(line))
                {
                    parts = line.Split(';');
                    if (parts.Length != 7)
                    {
                        Console.WriteLine($"Invalid stock data line: {line}");
                        continue;
                    }

                    StockData data = new StockData
                    {
                        date = DateTime.Parse(parts[0]),
                        open = double.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture),
                        high = double.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture),
                        low = double.Parse(parts[3], System.Globalization.CultureInfo.InvariantCulture),
                        close = double.Parse(parts[4], System.Globalization.CultureInfo.InvariantCulture),
                        adjClose = double.Parse(parts[5], System.Globalization.CultureInfo.InvariantCulture),
                        volume = uint.Parse(parts[6], System.Globalization.CultureInfo.InvariantCulture)
                    };

                    newStock.Data.Add(data);
                }

                AddStock(newStock);
            }

            Console.WriteLine($"Hash table loaded from {fullFilePath}");
        }
    }
}