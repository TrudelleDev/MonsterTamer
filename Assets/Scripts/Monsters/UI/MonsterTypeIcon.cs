using MonsterTamer.Monsters.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Monsters.UI
{
    /// <summary>
    /// Displays a monster's primary or secondary type icon.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    internal sealed class MonsterTypeIcon : MonoBehaviour
    {
        [SerializeField, Required]
        [Tooltip("Selects whether to display the primary or secondary monster type.")]
        private MonsterTypeSlot slot;

        private Image typeImage;

        /// <summary>
        /// Binds a monster instance and displays the appropriate type icon.
        /// </summary>
        internal void Bind(Monster monster)
        {
            EnsureImage();

            if (monster == null || monster.Definition == null)
            {
                Unbind();
                return;
            }

            Sprite sprite = GetTypeSprite(monster);
            typeImage.sprite = sprite;
            typeImage.gameObject.SetActive(sprite != null);
        }

        /// <summary>
        /// Hides the type icon.
        /// </summary>
        internal void Unbind()
        {
            EnsureImage();
            typeImage.gameObject.SetActive(false);
        }

        private Sprite GetTypeSprite(Monster monster)
        {
            var types = monster.Definition.Typing;

            return slot switch
            {
                MonsterTypeSlot.Primary => types.FirstType?.Icon,
                MonsterTypeSlot.Secondary => types.SecondType?.Icon,
                _ => null
            };
        }

        private void EnsureImage()
        {
            if (typeImage == null)
            {
                typeImage = GetComponent<Image>();
            }
        }
    }
}
