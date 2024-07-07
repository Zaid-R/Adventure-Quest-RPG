using System;
using System.Xml.Linq;


namespace AdventureQuestRPG
{
    public interface IBattleStates
    {
        int Health { get; set; }
        string Name { get; }
        int AttackPower { get; }
        int Defense { get; }
    }

}
