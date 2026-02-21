using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Moves.UI
{
    /// <summary>
    /// Displays a compact UI slot for a Monster move.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class MoveSlotUI : MonoBehaviour
    {
        [SerializeField, Required] private TextMeshProUGUI nameText;
        [SerializeField, Required] private TextMeshProUGUI powerPointText;
        [SerializeField, Required] private Image typeImage;

        public Move BoundMove { get; private set; }

        internal void Bind(Move move)
        {
            if (move?.Definition == null)
            {
                Unbind();
                return;
            }

            BoundMove = move;
            nameText.text = move.Definition.DisplayName;
            powerPointText.text = $"{move.PowerPointRemaining}/{move.Definition.MoveInfo.PowerPoint}";
            powerPointText.alignment = TextAlignmentOptions.Right;
            typeImage.sprite = move.Definition.Classification.TypeDefinition.Icon;
            typeImage.enabled = true;
        }

        internal void Unbind()
        {
            BoundMove = null;
            nameText.text = "-";
            powerPointText.text = "--";
            powerPointText.alignment = TextAlignmentOptions.Left;
            typeImage.sprite = null;
            typeImage.enabled = false;
        }
    }
}
