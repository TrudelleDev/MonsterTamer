using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MonsterTamer.Moves.UI
{
    /// <summary>
    /// Displays basic details of a Monster move.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class MoveDetailUI : MonoBehaviour
    {
        [SerializeField, Required]private TextMeshProUGUI powerText;
        [SerializeField, Required]private TextMeshProUGUI accuracyText;
        [SerializeField, Required]private TextMeshProUGUI effectText;

        internal void Bind(Move move)
        {
            if (move?.Definition == null)
            {
                Unbind();
                return;
            }

            powerText.text = move.Definition.MoveInfo.Power.ToString();
            accuracyText.text = move.Definition.MoveInfo.Accuracy.ToString();
            effectText.text = move.Definition.Effect;
        }

        internal void Unbind()
        {
            powerText.text = string.Empty;
            accuracyText.text = string.Empty;
            effectText.text = string.Empty;
        }
    }
}
