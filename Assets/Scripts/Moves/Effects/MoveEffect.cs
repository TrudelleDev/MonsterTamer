using System.Collections;
using MonsterTamer.Moves.Models;
using UnityEngine;

namespace MonsterTamer.Moves.Effects
{
    /// <summary>
    /// Base class for all move effects. Defines the execution flow of a move.
    /// </summary>
    internal abstract class MoveEffect : ScriptableObject
    {
        protected virtual IEnumerator WaitForHealthAnimation(MoveContext context)
        {
            yield break;
        }

        protected abstract void ApplyEffect(MoveContext context);

        protected abstract IEnumerator PlayEffectAnimation(MoveContext context);

        protected virtual void PlayEffectSound(MoveContext context)
        {
            context.Battle.Components.Audio.PlayMoveSound(context.Move.Definition);
        }

        internal abstract IEnumerator PerformMoveSequence(MoveContext context);
    }
}
