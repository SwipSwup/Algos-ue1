using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}