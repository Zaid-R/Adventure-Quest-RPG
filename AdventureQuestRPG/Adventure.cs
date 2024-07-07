using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventureQuestRPG
{
    // TODO: add comments to your code in rational way
    public class Adventure
    {
        private static string key = string.Empty;
        private static string filePath = Path.Combine(Environment.CurrentDirectory, "game.json");
        private static string message = string.Empty;
        private static Skill healing = new Healing();
        private static Skill specialAttack = new SpecialAttack();
        private static Player player;
        private static int monstersCounter;
        private static int locationsCounter;
        // TODO: maybe you can use Factory design pattern & sigleton ==> Ask GPT-4o to refactor the code using sigleton and factory and ask if the code need these patterns
        private static List<string> locations = new List<string> { "forest", "cave", "town" };

        private static List<Item> items = new() { new Potion(), new Weapon(), new Armor() };

        //savedMonstersNames should contains (savedLocations.Count * savedLocations.Count - 1) names;
        //because I want to have savedLocations.Count monsters in each location
        //and I should make one chance for Boss monster to appear randomly

        private static List<string> monstersNames = new List<string> {  "Bahamut",
                                                                        "Behemoth",
                                                                        "Dragon",
                                                                        "Cthulhu",
                                                                        "Lich",
                                                                        "Slime",
                                                                        "Orc",
                                                                        "Goblin"};

        private static Dictionary<int, string> monsterArrangement = new Dictionary<int, string> {
            { 1,"first"},
            { 2,"second"},
            { 3,"third"},
        };
        private static string getName()
        {
            string name;
            while (string.IsNullOrEmpty(name = Console.ReadLine().Trim()))
            {
                Console.Write("Your name can't be empty, please enter your name: ");
            }
            return name;
        }
        private enum ActionName
        {
            Initial,
            Attack,
            EndTheGame,
            DisplayInventory,
            UseSKill,
            SkillTree

        };
        private enum SkillAction
        {
            UnlockHealing,
            UnlockSpecialAttack,
            UpgradeHealing,
            UpgradeSpecialAttack,
        };
        public static void startGame()
        {
            Console.WriteLine("Welcome to Adventure Quest RPG game");
            bool isNewGame = true;
            string jsonData = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(jsonData))
            {

                message = "[1] new game\n" +
                          "[2] load the previous game\n";
                displayOptions(() => BattleSystem.getKey(1, 2));
                switch (key)
                {
                    case "2":
                        isNewGame = false;
                        loadGame(jsonData);
                        break;
                }
            }
            if (key == "1" || string.IsNullOrEmpty(jsonData))
            {
                Console.Write("Enter your name: ");
                player = new Player(getName());
            }


            Random random = new();

            (int, int) bossMonsterLocation = (random.Next(locations.Count), random.Next(locations.Count));

            int locationsLength = 3;

            ActionName playerAction = ActionName.Initial;

            while (locationsCounter < locationsLength && player.Health > 0)
            {
                bool canDiscoverNewLocation = isNewGame || !isNewGame && monstersCounter == 0 && locationsCounter + locations.Count == locationsLength;
                if (canDiscoverNewLocation)
                {
                    message = "[1] discover a new location\n" +
                                                    "[2] exit\n";

                    displayOptions(() => BattleSystem.getKey(1, 2));
                }
                else
                {
                    key = "1";
                }

                // Discover new location
                if (key == "1")
                {
                    
                    if (canDiscoverNewLocation)
                    {
                        int index = random.Next(locations.Count);
                        player.CurrentLocation = locations[index];
                        locations.RemoveAt(index);
                    }
                    int numberOfMonsters = locationsLength - monstersCounter;
                    Console.WriteLine($"You are in {player.CurrentLocation} now, you are facing {numberOfMonsters} monster{(numberOfMonsters==1?"":"s")}\n");
                    

                    while (monstersCounter < locationsLength)
                    {
                        Dictionary<int, ActionName> actions = new();
                        discoverAvailableActions(monstersCounter, ref actions);

                        playerAction = actions[int.Parse(key)];
                        switch (playerAction)
                        {
                            case ActionName.Attack:
                                attackMonster(player, bossMonsterLocation, locationsCounter, ref monstersCounter);
                                break;

                            case ActionName.UseSKill:
                                useSkill(bossMonsterLocation, locationsCounter, ref monstersCounter);
                                break;

                            case ActionName.SkillTree:
                                useSkillTree();
                                break;

                            case ActionName.DisplayInventory:
                                useInventory();
                                break;
                        }
                        //Console.Clear();
                        if (playerAction == ActionName.EndTheGame || player.IsDead)
                        {
                            if (player.IsDead)
                            {
                                //File.WriteAllText(filePath, "");
                            }
                            else
                            {
                                AdventureDto game = new AdventureDto(player, monstersCounter, locationsCounter, locations, items, monstersNames);
                                string jsonString = JsonConvert.SerializeObject(game);
                                File.WriteAllText(filePath, jsonString);
                            }
                            break;
                        }
                    }
                    if (player.Health > 0 && playerAction != ActionName.EndTheGame)
                    {
                        monstersCounter = 0;
                        player.Recover();
                    }
                }
                if (playerAction == ActionName.EndTheGame)
                {
                    break;
                }
                locationsCounter++;
            }

            if (!player.IsDead && monstersNames.Count == 0)
            {
                //File.WriteAllText(filePath, "");
                Console.WriteLine($"Congratz {player.Name} you won the game !!!!");
            }
            else if (player.IsDead)
            {
                Console.WriteLine("Game over!");
            }
        }

        private static void loadGame(string jsonData)
        {
            //Dictionary<string, object> game = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData)!;

            JsonNode game = JsonNode.Parse(jsonData)!;
            JsonNode playerNode = game["player"]!;
            //1. Initialize Skills
            JsonArray jsonSkills = playerNode["skills"].AsArray();
            List<Skill> savedSkills = new();
            foreach (JsonNode skill in jsonSkills)
            {

                int level = int.Parse(skill["Level"]!.ToString());
                if (skill["Name"]!.ToString() == "healing")
                {
                    savedSkills.Add(new Healing(int.Parse(skill["Health"]!.ToString()), level));
                }
                else
                {
                    savedSkills.Add(new SpecialAttack(int.Parse(skill["AttackPower"]!.ToString()), level));
                }
            }
            //2. Initialize Items
            JsonArray jsonItems;
            List<Item> savedItems;
            getSavedItems(playerNode["inventory"]["items"], out savedItems);
            //3. Initailize Inventory
            Inventory playerInventory = new Inventory(savedItems);
            //4. Initailize player
            string playerName = playerNode["Name"]!.ToString();
            int playerLP = int.Parse(playerNode["LP"]!.ToString());
            string playerCurrentLocation = playerNode["CurrentLocation"]!.ToString();
            int playerLevel = int.Parse(playerNode["Level"]!.ToString());
            int playerHealth = int.Parse(playerNode["Health"]!.ToString());
            int playerAttackPower = int.Parse(playerNode["AttackPower"]!.ToString());
            int playerDefense = int.Parse(playerNode["Defense"]!.ToString());

            player = new Player(playerName, playerAttackPower, playerHealth, playerDefense, playerInventory, playerLP, playerCurrentLocation, savedSkills, playerLevel);

            int savedMonstersCounter = int.Parse(game["monstersCounter"]!.ToString());
            int savedLocationsCounter = int.Parse(game["locationsCounter"]!.ToString());

            // Initialize savedLocations
            JsonArray jsonLocations = game["locations"]!.AsArray();
            List<string> savedLocations = jsonLocations.Select(node => node.ToString()).ToList();

            // Initialize items
            getSavedItems(game["items"], out savedItems);

            // Initialize savedMonstersNames
            JsonArray jsonMonstersNames = game["monstersNames"]!.AsArray();
            List<string> savedMonstersNames = jsonMonstersNames.Select(node => node.ToString()).ToList();

            // Assign values to the fields of adventure
            items = new(savedItems);
            monstersCounter = savedMonstersCounter;
            locationsCounter = savedLocationsCounter;
            locations = new(savedLocations);
            monstersNames = new(savedMonstersNames);


        }

        private static void getSavedItems(JsonNode items, out List<Item> savedItems)
        {
            JsonArray jsonItems = items.AsArray();
            savedItems = new();
            foreach (JsonNode item in jsonItems)
            {
                string itemName = item["Name"]!.ToString();
                if (itemName == "Potion")
                {
                    savedItems.Add(new Potion(int.Parse(item["Health"]!.ToString())));
                }
                else if (itemName == "Armor")
                {
                    savedItems.Add(new Armor(int.Parse(item["Defense"]!.ToString())));
                }
                else
                {
                    savedItems.Add(new Weapon(int.Parse(item["AttackPower"]!.ToString())));
                }
            }
        }

        private static void discoverAvailableActions(int monstersCounter, ref Dictionary<int, ActionName> actions)
        {
            actions = new()
                        {
                            { 1, ActionName.Attack },
                            { 2, ActionName.EndTheGame}
                        };
            // end the game
            // or attackMonster the first monster
            int to = 2;
            message = $"\t[1] attack the {monsterArrangement[monstersCounter + 1]} monster\n" +
                       "\t[2] end the game\n" +
                       "";

            if (!player.inventory.isEmpty())
            {
                to++;
                message += $"\t[{to}] display inventory\n";
                actions.Add(to, ActionName.DisplayInventory);
            }

            if (player.skills.Count > 0)
            {
                to++;
                message += $"\t[{to}] attack with a skill\n";
                actions.Add(to, ActionName.UseSKill);
            }

            if (player.LPCanUlock || player.LPCanUpgrade)
            {
                to++;
                message += $"\t[{to}] Skill tree\n";
                actions.Add(to, ActionName.SkillTree);
            }
            displayOptions(() => BattleSystem.getKey(1, to), 1);
        }

        private static void useInventory()
        {
            message = "\t\tEnter the number of the item you want to use\n"
                                                + "\t\tpress esc to exit inventory:";

            while (player.inventory.items.Count > 0 && key != "esc" && !player.inventory.isEmpty())
            {
                player.inventory.display();
                Console.Write(message);
                key = BattleSystem.getKey(1, player.inventory.items.Count, true);
                Console.WriteLine(key);
                if (key != "esc")
                {
                    var item = player.inventory.items[int.Parse(key) - 1];
                    switch (item.GetType().Name)
                    {
                        case nameof(Potion): player.use((Potion)item); break;
                        case nameof(Weapon): player.equip((Weapon)item); break;
                        case nameof(Armor): player.equip((Armor)item); break;
                    }
                    player.inventory.remove(item);
                }
                Console.WriteLine();
            }
        }

        private static void useSkillTree()
        {
            Dictionary<int, SkillAction> skillActions = new();

            getAvailableActionOnSkills(skillActions);
            displayOptions(() => BattleSystem.getKey(1, skillActions.Count, true), 2);
            if (key != "esc")
            {
                var skillActionOfPlayer = skillActions[int.Parse(key)];
                switch (skillActionOfPlayer)
                {
                    case SkillAction.UnlockSpecialAttack:
                        player.skills.Add(specialAttack);
                        player.LP--;
                        break;
                    case SkillAction.UpgradeSpecialAttack:
                        player.skills.Single(skill => skill.Name == specialAttack.Name).upgrade(player);
                        break;
                    case SkillAction.UnlockHealing:
                        player.skills.Add(healing);
                        player.LP--;
                        break;
                    case SkillAction.UpgradeHealing:
                        player.skills.Single(skill => skill.Name == healing.Name).upgrade(player);
                        break;
                }
            }
        }

        private static void useSkill((int, int) bossMonsterLocation, int locatoinsCounter, ref int monstersCounter)
        {
            message = $"\t\t[1] Use {player.skills[0].Name} then attack\n";
            if (player.skills.Count == 2)
            {
                message += $"\t\t[2] Use {player.skills[1].Name} then attack\n";
            }

            displayOptions(() => BattleSystem.getKey(1, player.skills.Count), 2);
            int skillIndex = int.Parse(key);
            player.skillInUse = player.skills[skillIndex - 1];
            attackMonster(player, bossMonsterLocation, locatoinsCounter, ref monstersCounter);
            player.skillInUse = null;
        }

        private static void displayOptions(Func<string> getKey, int tabs = 0)
        {
            string initialSpace = string.Join("", Enumerable.Repeat("\t", tabs).ToArray());
            Console.Write($"{message}{initialSpace}Choose an action by pressing its number:");
            key = getKey();
            Console.WriteLine(key + "\n");
        }

        private static void getAvailableActionOnSkills(Dictionary<int, SkillAction> skillActions)
        {
            message = "";
            //TODO: use loop instead of repeating the code
            if (!player.skills.Contains(healing))
            {
                skillActions.Add(1, SkillAction.UnlockHealing);
                message = "\t\t[1] healing (next level: 1)\n";
            }
            if (!player.skills.Contains(specialAttack))
            {
                skillActions.Add(skillActions.Count + 1, SkillAction.UnlockSpecialAttack);
                message += $"\t\t[{skillActions.Count}] special attack (next level: 1)\n";
            }

            if (player.LPCanUpgrade)
            {
                //TODO: use loop instead of repeating the code
                if (player.skills.Contains(healing))
                {
                    var healingSkillOfPlayer = player.skills.First(skill => skill.Name == healing.Name);
                    if (player.LP >= healingSkillOfPlayer.Level + 1)
                    {
                        skillActions.Add(skillActions.Count + 1, SkillAction.UpgradeHealing);
                        message += $"\t\t[{skillActions.Count}] healing (next level: {healingSkillOfPlayer.Level + 1})\n";

                    }
                }
                if (player.skills.Contains(specialAttack))
                {
                    var specialAttackSkillOfPlayer = player.skills.First(skill => skill.Name == specialAttack.Name);
                    if (player.LP >= specialAttackSkillOfPlayer.Level + 1)
                    {
                        skillActions.Add(skillActions.Count + 1, SkillAction.UnlockSpecialAttack);
                        message += $"\t\t[{skillActions.Count}] special attack (next level: {specialAttackSkillOfPlayer.Level + 1})\n";
                    }
                }
            }
            message += "\t\tpress esc to exit skill tree\n";
        }

        public static void attackMonster(Player player, (int, int) bossMonsterLocation, int locatoinsCounter, ref int monstersCounter)
        {
            Random random = new Random();

            Monster monster;
            if (bossMonsterLocation == (locatoinsCounter, monstersCounter))
            {
                monster = new BossMonster(player);
            }
            else
            {
                int index = random.Next(monstersNames.Count);
                MonsterLevel level = locations.Count == 2 ? MonsterLevel.Easy : MonsterLevel.Medium;
                monster = new Monster(monstersNames[index], level, player);
                monstersNames.RemoveAt(index);
            }
            BattleSystem.StartBattle(player, monster);
            if (player.Health > 0)
            {
                monstersCounter++;
                int itemIndex = random.Next(items.Count * 4);
                if (itemIndex < items.Count)
                {
                    player.inventory.add(items[itemIndex]);
                    Console.WriteLine($"You got new item ===> {items[itemIndex].Name}\n");
                    //The weapon and armor can be equiped only one time, so if the player got one of them I'll delete it, but the potion can be used many times so I don't delete it
                    if (items[itemIndex].GetType().Name != nameof(Potion))
                    {
                        items.RemoveAt(itemIndex);
                    }
                }
            }
        }
    }
}