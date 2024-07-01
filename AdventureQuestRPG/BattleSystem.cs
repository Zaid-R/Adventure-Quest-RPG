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
            target.Health-=damage;
            Console.WriteLine($"{attacker.Name} attacks {target.Name} | Damage : {damage} | Health: {target.Health}");
        }

        public static void StartBattle(Player player, Monster monster)
        {
            bool isPlayerTurn = true;

            while (player.Health != 0 && monster.Health != 0)
            {
                if (isPlayerTurn)
                {
                    Console.WriteLine("player's turn");
                    Attack(player, monster);
                }
                else
                {
                    Console.WriteLine("monster's turn");
                    Attack(monster, player);
                }
                isPlayerTurn = !isPlayerTurn;
            }
            if (player.Health == 0)
            {
                Console.WriteLine("player wins");
            }
            else 
            {
                Console.WriteLine("monster wins");
            }
        }
    }
}
