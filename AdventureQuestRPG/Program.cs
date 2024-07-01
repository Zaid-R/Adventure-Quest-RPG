namespace AdventureQuestRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player("Zaid");
            Monster monster = new Monster("yazzen", MonsterLevel.Easy , player);
            BattleSystem.StartBattle(player, monster);
        }
    }
}
