using MonsterTamer.Monsters.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Monsters.UI
{
    /// <summary>
    /// Displays a monster sprite in the UI based on the configured sprite type.
    /// Automatically hides the image if the sprite is missing or invalid.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    internal sealed class MonsterSprite : MonoBehaviour
    {
        [SerializeField, Tooltip("Specifies which monster sprite variant to display.")]
        private MonsterSpriteType spriteType;

        private Image image;

        /// <summary>
        /// Binds a Monster instance and updates the sprite image.
        /// </summary>
        internal void Bind(Monster monster)
        {
            EnsureImage();

            if (monster?.Definition?.Sprites == null)
            {
                Unbind();
                return;
            }

            image.sprite = GetSprite(monster);
            image.enabled = image.sprite != null;
        }

        /// <summary>
        /// Hides the sprite image and resets it.
        /// </summary>
        internal void Unbind()
        {
            EnsureImage();
            image.sprite = null;
            image.enabled = false;
        }

        private Sprite GetSprite(Monster monster)
        {
            var sprites = monster.Definition.Sprites;

            return spriteType switch
            {
                MonsterSpriteType.Menu => sprites.MenuSprite,
                MonsterSpriteType.Front => sprites.FrontSprite,
                MonsterSpriteType.Back => sprites.BackSprite,
                _ => null
            };
        }

        private void EnsureImage()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }
        }
    }
}
