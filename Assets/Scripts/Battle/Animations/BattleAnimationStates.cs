using UnityEngine;

namespace MonsterTamer.Battle.Animations
{
    /// <summary>
    /// Contains static Animator state hashes used by battle animation sequences.
    /// </summary>
    internal static class BattleAnimationStates
    {
        // Player animations
        internal static readonly int PlayerHudEnter = Animator.StringToHash("PlayerHudEnter");
        internal static readonly int PlayerHudExit = Animator.StringToHash("PlayerHudExit");
        internal static readonly int PlayerMonsterEnter = Animator.StringToHash("PlayerMonsterEnter");
        internal static readonly int PlayerMonsterExit = Animator.StringToHash("PlayerMonsterExit");
        internal static readonly int PlayerMonsterTakeDamage = Animator.StringToHash("PlayerMonsterTakeDamage");
        internal static readonly int PlayerMonsterDeath = Animator.StringToHash("PlayerMonsterDeath");
        internal static readonly int PlayerTrainerEnter = Animator.StringToHash("PlayerTrainerEnter");
        internal static readonly int PlayerTrainerExit = Animator.StringToHash("PlayerTrainerExit");

        // Opponent animations
        internal static readonly int OpponentHudEnter = Animator.StringToHash("OpponentHudEnter");
        internal static readonly int OpponentHudExit = Animator.StringToHash("OpponentHudExit");
        internal static readonly int OpponentMonsterEnter = Animator.StringToHash("OpponentMonsterEnter");
        internal static readonly int OpponentMonsterExit = Animator.StringToHash("OpponentMonsterExit");
        internal static readonly int OpponentMonsterTakeDamage = Animator.StringToHash("OpponentMonsterTakeDamage");
        internal static readonly int OpponentMonsterDeath = Animator.StringToHash("OpponentMonsterDeath");
        internal static readonly int OpponentTrainerEnter = Animator.StringToHash("OpponentTrainerEnter");
        internal static readonly int OpponentTrainerExit = Animator.StringToHash("OpponentTrainerExit");
        internal static readonly int OpponentTrainerDefeatOutro = Animator.StringToHash("OpponentTrainerDefeatOutro");

        // Wild monster animations
        internal static readonly int WildMonsterEnter = Animator.StringToHash("WildMonsterEnter");
    }
}
