using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;
using MonsterTamer.Characters.Player;
using MonsterTamer.Map;
using MonsterTamer.Views;

namespace MonsterTamer.Battle.States.Player
{
    /// <summary>
    /// Handle the blackout sequence and teleporting the player back to a checkpoint when all monsters have fainted.
    /// </summary>
    internal sealed class PlayerBlackoutState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private BattleView Battle => machine.BattleView;

        internal PlayerBlackoutState(BattleStateMachine machine)
        {
            this.machine = machine;
        }

        public void Enter() => Battle.StartCoroutine(PlayBlackoutSequence());

        public void Update() { }
        public void Exit() { }

        private IEnumerator PlayBlackoutSequence()
        {
            var dialogue = Battle.DialogueBox;

            yield return dialogue.DisplayBattleDialogue(BattleMessages.BlackoutMessage);

            // Teleport player to the checkpoint and restore party health
            MapEntryRegistry.SetNextEntry(MapEntryID.ForestEntrance);
            PlayerRelocator.Instance.RelocatePlayer();
            Battle.Player.Party.RestoreAllHealth();

            yield return dialogue.DisplayAndWaitTyping(BattleMessages.CheckpointRelocationMessage);
            yield return Battle.TurnPauseYield;

            ViewManager.Instance.Close<BattleView>();
        }
    }
}
