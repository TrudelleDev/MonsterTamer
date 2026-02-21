using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Transitions
{
    /// <summary>
    /// Central registry for resolving TransitionType enums into transition instances.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class TransitionLibrary : Singleton<TransitionLibrary>
    {
        [SerializeField, Required] private Transition alphaFade;
        [SerializeField, Required] private Transition battleIntro;

        internal Transition Resolve(TransitionType type)
        {
            return type switch
            {
                TransitionType.AlphaFade => alphaFade,
                TransitionType.BattleIntro => battleIntro,
                _ => null
            };
        }
    }
}