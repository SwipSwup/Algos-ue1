using System;
using System.Collections.Generic;
using System.Linq;

namespace UE1
{
    public class HashTable
    {
        private Dictionary<uint, LinkedList<Stock>> table = new Dictionary<uint, LinkedList<Stock>>();

        public void AddStock(Stock stock)
        {
            uint hashKey = GenerateHash(stock);

            if (table.TryGetValue(hashKey, out LinkedList<Stock> list))
            {
                list.AddFirst(stock);
            }
            else
            {
                LinkedList<Stock> newList = new LinkedList<Stock>();
                newList.AddFirst(stock);
                table.Add(hashKey, newList);
            }
        }

        public void RemoveStock(Stock stock)
        {
            uint hashKey = GenerateHash(stock);

            if (table.TryGetValue(hashKey, out LinkedList<Stock> list))
            {
                if (list.Count == 1)
                {
                    table.Remove(hashKey);
                }
                else
                {
                    foreach (Stock item in list)
                    {
                        //todo test if equals works or if custom solution is needed
                        if (item.Equals(stock))
                        {
                            list.Remove(item);
                            break;
                        }
                    }
                }
            }
        }
        
        public Stock[] GetStock(uint hashKey)
        {
            if (table.TryGetValue(hashKey, out LinkedList<Stock> list))
            {
                return list.ToArray();
            }

            return null;
        }

        public uint GenerateHash(Stock stock)
        {
            double constValue = 0.6180339887;
            int tableSize = 1000;
            double hashKey = 0;

            for (int i = 0; i < stock.name.Length; i++)
            {
                double hashValue = tableSize * ((stock.name[i] * constValue) % 1);
                hashKey =+ hashValue;
            }

            hashKey = hashKey / stock.name.Length;

            uint roundedHashKey = (uint)Math.Floor(hashKey);

            return roundedHashKey;
        }
    }
}