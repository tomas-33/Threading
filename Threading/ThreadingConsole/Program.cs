namespace ThreadingConsole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            int coreCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }

            Console.WriteLine("Number Of Cores: {0}", coreCount);
            Console.WriteLine($"Number Of Logical Processors: {Environment.ProcessorCount}\n");

            //var helper = new Helper();

            //Thread thread = new Thread(helper.Print1);
            //thread.Start();

            //for (int i = 0; i < 1000; i++)
            //{
            //    Console.Write(0);
            //}

            //thread = new Thread(helper.Write);
            //thread.Start();
            //helper.Write();

            Console.Write("Enter number of threads for calculation: ");
            var threadCount = Console.ReadLine();
            int threads;
            if(!int.TryParse(threadCount, out threads))
            {
                Console.WriteLine("Wrong input.");
                return;
            }

            var primes = new Primes();
            primes.ThreadCount = threads;
            Thread thread = new Thread(primes.RunParallel);
            thread.Start();

            Console.WriteLine("Actual position: ");
            Console.WriteLine("Found prime numbers: ");
            Console.WriteLine("Biggest is: ");
            Console.WriteLine("10 M in: ");
            Console.WriteLine("100 M in: ");

            while (!primes.Stop)
            {
                Console.SetCursorPosition(22, 4);
                Console.Write(primes.ActualNumber.ToString("### ### ### ### ### ### ###"));
                Console.SetCursorPosition(22, 5);
                Console.Write(primes.CountOfFoundPrimes.ToString("### ### ### ### ### ### ###"));
                Console.SetCursorPosition(22, 6);
                Console.Write(primes.BiggestFoundPrime.ToString("### ### ### ### ### ### ###"));
                Console.SetCursorPosition(22, 7);
                Console.Write(primes.TenMil);
                Console.SetCursorPosition(22, 8);
                Console.Write(primes.HundredMil);

                if (primes.HundredMil != default)
                {
                    primes.Stop = true;
                }

                Thread.Sleep(1000);
            }
        }
    }
}
