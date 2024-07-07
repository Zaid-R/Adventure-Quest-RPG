using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureQuestRPG
{
    public abstract class Skill
    {
        private int _level = 1;
        public string Name { get; private set; }
        public int Level { get => _level; protected set => _level = value > 10 ? 10 : value; }
        public Skill(string name)
        {
            Name = name;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;
                hash = hash * 397 + Name.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object? obj)
        {
            var skill = obj as Skill;
            if (skill is null) return false;
            return this.Name.Equals(skill.Name, StringComparison.OrdinalIgnoreCase);
        }

        // TODO: I need a way to prevent level up after level 10 => I think this will be done in Adventure inside the loop so I have to prevent displaying the maxed skill to be leveled up
        public abstract void upgrade(Player player);

    }

    public class Healing : Skill
    {
        public int Health { get; private set; } = 10;
        public Healing() : base("healing")
        {
        }
        public Healing(int health,int level):base("healing")
        {
            Health = health;
            Level = level;
        }

        public override void upgrade( Player player)
        {
            //the price of upgrade is equal to the next level, ex. if healing is now has level 4 and I want to upgrade to level 5 then I have to pay 5 LP
            if (player.LP >= Level + 1)
            {
                Level++;
                player.LP -= Level;
                Health += 10;
                Console.WriteLine($"\t\tYour {Name} skill upgraded | {Name} level: {Level}\n");
            }
        }
    }

    public class SpecialAttack : Skill
    {
        public int AttackPower { get; private set; } = 10;
        public SpecialAttack() : base("special attack")
        {
        }
        public SpecialAttack(int attackPower, int level) : base("special attack")
        {
            AttackPower = attackPower;
            Level = level;
        }

        public override void upgrade( Player player)
        {
            if (player.LP >= Level + 1)
            {
                Level++;
                player.LP -= Level;
                AttackPower += 10;
                Console.WriteLine($"\t\tYour {Name} skill upgraded | {Name} level: {Level}\n");
            }
        }
    }
}
