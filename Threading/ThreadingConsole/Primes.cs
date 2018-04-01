namespace ThreadingConsole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;

    public class Primes
    {
        private List<ulong> _foundPrimes;
        private object _locker = new object();
        private object _locker2 = new object();

        public bool Stop { get; set; }
        public ulong ActualNumber { get; private set; }
        public int ThreadCount { get; set; }
        public ulong CountOfFoundPrimes { get; private set; }
        public ulong BiggestFoundPrime { get; private set; }
        public DateTime Start { get; private set; }
        public TimeSpan TenMil { get; private set; }

        public TimeSpan HundredMil { get; private set; }

        public List<ulong> FoundPrimes { get { return this._foundPrimes; } private set { this._foundPrimes = value; } }

        public Primes()
        {
            this._foundPrimes = new List<ulong>();
            this.Stop = false;
            this.ActualNumber = 2;
            this.ThreadCount = 1;
            this.TenMil = new TimeSpan();
            this.HundredMil = new TimeSpan();
        }

        public void RunParallel()
        {
            this.Start = DateTime.Now;

            var threads = new Thread[this.ThreadCount];

            for (int i = 0; i < this.ThreadCount; i++)
            {
                threads[i] = new Thread(this.Run);
                threads[i].Start();
            }
        }

        private void Run()
        {
            do
            {
                ulong number;
                lock (this._locker)
                {
                    if (this.ActualNumber == ulong.MaxValue)
                    {
                        break;
                    }
                    number = this.ActualNumber;
                    this.ActualNumber++;
                }

                if (this.ActualNumber == 10000000)
                {
                    this.TenMil = DateTime.Now - this.Start;
                }

                if (this.ActualNumber == 100000000)
                {
                    this.HundredMil = DateTime.Now - this.Start;
                }

                if (this.IsPrime(number))
                {
                    //this._foundPrimes.Add(this.ActualNumber);
                    lock (this._locker2)
                    {
                        this.CountOfFoundPrimes++;
                        if (number > this.BiggestFoundPrime)
                        {
                            this.BiggestFoundPrime = number;
                        }
                    }
                }


            } while (!this.Stop);
        }

        private bool IsPrime(ulong number)
        {
            if (number < 2)
                return false;
            if (number == 2)
                return true;
            if (number % 2 == 0)
                return false;

            ulong max = (ulong)Math.Sqrt(number);

            for (ulong i = 3; i <= max; i += 2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
