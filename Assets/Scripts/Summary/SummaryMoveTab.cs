using MonsterTamer.Monsters;
using MonsterTamer.Moves.UI;
using MonsterTamer.Shared.UI.Navigation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Summary
{
    /// <summary>
    /// Displays monster move-set and manages navigation within the summary interface. 
    /// </summary>
    internal sealed class SummaryMoveTab : MonoBehaviour
    {
        [SerializeField, Required] private MoveSlotUIManager moveManager;
        [SerializeField, Required] private VerticalMenuController controller;

        internal VerticalMenuController MenuController => controller;

        internal void Bind(Monster monster)
        {
            Unbind();

            if (monster?.Definition == null) return;

            moveManager.Bind(monster);
        }

        internal void Unbind() => moveManager.Unbind();
    }
}