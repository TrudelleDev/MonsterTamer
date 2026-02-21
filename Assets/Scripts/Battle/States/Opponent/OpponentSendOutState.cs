using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;
using MonsterTamer.Battle.States.Player;

namespace MonsterTamer.Battle.States.Opponent
{
    /// <summary>
    /// Plays the sequence of sending out an opponent's Monster and transitions to the player's action menu.
    /// </summary>
    internal sealed class OpponentSendOutState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private BattleView Battle => machine.BattleView;

        internal OpponentSendOutState(BattleStateMachine machine)
        {
            this.machine = machine;
        }

        public void Enter()
        {
            Battle.StartCoroutine(PlaySequence());
        }

        public void Update() { }
        public void Exit() { }

        private IEnumerator PlaySequence()
        {
            var monster = Battle.Opponent.Party.FirstUsableMonster;
            var animation = Battle.Components.Animation;
            var trainerName = Battle.Opponent.Definition.DisplayName;
            var monsterName = monster.Definition.DisplayName;
            var sendMessage = BattleMessages.TrainerSentOut(trainerName, monsterName);

            if (monster is null)
            {
                // No usable monster; opponent lost
                machine.SetState(new PlayerWildVictoryState(machine));
                yield break;
            }

            Battle.SetNextOpponentMonster(monster);

            yield return Battle.DialogueBox.DisplayAndWaitTyping(sendMessage); 
            yield return animation.PlayOpponentMonsterEnter();
            yield return animation.PlayOpponentHudEnter();

            machine.SetState(new PlayerActionMenuState(machine));
        }
    }
}
