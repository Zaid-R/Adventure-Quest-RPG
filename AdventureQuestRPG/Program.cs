namespace AdventureQuestRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player p = new Player("Zaid");
            Console.WriteLine($"Health: {p.Health}");
        }
    }
}
