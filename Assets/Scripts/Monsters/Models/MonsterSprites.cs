using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Monsters.Models
{
    /// <summary>
    /// Contains references for a monster's visual representation:
    /// front, back, and menu sprites.
    /// </summary>
    [Serializable]
    internal struct MonsterSprites
    {
        [SerializeField, Required, Tooltip("Front sprite used in battle (opponent) and summary views.")]
        private Sprite frontSprite;

        [SerializeField, Required, Tooltip("Back sprite used in battle (player side).")]
        private Sprite backSprite;

        [SerializeField, Required, Tooltip("Sprite used in menus like party and summary.")]
        private Sprite menuSprite;

        internal readonly Sprite FrontSprite => frontSprite;
        internal readonly Sprite BackSprite => backSprite;
        internal readonly Sprite MenuSprite => menuSprite;
    }
}
