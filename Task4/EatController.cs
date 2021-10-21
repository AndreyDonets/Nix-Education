using System;
using System.Linq;
using System.Threading;
using Task4.Models;

namespace Task4
{
    public class EatController
    {
        public void Eating(object obj)
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
    }
}
