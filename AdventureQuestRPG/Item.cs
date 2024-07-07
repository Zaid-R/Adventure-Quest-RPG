using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureQuestRPG
{
    public abstract class Item
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        protected Item(string name, string description)
        {
            Name = name;
            Description = description;
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
            var item = obj as Item;
            if(item is null) return false;
            return this.Name.Equals(item.Name,StringComparison.OrdinalIgnoreCase);
        }
    }

    public class Potion : Item
    {
        public int Health { get; } = 50;
        public Potion() : base("Potion", "health +50")
        {

        }
        public Potion(int health) : base("Potion", "health +50")
        {
            Health = health;
        }
    }
    public class Armor : Item
    {
        public int Defense { get; } = 5;
        public Armor() : base("Armor","defense +5")
        {
        }
        public Armor(int defense) : base("Armor", "defense +5")
        {
            Defense = defense;
        }
    }

    public class Weapon : Item
    {
        public int AttackPower { get; } = 5;
        public Weapon() : base("Weapon", "attack power +5")
        {
        }

        public Weapon(int attackPower) : base("Weapon", "attack power +5")
        {
            AttackPower = attackPower;
        }
    }
}
