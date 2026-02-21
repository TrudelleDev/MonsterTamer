using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;
using MonsterTamer.Monsters;

namespace MonsterTamer.Battle.States.Player
{
    /// <summary>
    /// Handle the forced send-out sequence when a player must replace a fainted monster.
    /// </summary>
    internal sealed class PlayerForcedSendOutState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private readonly Monster selectedMonster;
        private BattleView Battle => machine.BattleView;

        internal PlayerForcedSendOutState(BattleStateMachine machine, Monster selectedMonster)
        {
            this.machine = machine;
            this.selectedMonster = selectedMonster;
        }

        public void Enter() => Battle.StartCoroutine(PlaySequence());

        public void Update() { }
        public void Exit() { }

        private IEnumerator PlaySequence()
        {
            var animation = Battle.Components.Animation;

            // Get data from new Monster
            Battle.SetNextPlayerMonster(selectedMonster);
            var monsterName = selectedMonster.Definition.DisplayName;
            var sendMessage = BattleMessages.PlayerSendMonster(monsterName);

            Battle.DialogueBox.DisplayWithInput(sendMessage);

            yield return animation.PlayPlayerMonsterEnter();
            yield return animation.PlayPlayerHudEnter();

            machine.SetState(new PlayerActionMenuState(machine));
        }
    }
}
