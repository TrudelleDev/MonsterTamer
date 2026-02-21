using MonsterTamer.Moves;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Battle.UI
{
    /// <summary>
    /// Displays the currently selected move's details, including remaining PP and elemental type icon.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class BattleMoveInfoPanel : MonoBehaviour
    {
        [SerializeField, Required] private TextMeshProUGUI ppText;
        [SerializeField, Required] private Image typeIcon;

        internal void Bind(Move move)
        {
            if (move?.Definition == null)
            {
                Unbind();
                return;
            }

            ppText.text = $"{move.PowerPointRemaining}/{move.Definition.MoveInfo.PowerPoint}";
            typeIcon.sprite = move.Definition.Classification.TypeDefinition.Icon;
        }

        internal void Unbind()
        {
            ppText.text = "- / -";
            typeIcon.sprite = null;
        }
    }
}
