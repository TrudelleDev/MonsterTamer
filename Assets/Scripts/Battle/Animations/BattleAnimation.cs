using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Utilities;
using UnityEngine;

namespace MonsterTamer.Battle.Animations
{
    /// <summary>
    /// Controls all battle-related animations for players, opponents, and wild monsters.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class BattleAnimation : MonoBehaviour
    {
        [SerializeField] private PlayerAnimations playerAnimations;
        [SerializeField] private OpponentAnimations opponentAnimations;

        // Opponent animations
        internal IEnumerator PlayOpponentHudEnter() => AnimatorHelper.PlayAndWait(opponentAnimations.HudAnimator, BattleAnimationStates.OpponentHudEnter);
        internal IEnumerator PlayOpponentHudExit() => AnimatorHelper.PlayAndWait(opponentAnimations.HudAnimator, BattleAnimationStates.OpponentHudExit);
        internal IEnumerator PlayOpponentMonsterTakeDamage() => AnimatorHelper.PlayAndWait(opponentAnimations.MonsterAnimator, BattleAnimationStates.OpponentMonsterTakeDamage);
        internal IEnumerator PlayOpponentTrainerEnter() => AnimatorHelper.PlayAndWait(opponentAnimations.TrainerAnimator, BattleAnimationStates.OpponentTrainerEnter);
        internal IEnumerator PlayOpponentTrainerExit() => AnimatorHelper.PlayAndWait(opponentAnimations.TrainerAnimator, BattleAnimationStates.OpponentTrainerExit);
        internal IEnumerator PlayOpponentMonsterEnter() => AnimatorHelper.PlayAndWait(opponentAnimations.MonsterAnimator, BattleAnimationStates.OpponentMonsterEnter);
        internal IEnumerator PlayOpponentTrainerDefeatOutro() => AnimatorHelper.PlayAndWait(opponentAnimations.TrainerAnimator, BattleAnimationStates.OpponentTrainerDefeatOutro);
        internal void PlayOpponentMonsterDeath() => opponentAnimations.MonsterAnimator.Play(BattleAnimationStates.OpponentMonsterDeath);

        // Player animations
        internal IEnumerator PlayPlayerHudEnter() => AnimatorHelper.PlayAndWait(playerAnimations.HudAnimator, BattleAnimationStates.PlayerHudEnter);
        internal IEnumerator PlayPlayerTrainerExit() => AnimatorHelper.PlayAndWait(playerAnimations.TrainerSpriteAnimator, BattleAnimationStates.PlayerTrainerExit);
        internal IEnumerator PlayPlayerMonsterEnter() => AnimatorHelper.PlayAndWait(playerAnimations.MonsterAnimator, BattleAnimationStates.PlayerMonsterEnter);
        internal IEnumerator PlayPlayerMonsterExit() => AnimatorHelper.PlayAndWait(playerAnimations.MonsterAnimator, BattleAnimationStates.PlayerMonsterExit);
        internal IEnumerator PlayPlayerMonsterTakeDamage() => AnimatorHelper.PlayAndWait(playerAnimations.MonsterAnimator, BattleAnimationStates.PlayerMonsterTakeDamage);
        internal void PlayPlayerHudExit() => playerAnimations.HudAnimator.Play(BattleAnimationStates.PlayerHudExit);
        internal void PlayPlayerTrainerEnter() => playerAnimations.TrainerSpriteAnimator.Play(BattleAnimationStates.PlayerTrainerEnter);
        internal void PlayPlayerMonsterDeath() => playerAnimations.MonsterAnimator.Play(BattleAnimationStates.PlayerMonsterDeath);

        // Wild monster animations
        internal IEnumerator PlayWildMonsterEnter() => AnimatorHelper.PlayAndWait(opponentAnimations.MonsterAnimator, BattleAnimationStates.WildMonsterEnter);

        internal void ResetIntro()
        {
            AnimatorHelper.RebindAll(
                playerAnimations.HudAnimator, playerAnimations.MonsterAnimator, playerAnimations.TrainerSpriteAnimator,
                opponentAnimations.HudAnimator, opponentAnimations.MonsterAnimator, opponentAnimations.TrainerAnimator
            );
        }
    }
}