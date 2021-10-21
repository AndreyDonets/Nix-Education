using System;
using System.Collections.Generic;
using System.Threading;
using Task4.Models;

namespace Task4
{
    class Program
    {

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
            var eat = new EatController();
            Thread thread1 = new Thread(new ParameterizedThreadStart(eat.Eating));
            Thread thread2 = new Thread(new ParameterizedThreadStart(eat.Eating));
            Thread thread3 = new Thread(new ParameterizedThreadStart(eat.Eating));
            Thread thread4 = new Thread(new ParameterizedThreadStart(eat.Eating));
            Thread thread5 = new Thread(new ParameterizedThreadStart(eat.Eating));
            var threads = new List<Thread>() { thread1, thread2, thread3, thread4, thread5 };
            for (int i = 0; i < philosophers.Count; i++)
                threads[i].Start(philosophers[i]);
            Console.ReadLine();
        }


    }
}
