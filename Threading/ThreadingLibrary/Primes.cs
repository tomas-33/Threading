using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadingLibrary
{
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
        public ulong Limit { get; set; }

        public event TenMilHandler TenMilEvent;
        public event HundredMilHandler HundredMilEvent;

        public delegate void TenMilHandler(TimeSpan time, EventArgs e);
        public delegate void HundredMilHandler(TimeSpan time, EventArgs e);

        public Primes()
        {
            this.ThreadCount = 1;
            this.Limit = 100000000;
        }

        public void RunParallel()
        {
            this._foundPrimes = new List<ulong>();
            this.Stop = false;
            this.ActualNumber = 2;
            this.TenMil = new TimeSpan();
            this.HundredMil = new TimeSpan();
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

                if (this.Limit != 0 && this.ActualNumber >= this.Limit)
                {
                    this.Stop = true;
                }

                if (this.ActualNumber == 10000000)
                {
                    this.TenMil = DateTime.Now - this.Start;
                    this.TenMilEvent(this.TenMil, null);
                }

                if (this.ActualNumber == 100000000)
                {
                    this.HundredMil = DateTime.Now - this.Start;
                    this.HundredMilEvent(this.HundredMil, null);
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
