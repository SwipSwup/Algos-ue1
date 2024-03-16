using System;
using System.Collections.Generic;

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
            tmp.RemoveRange(30, tmp.Count - 30);
            
            double max = double.MinValue;
            double min = double.MaxValue;
            foreach (StockData value in tmp)
            {
                if (value.adjClose > max)
                    max = value.adjClose;
                if (value.adjClose < min)
                    min = value.adjClose;
            }

            double valueRange = max - min;
            double yScale = valueRange != 0 ? height / valueRange : 1;
            double xScale = (double)width / tmp.Count;

            char[,] plot = new char[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    plot[y, x] = ' ';
                }
            }


            for (int i = 0; i < tmp.Count - 1; i++)
            {
                int x1 = (int)(i * xScale);
                int x2 = (int)((i + 1) * xScale);
                int y1 = (int)((max - tmp[i].adjClose) * yScale);
                int y2 = (int)((max - tmp[i + 1].adjClose) * yScale);
                DrawLine(plot, x1, y1, x2, y2, '*');
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(plot[y, x]);
                }

                Console.WriteLine();
            }
        }
        private void DrawLine(char[,] plot, int x0, int y0, int x1, int y1, char symbol)
        {
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (x0 != x1 || y0 != y1)
            {
                if (x0 >= 0 && y0 >= 0 && x0 < plot.GetLength(1) && y0 < plot.GetLength(0))
                    plot[y0, x0] = symbol;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
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