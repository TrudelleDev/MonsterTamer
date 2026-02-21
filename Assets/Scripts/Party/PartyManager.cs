using System;
using System.Collections.Generic;
using System.Linq;
using MonsterTamer.Monsters;
using MonsterTamer.Party.Definitions;
using MonsterTamer.Utilities;

namespace MonsterTamer.Party
{
    /// <summary>
    ///  Manages a party of monsters
    /// </summary>
    internal sealed class PartyManager
    {
        internal const int MaxPartySize = 6;

        private readonly List<Monster> members = new();
        private List<Monster> originalPartyOrder;

        internal event Action PartyChanged;

        internal Monster SelectedMonster { get; private set; }
        internal Monster FirstUsableMonster => members.FirstOrDefault(m => m.Health.CurrentHealth > 0);
        internal int SelectedIndex => SelectedMonster != null ? members.IndexOf(SelectedMonster) : -1;
        internal IReadOnlyList<Monster> Members => members;
        internal bool HasAnyUsableMonster => members.Any(m => m.Health.CurrentHealth > 0);

        internal PartyManager(PartyDefinition partyDefinition)
        {
            Initialize(partyDefinition);
        }

        private void Initialize(PartyDefinition partyDefinition)
        {
            if (partyDefinition == null || partyDefinition.Members == null) return;

            foreach (var entry in partyDefinition.Members)
            {
                if (entry.MonsterDefinition != null)
                {
                    var monster = MonsterFactory.Create(entry.Level, entry.MonsterDefinition);
                    members.Add(monster);
                }
            }

            SelectedMonster = members.FirstOrDefault();
            PartyChanged?.Invoke();
        }

        internal void Add(Monster monster)
        {
            if (monster == null) return;

            if (members.Count >= MaxPartySize)
            {
                Log.Warning(nameof(PartyManager), $"Party full. Cannot add {monster.Definition.DisplayName}.");
                return;
            }

            members.Add(monster);
            SelectedMonster ??= monster;
            PartyChanged?.Invoke();
        }

        internal void CaptureOrder()
        {
            originalPartyOrder = new List<Monster>(members);
        }

        internal void RevertToCapturedOrder()
        {
            if (originalPartyOrder == null)
            {
                return;
            }

            members.Clear();
            members.AddRange(originalPartyOrder);

            if (SelectedMonster != null && !members.Contains(SelectedMonster))
            {
                SelectedMonster = members.FirstOrDefault();
            }

            PartyChanged?.Invoke();
        }

        internal bool Swap(int indexA, int indexB)
        {
            if (!IsValidIndex(indexA) || !IsValidIndex(indexB) || indexA == indexB)
            {
                return false;
            }

            (members[indexA], members[indexB]) = (members[indexB], members[indexA]);

            PartyChanged?.Invoke();
            return true;
        }

        internal void SetSelection(int index)
        {
            if (IsValidIndex(index))
            {
                SelectedMonster = members[index];
            }
        }

        internal void RestoreAllHealth()
        {
            foreach (var monster in members)
            {
                monster.Health.RestoreFullHealth();
            }

            PartyChanged?.Invoke();
        }

        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < members.Count;
        }
    }
}