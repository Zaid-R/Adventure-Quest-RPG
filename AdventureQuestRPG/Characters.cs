
namespace AdventureQuestRPG
{
    public enum MonsterLevel
    {
        Easy = 1,
        Medium,
        Hard
    }

    public abstract class Charachter : IBattleStates
    {
        private int _health = 100;
        private readonly string _name;
        public bool IsDead => Health == 0;
        public int Health
        {
            get => _health;
            set
            {
                if (value >= 0)
                {
                    _health = value > 100 ? 100 : value;
                }
                else
                {
                    _health = 0;
                }
            }
        }
        public string Name { get => _name; }
        public int AttackPower { get;  protected set; }

        public int Defense { get; protected set; }


        public Charachter(string name)
        {
            _name = name;
        }
    }

    public class Player : Charachter
    {
        public readonly Inventory inventory = new();
        public int LP { get; set; }
        public bool LPCanUlock => LP > 0 && skills.Count !=2;
        public bool LPCanUpgrade => skills.Count > 0 && LP >=  skills.Select(skill => skill.Level + 1).Min();
        public string CurrentLocation { get; set; }
        public readonly List<Skill> skills = new();
        public Skill? skillInUse;
        public int Level { get; private set; } = 1;
        public Player(string name) : base(name)
        {
            AttackPower = 10;
            Defense = 1;
        }

        public Player(string name,int attackPower,int health,int defense,Inventory inventory, int lP, string currentLocation, List<Skill> skills, int level):base(name)
        {
            this.inventory = inventory;
            LP = lP;
            CurrentLocation = currentLocation;
            this.skills = new(skills);
            Level = level;
            Health  = health;
            AttackPower = attackPower;
            Defense = defense;
        }

        public void use(Potion potion)
        {
            Health += potion.Health;
            Console.WriteLine($"\t\tYou used a potion | health: {Health}");
        }

        public void equip(Weapon item)
        {
            AttackPower += item.AttackPower;
            Console.WriteLine($"\t\tYou equiped a weapon| attack power: {AttackPower}");
        }
        public void equip(Armor armor) { 
            Defense += armor.Defense;
            Console.WriteLine($"\t\tYou equiped an armor| defense: {Defense}");
        }

        public void Upgrade()
        {
            Health += (Level *10);
            Level++;
            AttackPower += 2;
            Defense += 2;
            LP++;
            Console.WriteLine($"Your level is {Level} | health: {Health} | attackpower: {AttackPower}  | defense: {Defense} | LP: {LP}\n");
        }

        public void useSkill()
        {
            if (skillInUse is not null)
            {
                if (skillInUse is Healing healing)
                {
                    Health += healing.Health;
                }
                else if (skillInUse is SpecialAttack specialAttack)
                {
                    AttackPower += specialAttack.AttackPower;
                }
            }
        }

        public void disableSkill()
        {
            if (skillInUse is not null)
            {
                if (skillInUse is SpecialAttack specialAttack)
                {
                    AttackPower -= specialAttack.AttackPower;
                }
                skillInUse = null;
            }
        }

        public void Recover()
        {
            Health = 100;
            Console.WriteLine($"You killed all the monsters in the {CurrentLocation}, you deserve a recover | your health: 100\n");
        }

    }

    // TODO: See when you'll create the monsters so they have the right stats, since the level and stats of the player are changing
    public class Monster : Charachter
    {
        public MonsterLevel Level { get; protected set; }
        public Monster(string name, MonsterLevel level, Player player) : base(name)
        {
            Level = level;

            AttackPower = calculateMonstProperty(player.AttackPower, level);

            Defense = calculateMonstProperty(player.Defense, level);
        }
        public int calculateMonstProperty(int playerPropery, MonsterLevel level)
        {
            double percent = 0.3 + (0.25 * (int)level);
            return Convert.ToInt32(Math.Ceiling(playerPropery * percent));
        }
    }

    public class BossMonster : Monster
    {
        public BossMonster( Player player) : base("Boss", MonsterLevel.Hard, player)
        {
        }
    }
}

