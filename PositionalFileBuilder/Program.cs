using System;
using System.Diagnostics;
using System.IO;
using PositionalFileBuilder.MapExample;

namespace PositionalFileBuilder
{
    class Program
    {
        static void Main()
        {
            var rnd = new Random();

            var mapAccount = new MapAccount { IncludeNewLine = true };

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            for (var i = 1; i < 1000; i++)
            {

                mapAccount.SerializeToBuffer(new Account
                {
                    Operation = $"Operation {i}",
                    TransactionType = (rnd.Next(0, 5) > 2) ? "Credit" : "Debit",
                    Amount = rnd.Next(1000, 3000),
                    Discount = rnd.Next(10, 20),
                    OperationDate = DateTime.Now,
                    Fee = rnd.Next(1, 5)
                });
            }

            using (var outputFile = new StreamWriter(@"C:\Temp\output.txt", true))
                outputFile.Write(mapAccount.GetBuffer());

            //Console.WriteLine(mapAccount.GetBuffer());

            stopWatch.Stop();
            var ts = stopWatch.Elapsed;
            Console.WriteLine($"Time: {ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}");

            Console.ReadKey();
        }
    }
}
