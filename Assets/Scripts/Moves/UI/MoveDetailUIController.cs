using MonsterTamer.Shared.UI.Core;
using MonsterTamer.Shared.UI.Navigation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Moves.UI
{
    /// <summary>
    /// Controls the move detail section in the summary screen.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class MoveDetailUIController : MonoBehaviour
    {
        [SerializeField, Required] private MoveDetailUI moveDetail;
        [SerializeField, Required] private VerticalMenuController controller;

        private void Awake() => controller.Focused += OnControllerSelect;
        private void OnDestroy() => controller.Focused -= OnControllerSelect;

        private void OnControllerSelect(MenuButton button)
        {
            MoveSlotUI summaryMove = button.GetComponent<MoveSlotUI>();

            if (summaryMove.BoundMove != null)
            {
                moveDetail.Bind(summaryMove.BoundMove);
            }
            else
            {
                moveDetail.Unbind();
            }
        }
    }
}
