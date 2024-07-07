namespace AdventureQuestRPG
{
    public class AdventureDto
    {
        public Player player;

        public int monstersCounter;

        public int locationsCounter;

        public List<string> locations;

        public List<Item> items;

        public List<string> monstersNames;

        public (int location, int monster) bossMonsterLocation;
        public AdventureDto(Player player, (int location, int monster) bossMonsterLocation, int monstersCounter, int locationsCounter, List<string> locations, List<Item> items, List<string> monstersNames)
        {
            this.bossMonsterLocation = bossMonsterLocation;
            this.player = player;
            this.monstersCounter = monstersCounter;
            this.locationsCounter = locationsCounter;
            this.locations = locations;
            this.items = items;
            this.monstersNames = monstersNames;
        }
    }
}
