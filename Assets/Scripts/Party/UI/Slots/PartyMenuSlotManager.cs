using System.Collections.Generic;
using MonsterTamer.Characters.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Party.UI.Slots
{
    /// <summary>
    /// Synchronizes the Party UI slots with the player's current monster party.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class PartyMenuSlotManager : MonoBehaviour
    {
        [SerializeField, Required] private Character player;

        private List<PartyMenuSlot> slots;

        private void Start() => Refresh();

        internal void Refresh()
        {
            slots ??= new List<PartyMenuSlot>(GetComponentsInChildren<PartyMenuSlot>());

            UnbindAll();

            for (int i = 0; i < slots.Count; i++)
            {
                if (i < player.Party.Members.Count)
                {
                    slots[i].Bind(player.Party.Members[i]);
                    slots[i].SetSlotIndex(i);
                }
                else
                {
                    slots[i].Unbind();
                }
            }
        }

        private void UnbindAll()
        {
            foreach (PartyMenuSlot slot in slots)
            {
                slot.Unbind();
            }
        }
    }
}
