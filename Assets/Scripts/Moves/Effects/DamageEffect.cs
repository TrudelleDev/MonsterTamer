using System.Collections;
using MonsterTamer.Battle;
using MonsterTamer.Moves.Models;
using UnityEngine;

namespace MonsterTamer.Moves.Effects
{
    /// <summary>
    /// Applies direct damage to the target and handles its visual sequence.
    /// </summary>
    [CreateAssetMenu(menuName = "MonsterTamer/Move/Effects/Damage")]
    internal sealed class DamageEffect : MoveEffect
    {
        private static bool IsTargetPlayer(MoveContext context) => context.Target == context.Battle.PlayerActiveMonster;

        protected override void ApplyEffect(MoveContext context)
        {
            int damage = DamageCalculator.CalculateDamage(context.User, context.Target, context.Move);
            context.Target.Health.ApplyDamage(damage);
        }

        protected override IEnumerator WaitForHealthAnimation(MoveContext context)
        {
            var huds = context.Battle.BattleHUDs;
            var bar = IsTargetPlayer(context) ? huds.PlayerBattleHud.HealthBar : huds.OpponentBattleHud.HealthBar;

            yield return bar.WaitForHealthAnimationComplete();
        }

        protected override IEnumerator PlayEffectAnimation(MoveContext context)
        {
            var anim = context.Battle.Components.Animation;

            yield return IsTargetPlayer(context)
                ? anim.PlayPlayerMonsterTakeDamage()
                : anim.PlayOpponentMonsterTakeDamage();
        }

        internal override IEnumerator PerformMoveSequence(MoveContext context)
        {
            context.Move.ConsumePowerPoint();

            PlayEffectSound(context);
            yield return PlayEffectAnimation(context);

            ApplyEffect(context);
            yield return WaitForHealthAnimation(context);
        }
    }
}
