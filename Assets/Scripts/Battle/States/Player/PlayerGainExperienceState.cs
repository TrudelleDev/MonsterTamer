using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;
using MonsterTamer.Battle.States.Opponent;

namespace MonsterTamer.Battle.States.Player
{
    /// <summary>
    /// Handles XP distribution and level-up sequences immediately after a monster faints.
    /// </summary>
    internal sealed class PlayerGainExperienceState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private BattleView Battle => machine.BattleView;

        internal PlayerGainExperienceState(BattleStateMachine machine)
        {
            this.machine = machine;
        }

        public void Enter() => Battle.StartCoroutine(PlaySequence());

        public void Update() { }
        public void Exit() { }

        private IEnumerator PlaySequence()
        {
            var player = Battle.PlayerActiveMonster;
            var opponent = Battle.OpponentActiveMonster; // The one that just fainted
            var dialogue = Battle.DialogueBox;
            var expBar = Battle.BattleHUDs.PlayerBattleHud.ExperienceBar;

            // Calculate experience gain
            int expGained = player.Experience.CalculateExpGain(opponent);
            var expGainMessage = BattleMessages.GainExperience(player.Definition.DisplayName, expGained);
            yield return dialogue.DisplayBattleDialogue(expGainMessage);

            // Track Level Ups
            int levelsGained = 0;
            void OnLevelChange(int _) => levelsGained++;
            player.Experience.LevelChanged += OnLevelChange;

            // Animate experience bar
            player.Experience.AddExperience(expGained);
            yield return expBar.WaitForAnimationComplete();

            // Handle level up dialogue
            if (levelsGained > 0)
            {
                var levelUpMessage = BattleMessages.LevelUp(player.Definition.DisplayName, player.Experience.Level);
                yield return dialogue.DisplayBattleDialogue(levelUpMessage);
            }

            player.Experience.LevelChanged -= OnLevelChange;

            DetermineNextState();
        }

        private void DetermineNextState()
        {
            // Check if the opponent is a Trainer AND has more monsters to send out
            if (Battle.Opponent != null && Battle.Opponent.Party.HasAnyUsableMonster)
            {
                machine.SetState(new OpponentSendOutState(machine));
            }
            else
            {
                // Branch to the appropriate victory sequence based on the encounter type
                machine.SetState(Battle.Opponent != null
                    ? new PlayerTrainerVictoryState(machine)
                    : new PlayerWildVictoryState(machine));
            }
        }
    }
}
