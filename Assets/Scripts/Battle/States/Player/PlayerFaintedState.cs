using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;
using MonsterTamer.Monsters;

namespace MonsterTamer.Battle.States.Player
{
    /// <summary>
    /// Handle the player faint sequence and forces the player to switch monsters or triggers a blackout if none remain.
    /// </summary>
    internal sealed class PlayerFaintedState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private readonly Monster faintedMonster;
        private BattleView Battle => machine.BattleView;

        internal PlayerFaintedState(BattleStateMachine machine, Monster faintedMonster)
        {
            this.machine = machine;
            this.faintedMonster = faintedMonster;
        }

        public void Enter() => Battle.StartCoroutine(PlaySequence());

        public void Update() { }
        public void Exit() { }

        private IEnumerator PlaySequence()
        {
            var animation = Battle.Components.Animation;
            var dialogue = Battle.DialogueBox;
            var faintMessage = BattleMessages.FaintedMessage(faintedMonster.Definition.DisplayName);

            // Remove active Monster from the field
            animation.PlayPlayerMonsterDeath();
            animation.PlayPlayerHudExit();

            yield return dialogue.DisplayBattleDialogue(faintMessage);

            // Force the player to choose and other Monster.
            if (Battle.Player.Party.HasAnyUsableMonster)
            {
                machine.SetState(new PlayerPartySelectState(machine, isForced: true));
            }            
            else
            {
                machine.SetState(new PlayerBlackoutState(machine));
            }                   
        }
    }
}
