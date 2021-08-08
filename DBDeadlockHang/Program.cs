using System;
using System.Threading;

namespace DBDeadlockHang
{
    class Program
    {
        private static DBWrapper1 db1;
        private static DBWrapper2 db2;
        static void Main(string[] args)
        {
            db1 = new DBWrapper1("db1");
            db2 = new DBWrapper2("db2");
            new Thread(new ThreadStart(ThreadProc)).Start();
            Thread.Sleep(100);
            lock (db2)
            {
                var threadName = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine(threadName+":updating DB2");
                Thread.Sleep(1000);
                lock (db1)
                {
                    Console.WriteLine(threadName + ":updating db1");
                }
            }
        }
        static void ThreadProc()
        {
            Console.WriteLine("Start worker thread");
            lock (db1)
            {
                var threadName = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine(threadName + ":updating db1");
                Thread.Sleep(1000);
                lock (db2)
                {
                    Console.WriteLine(threadName + ":updateing db2");
                }
            }
        }
    }

    public class DBWrapper1
    {
        public DBWrapper1(string dbName)
        {

        }
    }
    public class DBWrapper2
    {
        public DBWrapper2(string dbName)
        {

        }
    }
}
