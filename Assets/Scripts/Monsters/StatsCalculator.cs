using MonsterTamer.Monsters.Definitions;
using MonsterTamer.Monsters.Enums;
using MonsterTamer.Monsters.Models;
using MonsterTamer.Natures;
using UnityEngine;

namespace MonsterTamer.Monsters
{
    /// <summary>
    /// Static utility for Monster stat math and IV generation. 
    /// </summary>
    internal static class StatsCalculator
    {
        private const float BaseMult = 0.01f;
        private const float EVFactor = 0.25f;
        private const int IVMin = 1, IVMax = 32;

        /// <summary>
        /// Calculates core stats based on base values, IVs, EVs, level, and nature. 
        /// </summary>
        internal static MonsterStats CalculateCoreStats(MonsterDefinition def, MonsterStats ivs, MonsterStats evs, int level, NatureDefinition nature)
        {
            int hp = Mathf.FloorToInt(BaseMult * (2 * def.BaseStats.HealthPoint + ivs.HealthPoint + (evs.HealthPoint * EVFactor)) * level) + level + 10;

            return new MonsterStats(
                hp,
                Stat(def.BaseStats.Attack, ivs.Attack, evs.Attack, level, nature.GetMultiplier(MonsterStat.Attack)),
                Stat(def.BaseStats.Defense, ivs.Defense, evs.Defense, level, nature.GetMultiplier(MonsterStat.Defense)),
                Stat(def.BaseStats.SpecialAttack, ivs.SpecialAttack, evs.SpecialAttack, level, nature.GetMultiplier(MonsterStat.SpecialAttack)),
                Stat(def.BaseStats.SpecialDefense, ivs.SpecialDefense, evs.SpecialDefense, level, nature.GetMultiplier(MonsterStat.SpecialDefense)),
                Stat(def.BaseStats.Speed, ivs.Speed, evs.Speed, level, nature.GetMultiplier(MonsterStat.Speed))
            );
        }

        /// <summary>
        ///  Generates a new stats container with random IVs (1-31). 
        /// </summary>
        internal static MonsterStats GenerateRandomIVs() => new(
            Random.Range(IVMin, IVMax), Random.Range(IVMin, IVMax),
            Random.Range(IVMin, IVMax), Random.Range(IVMin, IVMax),
            Random.Range(IVMin, IVMax), Random.Range(IVMin, IVMax)
        );

        private static int Stat(int @base, int iv, int ev, int lvl, float mult)
        {
            return Mathf.FloorToInt(((BaseMult * (2 * @base + iv + (ev * EVFactor)) * lvl) + 5) * mult);
        }   
    }
}