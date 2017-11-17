using System;

namespace PositionalFileBuilder.MapExample
{
    public class Account
    {
        public string Operation { get; set; }
        public DateTime OperationDate { get; set; }
        public string TransactionType { get; set; }
        public int Amount { get; set; }
        public int Discount { get; set; }
        public int Fee { get; set; }
    }    
}
