using System.Collections.Generic;
using MonsterTamer.Monsters.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Monsters
{
    /// <summary>
    /// A weighted collection of Monster definitions used to determine wild encounter tables.
    /// </summary>
    [CreateAssetMenu(menuName = "MonsterTamer/Monster/Wild Monster Database")]
    internal sealed class WildMonsterDatabase : ScriptableObject
    {
        [SerializeField, Required] private List<WildMonsterEntry> entries = new();

        internal List<WildMonsterEntry> Entries => entries;
    }
}