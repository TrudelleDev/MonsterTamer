using System.Linq;
using MonsterTamer.Monsters.Components;
using MonsterTamer.Monsters.Definitions;
using MonsterTamer.Moves;
using MonsterTamer.Moves.Definitions;
using MonsterTamer.Natures;

namespace MonsterTamer.Monsters
{
    /// <summary>
    /// A runtime instance of a Monster, managing its unique state, components, and progression.
    /// </summary>
    internal class Monster
    {
        internal ExperienceComponent Experience { get; }
        internal HealthComponent Health { get; }
        internal StatsComponent Stats { get; }
        internal MetadataComponent Meta { get; }
        internal MovesComponent Moves { get; }
        internal MonsterDefinition Definition { get; }
        internal Nature Nature { get; }

        internal bool IsFainted => Health.CurrentHealth <= 0;

        internal Monster(int level, MonsterDefinition definition, NatureDefinition natureDefinition, MoveDefinition[] moveDefinitions)
        {
            Definition = definition;

            Nature = new Nature(natureDefinition);
            Experience = new ExperienceComponent(level);
            Stats = new StatsComponent(this);
            Health = new HealthComponent(Stats.Core.HealthPoint);
            Meta = new MetadataComponent();
            Moves = new MovesComponent(moveDefinitions);
        }

        internal Move GetRandomMove()
        {
            if (Moves.MoveSet.Length == 0) return null;

            return Moves.MoveSet[UnityEngine.Random.Range(0, Moves.ActiveMoves.Count())];
        }
    }
}

