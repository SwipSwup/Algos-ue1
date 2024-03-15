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

            for (int i = 0; i < table.Length; i++)
            {
                table[i] = null;
            }
        }

        public void AddStock(Stock stock)
        {
            if (GenerateHash(stock.name, out uint hashKey))
            {
                if (table[hashKey] != null)
                {
                    table[hashKey].AddFirst(stock);
                }
                else
                {
                    LinkedList<Stock> newList = new LinkedList<Stock>();
                    newList.AddFirst(stock);
                    table[hashKey] = newList;
                }
            }
        }

        public void RemoveStock(Stock stock)
        {
            if (GenerateHash(stock.name, out uint hashKey) && table[hashKey] != null)
            {
                if (table[hashKey].Count == 1)
                {
                    table[hashKey] = null;
                }
                else
                {
                    foreach (Stock item in table[hashKey])
                    {
                        //todo test if equals works or if custom solution is needed
                        if (item.Equals(stock))
                        {
                            table[hashKey].Remove(item);
                            break;
                        }
                    }
                }
            }
        }

        //todo Funktion sollte mit namen und mit kürzel suchen können
        public bool TryGetStock(string name, out Stock? stock)
        {
            stock = null;

            if (GenerateHash(name, out uint hashKey))
            {
                foreach (Stock _stock in table[hashKey])
                {
                    if (_stock.name == name)
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

        public void DeleteStock()
        {
            Console.WriteLine("Enter Stockname to delete;");
            string stockName = Console.ReadLine();

            GenerateHash(stockName, out uint key);

            LinkedList<Stock> linkedList = table[key];

            if (linkedList != null)
            {
                LinkedListNode<Stock> currentNode = linkedList.First;
                while (currentNode != null)
                {
                    if (currentNode.Value.name == stockName)
                    {
                        linkedList.Remove(currentNode);
                        Console.WriteLine($"Stock '{stockName}' deleted successfully");
                        return;
                    }

                    currentNode = currentNode.Next;
                }

            }
            else
            {
                Console.WriteLine("No Stocks Found ");
            }



                


        }

        // TODO: Fix Error, David?
        public void SaveToFile(string filename)
        {
            string basePath = "../../../UE1/resources/saved/";
            string fullFilePath = Path.Combine(basePath, "SAVED_" + filename + ".csv");

            using (StreamWriter writer = new StreamWriter(fullFilePath))
            {
                foreach (var bucket in table)
                {
                    if (bucket != null)
                    {
                        foreach (var stock in bucket)
                        {
                            writer.WriteLine($"{stock.name},{stock.sin},{stock.symbol}");

                            // GIVES ERROR HERE
                            foreach (var stockData in stock.data)
                            {
                                writer.WriteLine(
                                    $"{stockData.date},{stockData.open},{stockData.high},{stockData.low},{stockData.close},{stockData.adjClose},{stockData.volume}");
                            }
                        }
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

            using (StreamReader reader = new StreamReader(fullFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');

                    if (parts.Length < 3)
                    {
                        Console.WriteLine($"Invalid line: {line}");
                        continue;
                    }

                    Stock newStock = new Stock
                    {
                        name = parts[0],
                        sin = parts[1],
                        symbol = parts[2],
                        data = new List<StockData>()
                    };

                    // Read stock data
                    while ((line = reader.ReadLine()) != null && !string.IsNullOrWhiteSpace(line))
                    {
                        parts = line.Split(',');
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

                        newStock.data.Add(data);
                    }

                    AddStock(newStock);
                }
            }

            Console.WriteLine($"Hash table loaded from {fullFilePath}");
        }
    }
}