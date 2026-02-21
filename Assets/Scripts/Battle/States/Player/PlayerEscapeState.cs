using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;

namespace MonsterTamer.Battle.States.Player
{
    /// <summary>
    /// Handle the sequence for successfully fleeing from a wild battle.
    /// </summary>
    internal sealed class PlayerEscapeState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private BattleView Battle => machine.BattleView;

        internal PlayerEscapeState(BattleStateMachine machine)
        {
            this.machine = machine;
        }

        public void Enter() => Battle.StartCoroutine(PlaySequence());

        public void Update() { }
        public void Exit() { }

        private IEnumerator PlaySequence()
        {
            yield return Battle.DialogueBox.DisplayAndWaitTyping(BattleMessages.EscapeSuccess);
            yield return Battle.TurnPauseYield;

            Battle.CloseBattle();
        }
    }
}
