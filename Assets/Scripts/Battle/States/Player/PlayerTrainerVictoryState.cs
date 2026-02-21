using System.Collections;
using MonsterTamer.Battle.States.Core;

namespace MonsterTamer.Battle.States.Player
{
    /// <summary>
    /// Handles the victory flow for trainer battles, playing the defeat outro and trainer dialogue.
    /// </summary>
    internal sealed class PlayerTrainerVictoryState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private BattleView Battle => machine.BattleView;

        internal PlayerTrainerVictoryState(BattleStateMachine machine)
        {
            this.machine = machine;
        }

        public void Enter() => Battle.StartCoroutine(PlaySequence());

        public void Exit() { }
        public void Update() { }

        private IEnumerator PlaySequence()
        {
            var animation = Battle.Components.Animation;
            var opponent = Battle.Opponent.Definition;
            var dialogue = Battle.DialogueBox;

            // Trainer Outro Logic
            yield return animation.PlayOpponentHudExit();
            yield return animation.PlayOpponentTrainerDefeatOutro();
            yield return dialogue.DisplayBattleDialogue(opponent.PostBattleClosingDialogue);

            // Return to Overworld
            Battle.CloseBattle();
        }
    }
}