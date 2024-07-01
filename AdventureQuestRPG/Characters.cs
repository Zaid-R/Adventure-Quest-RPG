using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventureQuestRPG
{
    public enum MonsterLevel
    {
        Easy = 1,
        Medium = 2,
        Hard = 3
    }

    public abstract class Charachter
    {
        private int _health = 100;
        private readonly string _name;
        public int Health
        {
            get => _health;
            set => _health = value > 0 ? value : 0;
        }
        public String Name { get => _name; }
        public int AttackPower { get; protected set; }
        public int Defense { get; protected set; }

        public Charachter(string name)
        {
            _name = name;
        }
    }

    public class Player : Charachter
    {
        public int Level { get; private set; }
        public Player(string name) : base(name)
        {
            AttackPower = 10;
            Defense = 1;
            Level = 1;
        }

        public void Upgrade()
        {
            Level++;
            AttackPower += 2;
            Defense += 2;
        }

        public void Recover()
        {
            Health = 100;
        }
    }

    public abstract class AMonster : Charachter
    {
        protected AMonster(string name) : base(name)
        {
        }
    }

    public class Monster : AMonster
    {
        public MonsterLevel Level { get; protected set; }
        public Monster(string name, MonsterLevel level, Player player) : base(name)
        {
            Level = level;

            AttackPower = calculateMonstProperty(player.AttackPower,level);
            Defense = calculateMonstProperty(player.Defense,level);
        }
        public int calculateMonstProperty(int playerPropery, MonsterLevel level)
        {
            double percent = 0.5 + (0.2 * (int)level);
            return Convert.ToInt32(playerPropery * percent);
        }
    }

}

