using System;
using System.Collections.Generic;

namespace UE1
{
    public struct Stock
    {
        //(Date,Open,High,Low,Close,Volume,Adj Close)
        public string name;
        public string sin;
        public string symbol;
        public List<StockData> data;
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