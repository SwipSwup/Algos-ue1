using System;

namespace UE1
{
    public struct Stock
    {
        //(Date,Open,High,Low,Close,Volume,Adj Close)

        public DateTime date;
        public double open;
        public double high;
        public double low;
        public double close;
        public double adjClose;
        public uint volume;
        
    }
}