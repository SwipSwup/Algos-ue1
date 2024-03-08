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
            uint hashKey = GenerateHash(stock.name);

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

        public void RemoveStock(Stock stock)
        {
            uint hashKey = GenerateHash(stock.name);

            if (table[hashKey] != null)
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
        
        public Stock[] GetStock(string name)
        {
            uint hashKey = GenerateHash(name);
            return table[hashKey] != null ? table[hashKey].ToArray() : null;
        }

        public uint GenerateHash(string name)
        {
            int tableSize = 1000;
            double hashKey = 0;

            for (int i = 0; i < name.Length; i++)
            {
                hashKey += tableSize * (name[i] * GoldenCut % 1);
            }

            hashKey /= name.Length;

            return (uint)Math.Floor(hashKey);
        }
    }
}