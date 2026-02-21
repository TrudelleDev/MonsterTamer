using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;
using MonsterTamer.Battle.States.Player;
using MonsterTamer.Monsters;

namespace MonsterTamer.Battle.States.Opponent
{
    /// <summary>
    /// Plays the opponent faint sequence and transitions to experience gain.
    /// </summary>
    internal sealed class OpponentFaintedState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private readonly Monster monster;
        private BattleView Battle => machine.BattleView;

        internal OpponentFaintedState(BattleStateMachine machine, Monster monster)
        {
            this.machine = machine;
            this.monster = monster;
        }

        public void Enter()
        {
            Battle.StartCoroutine(PlaySequence());
        }

        public void Update() { }
        public void Exit() { }

        private IEnumerator PlaySequence()
        {
            var animation = Battle.Components.Animation;
            var faintMessage = BattleMessages.FaintedMessage(monster.Definition.DisplayName);
    
            animation.PlayOpponentMonsterDeath();
            yield return animation.PlayOpponentHudExit();
            yield return Battle.DialogueBox.DisplayBattleDialogue(faintMessage);

            machine.SetState(new PlayerGainExperienceState(machine));
        }
    }
}
