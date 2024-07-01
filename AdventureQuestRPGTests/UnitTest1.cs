using System;
using Xunit;
using AdventureQuestRPG;

namespace AdventureQuestRPGTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test_Enemy_Health()
        {
            // Act
            Player player = new Player("Ahmad");
            Monster monster = new Monster("Devil", MonsterLevel.Easy, player);
            BattleSystem.Attack(player, monster);

            // Assert
            Assert.Equal(91, monster.Health);
        }

        [Fact]
        public void Test_Player_Health()
        {
            // Act
            Player player = new Player("Zaid");
            MonsterLevel level = MonsterLevel.Easy;
            Monster monster = new Monster("Devil", level, player);
            BattleSystem.Attack(monster, player);

            int expectedResult = 100 - (monster.calculateMonstProperty(player.AttackPower, level) - player.Defense);
            // Assert
            Assert.Equal(expectedResult, player.Health);
        }

        [Fact]
        public void Test_Health_Validation()
        {
            // Act
            Player player = new Player("Ahmad");
            Monster monster = new Monster("Devil", MonsterLevel.Easy, player);
            BattleSystem.StartBattle(player,monster);

            // Assert
            Assert.Equal(0, monster.Health);
        }
    }
}