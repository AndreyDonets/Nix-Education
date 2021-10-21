namespace Task4.Models
{
    public class Philosopher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsThink { get; set; }
        public Fork[] Forks { get; }
        public Philosopher(Fork fork1, Fork fork2) => Forks = new Fork[] { fork1, fork2 };
    }
}
