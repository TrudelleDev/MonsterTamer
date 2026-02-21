using MonsterTamer.Monsters;
using MonsterTamer.Monsters.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Battle.UI
{
    /// <summary>
    /// Displays the opponent active Monster's battle HUD.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class OpponentBattleHud : MonoBehaviour
    {
        [SerializeField, Required] private TextMeshProUGUI nameText;
        [SerializeField, Required] private TextMeshProUGUI levelText;
        [SerializeField, Required] private HealthBar healthBar;
        [SerializeField, Required] private Image frontSprite;

        internal HealthBar HealthBar => healthBar;

        internal void Bind(Monster monster)
        {
            if (monster?.Definition == null)
            {
                Unbind();
                return;
            }

            nameText.text = monster.Definition.DisplayName;
            levelText.text = $"L{monster.Experience.Level}";
            healthBar.Bind(monster);
            frontSprite.sprite = monster.Definition.Sprites.FrontSprite;
        }

        internal void Unbind()
        {
            nameText.text = string.Empty;
            levelText.text = string.Empty;
            frontSprite.sprite = null;
            healthBar.Unbind();
        }
    }
}
