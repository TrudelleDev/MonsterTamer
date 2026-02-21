using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;
using MonsterTamer.Battle.States.Opponent;
using MonsterTamer.Monsters;
using MonsterTamer.Moves;

namespace MonsterTamer.Battle.States.Player
{
    /// <summary>
    /// Handles the withdrawal of the current monster and the entry of a new one,
    /// then passes the turn to the opponent.
    /// </summary>
    internal sealed class PlayerSwapMonsterState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private readonly Monster newMonster;
        private readonly Move opponentMove;

        private BattleView Battle => machine.BattleView;

        internal PlayerSwapMonsterState(BattleStateMachine machine, Monster newMonster, Move opponentMove)
        {
            this.machine = machine;
            this.newMonster = newMonster;
            this.opponentMove = opponentMove;
        }

        public void Enter() => Battle.StartCoroutine(PlaySequence());

        public void Update() { }
        public void Exit() { }

        private IEnumerator PlaySequence()
        {
            var animation = Battle.Components.Animation;
            var dialogue = Battle.DialogueBox;

            // Withdraw Current Monster
            string currentName = Battle.PlayerActiveMonster.Definition.DisplayName;
            yield return dialogue.DisplayBattleDialogue(BattleMessages.MonsterReturnParty(currentName));

            animation.PlayPlayerHudExit();
            yield return animation.PlayPlayerMonsterExit();

            // Update new Monster data
            Battle.SetNextPlayerMonster(newMonster);
            string newName = newMonster.Definition.DisplayName;

            // Send new Monster
            dialogue.DisplayWithInput(BattleMessages.PlayerSendMonster(newName));
            yield return animation.PlayPlayerMonsterEnter();
            yield return animation.PlayPlayerHudEnter();

            // Opponent's Free Hit
            // isActingFirst is false because the player already used their turn to swap
            machine.SetState(new OpponentTurnState(machine, opponentMove, null, isActingFirst: false));
        }
    }
}
