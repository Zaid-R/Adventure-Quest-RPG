using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureQuestRPG
{
    public class BattleSystem
    {
        public static void Attack(Charachter attacker , Charachter target)
        {
            int damage = attacker.AttackPower - target.Defense;
            target.Health = target.Health - damage;
            Console.WriteLine($"{attacker.Name} attacks {target.Name} | Damage : {damage} | {target.Name}'s health: {target.Health}\n=============\n");
        }

        public static void StartBattle(Player player, Monster monster)
        {
            bool isPlayerTurn = true;

            while (player.Health != 0 && monster.Health != 0)
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
            if (player.Health == 0)
            {
                Console.WriteLine($"{monster.Name} wins");
            }
            else 
            {
                Console.WriteLine($"{player.Name} wins");
                player.Upgrade();
            }
        }
    }
}
