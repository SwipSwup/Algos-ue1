using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

namespace UE1
{
    public class Stock
    {
        //(Date,Open,High,Low,Close,Volume,Adj Close)
        public Stock(string name, string sin, string symbol)
        {
            Name = name;
            Sin = sin;
            Symbol = symbol;
        }

        public string Name { get; private set; }
        public string Sin { get; private set; }
        public string Symbol { get; private set; }

        public List<StockData> data;


        public void Plot(int width, int height)
        {
            List<StockData> tmp = new List<StockData>(data);
            tmp.Reverse();
            int[] normalized =
                NormalizeDataPoints(tmp, GetLowestPoint(tmp, width), GetHighestPoint(tmp, width), height);

            char[,] dataPoints = new char[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    dataPoints[y,x] = ' ';
                }
            }
            
            for (int i = 0; i < normalized.Length; i++)
            {
                if(height - normalized[i] >= height)
                    continue;
                Console.WriteLine(height - normalized[i]);
                dataPoints[height - normalized[i], i] = '*';
            }
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(dataPoints[y, x]);
                }

                Console.WriteLine();
            }
        }

        private int[] NormalizeDataPoints(List<StockData> data, double min, double max, int height)
        {
            int[] points = new int[30];
            for (int i = 0; i < 30; i++)
            {
                points[i] = (int)((data[i].adjClose - min) / (max - min) * height);
            }

            return points;
        }

        private double GetHighestPoint(List<StockData> data, int n)
        {
            double max = 0;

            for (int i = 0; i < n; i++)
            {
                if (data[i].adjClose > max)
                    max = data[i].adjClose;
            }

            return max;
        }

        private double GetLowestPoint(List<StockData> data, int n)
        {
            double min = Double.MaxValue;

            for (int i = 0; i < n; i++)
            {
                if (data[i].adjClose < min)
                    min = data[i].adjClose;
            }

            return min;
        }
    }

    public struct StockData
    {
        public DateTime date;
        public double open;
        public double high;
        public double low;
        public double close;
        public double adjClose;
        public uint volume;
    }
}