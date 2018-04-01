namespace ThreadingConsole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class Helper
    {
        private bool _complete = false;
        private object _locker = new object();

        public void Print1()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.Write(1);
            }
        }

        public void Write()
        {
            lock (this._locker)
            {
                if (!this._complete)
                {
                    Console.WriteLine("Complete");
                    this._complete = true;
                }
            }
        }
    }
}
