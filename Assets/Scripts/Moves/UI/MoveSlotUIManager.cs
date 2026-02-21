using System.Collections.Generic;
using MonsterTamer.Monsters;
using MonsterTamer.Shared.UI.Core;
using UnityEngine;

namespace MonsterTamer.Moves.UI
{
    /// <summary>
    /// Manages and binds a Monster's moves to the summary screen UI slots.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class MoveSlotUIManager : MonoBehaviour
    {
        internal void Bind(Monster monster)
        {
            if (monster?.Moves == null)
            {
                return;
            }

            BindMoves(monster.Moves.MoveSet);
        }

        internal void Unbind()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                MoveSlotUI moveSlot = child.GetComponent<MoveSlotUI>();
                MenuButton button = child.GetComponent<MenuButton>();

                if (moveSlot != null && button != null)
                {
                    moveSlot.Unbind();
                    button.SetInteractable(false);
                }
            }
        }

        private void BindMoves(IReadOnlyList<Move> moves)
        {
            for (int i = 0; i < moves.Count && i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                MoveSlotUI moveSlot = child.GetComponent<MoveSlotUI>();
                MenuButton button = child.GetComponent<MenuButton>();

                if (moveSlot != null && button != null)
                {
                    moveSlot.Bind(moves[i]);
                    button.SetInteractable(true);
                }
            }
        }
    }
}
