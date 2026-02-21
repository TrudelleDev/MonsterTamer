using MonsterTamer.Monsters;
using MonsterTamer.Monsters.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MonsterTamer.Summary
{
    /// <summary>
    /// Displays a monster's identity, including name and visual representation.
    /// </summary>
    internal sealed class SummaryHeader : MonoBehaviour
    {
        [SerializeField, Required] private TextMeshProUGUI nameText;
        [SerializeField, Required] private MonsterSprite sprite;

        internal void Bind(Monster monster)
        {
            Unbind();

            if (monster?.Definition == null) return;

            nameText.text = monster.Definition.DisplayName;
            sprite.Bind(monster);
        }

        internal void Unbind()
        {
            nameText.text = string.Empty;
            sprite.Unbind();
        }
    }
}