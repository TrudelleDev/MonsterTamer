using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;
using MonsterTamer.Battle.States.Player;

namespace MonsterTamer.Battle.States.Intro
{
    /// <summary>
    /// Play the opening sequence of a trainer battle and transitions to the player's action menu.
    /// </summary>
    internal sealed class TrainerBattleIntroState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private BattleView Battle => machine.BattleView;

        internal TrainerBattleIntroState(BattleStateMachine machine)
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
            yield return PlayOpponentEntrance();
            yield return PlayPlayerEntrance();

            machine.SetState(new PlayerActionMenuState(machine));
        }

        private IEnumerator PlayOpponentEntrance()
        {
            var animation = Battle.Components.Animation;
            var monster = Battle.OpponentActiveMonster;
            var trainer = Battle.Opponent.Definition;

            var introMessage = BattleMessages.TrainerIntro(trainer.DisplayName);
            var sendMessage = BattleMessages.TrainerSentOut(trainer.DisplayName, monster.Definition.DisplayName);

            // Player trainer appears (sync with opponent)
            animation.PlayPlayerTrainerEnter();
            yield return animation.PlayOpponentTrainerEnter();

            // Show battle intro messages
            yield return Battle.DialogueBox.DisplayBattleDialogue(introMessage);
            Battle.DialogueBox.DisplayWithInput(sendMessage);

            // Opponent trainer exits, monster enters, and HUD shows
            yield return animation.PlayOpponentTrainerExit();
            yield return animation.PlayOpponentMonsterEnter();
            yield return animation.PlayOpponentHudEnter();
        }

        private IEnumerator PlayPlayerEntrance()
        {
            var animation = Battle.Components.Animation;
            var monster = Battle.PlayerActiveMonster;

            // Show message for sending out player's monster
            var sendMessage = BattleMessages.PlayerSendMonster(monster.Definition.DisplayName);
            Battle.DialogueBox.DisplayWithInput(sendMessage);

            // Player trainer exits, monster enters, and HUD shows
            yield return animation.PlayPlayerTrainerExit();
            yield return animation.PlayPlayerMonsterEnter();
            yield return animation.PlayPlayerHudEnter();
        }
    }
}
