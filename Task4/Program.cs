using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Task4
{
    class Program
    {
        private static void Action(object obj)
        {
            if (obj is Philosopher)
            {
                var philosopher = obj as Philosopher;
                Thread.Sleep(new Random().Next(0, 500));
                while (philosopher.Forks.Any(x => x.IsBusy == true))
                {
                    Thread.Sleep(200);
                    Console.WriteLine($"{philosopher.Name} waiting fork!");
                }
                foreach (var fork in philosopher.Forks)
                    fork.IsBusy = true;
                Thread.Sleep(1000);
                Console.WriteLine($"{philosopher.Name} ate!");
                foreach (var fork in philosopher.Forks)
                    fork.IsBusy = false;
            }
        }
        static void Main(string[] args)
        {
            Fork fork1 = new Fork() { Id = 0 };
            Fork fork2 = new Fork() { Id = 1 };
            Fork fork3 = new Fork() { Id = 2 };
            Fork fork4 = new Fork() { Id = 3 };
            Fork fork5 = new Fork() { Id = 4 };
            Philosopher philosopher1 = new Philosopher(fork1, fork2) { Id = 0, Name = "First" };
            Philosopher philosopher2 = new Philosopher(fork2, fork3) { Id = 1, Name = "Second" };
            Philosopher philosopher3 = new Philosopher(fork3, fork4) { Id = 2, Name = "Third" };
            Philosopher philosopher4 = new Philosopher(fork4, fork5) { Id = 3, Name = "Fourth" };
            Philosopher philosopher5 = new Philosopher(fork5, fork1) { Id = 4, Name = "Fifth" };
            var philosophers = new List<Philosopher>() { philosopher1, philosopher2, philosopher3, philosopher4, philosopher5 };
            Thread thread1 = new Thread(new ParameterizedThreadStart(Action));
            Thread thread2 = new Thread(new ParameterizedThreadStart(Action));
            Thread thread3 = new Thread(new ParameterizedThreadStart(Action));
            Thread thread4 = new Thread(new ParameterizedThreadStart(Action));
            Thread thread5 = new Thread(new ParameterizedThreadStart(Action));
            var threads = new List<Thread>() { thread1, thread2, thread3, thread4, thread5 };
            for (int i = 0; i < philosophers.Count; i++)
                threads[i].Start(philosophers[i]);
            Console.ReadLine();
        }


    }
}
