using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Program
    {

        static SemaphoreSlim _semaSlim = new SemaphoreSlim(4);

        static void Main(string[] args)
        {



            #region Semaphore

            //Semaphore semaphore = new Semaphore(3, 7, "SEMAPHORE");
            //for (int i = 0; i < 10; i++)
            //{
            //    ThreadPool.QueueUserWorkItem(SomeMethod, semaphore);
            //}
            //Console.ReadLine();

            #endregion

            #region SemaphoreSlim



            for (int i = 0; i < 6; i++)
            {
                var name = $"Thread {i}";
                int seconds = 2 + 2 * i;
                var t = new Thread(() =>
                {
                    AccessDatabase(name, seconds);
                });
                t.Start();
            }


            #endregion



        }

        private static void AccessDatabase(string name, int seconds)
        {
            Console.WriteLine($"{name} waits for access");
            _semaSlim.Wait();
            Console.WriteLine($"{name} is working on database");
            Thread.Sleep(seconds * 1000);
            Console.WriteLine($"{name} completed its work");
            _semaSlim.Release();
        }

        private static void SomeMethod(object state)
        {
            var s = state as Semaphore;
            bool st = false;


            while (!st)
            {
                if (s.WaitOne(500))
                {
                    try
                    {
                        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} got the key.");

                        Thread.Sleep(2000);

                    }
                    finally
                    {
                        st = true;
                        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} returned the key.");
                        s.Release();
                    }
                }
                else
                {
                    s.Release(2);
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} we don't have enough keys. Please wait...");
                }
            }

        }

    }
}
