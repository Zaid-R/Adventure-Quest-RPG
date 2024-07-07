using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventureQuestRPG
{
    public class BattleSystem
    {
        public static void Attack(IBattleStates attacker , IBattleStates target)
        {
            int damage = attacker.AttackPower - target.Defense;
            // after certain number of rounds the damage will be negative due to leveling up feature, so I should eliminate the case of negative damage 
            damage = Math.Max(1, damage);
            target.Health = target.Health - damage;
            Console.WriteLine($"{attacker.Name} attacks {target.Name} | Damage : {damage} | {target.Name}'s health: {target.Health}\n=============\n");
        }

        public static void StartBattle(Player player, Monster monster)
        {
            bool isPlayerTurn = true;
            player.useSkill();
            while (!player.IsDead && !monster.IsDead)
            {
                if (isPlayerTurn)
                {
                    Console.WriteLine("Player's turn ...");
                    Attack(player, monster);
                }
                else
                {
                    Console.WriteLine("Monster's turn...");
                    Attack(monster, player);
                }
                isPlayerTurn = !isPlayerTurn;
                //Thread.Sleep(1000);
            }
            player.disableSkill();
            if (player.IsDead)
            {
                Console.WriteLine($"{monster.Name} wins");
            }
            else 
            {
                Console.WriteLine($"{player.Name} wins");
                player.Upgrade();
            }
            Console.WriteLine();
        }
        public static string getKey(int from, int to, bool escIsAllowed = false)
        {
            string difficultyInput;
            Regex regex = new Regex($"^[{from}-{to}]$");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                difficultyInput = keyInfo.KeyChar.ToString();

                if (regex.IsMatch(difficultyInput))
                {
                    break;
                }

                if (escIsAllowed && keyInfo.Key == ConsoleKey.Escape)
                {
                    difficultyInput = "esc";
                    break;
                }

                Console.Write($"\nPlease enter a number between {from} and {to}");
                if (escIsAllowed)
                {
                    Console.Write($" or esc:");
                }
                Console.Write(": ");
            }
            return difficultyInput;
        }
        public static MonsterLevel chooseDificulty()
        {
            // 3. let the player determine the difficulty of the game
            string message =
                    "please enter the number of the difficulty:\n" +
                    "[1] Easy\n" +
                    "[2] Medium\n" +
                    "[3] Hard\n";
            Console.WriteLine(message);

            int difficulty = int.Parse(getKey(1,3));
            return (MonsterLevel)difficulty;
        }
    }
}
