using System.Text.RegularExpressions;

namespace AdventureQuestRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // 1. let the player enter his name
            Console.Write("Welcome to Adventure Quest game\nEnter your name: ");
            string name;
            while (string.IsNullOrEmpty(name = Console.ReadLine().Trim()))
            {
                Console.Write("Your name can't be empty, please enter your name: ");
            }
            Player player = new Player(name);

            // 2. make the game running for multiple rounds, when the user enter esc exist the game
            string existKey = "";
            while (String.Compare(existKey, "escape", true) != 0)
            {
                // 3. let the player determine the difficulty of the game
                string message =
                        "please enter the number of the difficulty:\n" +
                        "[1] Easy\n" +
                        "[2] Medium\n" +
                        "[3] Hard\n";
                Console.WriteLine(message);
                string difficultyInput;
                Regex regex = new Regex("^[1-3]$");
                while (!regex.IsMatch(difficultyInput = ((char)Console.ReadKey().Key).ToString()))
                {
                    Console.Write("\nPlease enter a number between 1 and 3:");
                }
                Console.WriteLine();

                int difficulty = int.Parse(difficultyInput);
                MonsterLevel level = (MonsterLevel)difficulty;
  
                Monster monster = new Monster("Devil", level, player);
                BattleSystem.StartBattle(player, monster);
                player.Recover();

                Console.WriteLine("====================");
                Console.WriteLine("If you want to exit then press Esc, otherwise press any other key:");
                existKey = Console.ReadKey().Key.ToString();
            }

        }
    }
}
