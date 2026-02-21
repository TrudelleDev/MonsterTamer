using System.Collections;
using MonsterTamer.Battle.States.Core;

namespace MonsterTamer.Battle.States.Player
{
    /// <summary>
    /// Handles the final transition out of a wild battle after all rewards 
    /// (processed in BattleExperienceState) are finished.
    /// </summary>
    internal sealed class PlayerWildVictoryState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private BattleView Battle => machine.BattleView;

        internal PlayerWildVictoryState(BattleStateMachine machine)
        {
            this.machine = machine;
        }

        public void Enter() => Battle.StartCoroutine(PlaySequence());

        public void Update() { }
        public void Exit() { }

        private IEnumerator PlaySequence()
        {
            // At this point, BattleExperienceState has already finished.
            // We just pause for a beat and close the curtain.
            yield return Battle.TurnPauseYield;

            Battle.CloseBattle();
        }
    }
}