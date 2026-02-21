using System.Collections.Generic;
using MonsterTamer.Party.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Party.Definitions
{
    /// <summary>
    /// Defines a trainer's party with a fixed set of Monster.
    /// </summary>
    [CreateAssetMenu(menuName = "MonsterTamer/Party/Party Definition")]
    internal sealed class PartyDefinition : ScriptableObject
    {
        [SerializeField, Required] private List<PartyMemberEntry> members = new();

        internal IReadOnlyList<PartyMemberEntry> Members => members;
    }
}
