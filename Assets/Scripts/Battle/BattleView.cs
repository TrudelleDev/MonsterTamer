using System;
using System.Collections;
using MonsterTamer.Audio;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;
using MonsterTamer.Battle.States.Intro;
using MonsterTamer.Characters.Core;
using MonsterTamer.Dialogue;
using MonsterTamer.Monsters;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Battle
{
    /// <summary>
    /// Central controller for the battle view, handling HUDs, animations, dialogue,
    /// active monsters, and the battle state machine.
    /// </summary>

    [DisallowMultipleComponent]
    internal sealed class BattleView : View
    {
        [SerializeField, Required] private AudioClip battleBgm;
        [SerializeField, Required] private DialogueBox dialogueBox;
        [SerializeField, Required] private Image opponentTrainerSprite;
        [SerializeField] private BattleHuds battleHuds;
        [SerializeField] private BattleComponents components;

        private BattleStateMachine stateMachine;
        private IBattleState introState;

        internal event Action OnBattleViewClose;
        internal readonly WaitForSecondsRealtime TurnPauseYield = new(0.5f);

        internal Character Player { get; private set; }
        internal Character Opponent { get; private set; }
        internal Monster PlayerActiveMonster { get; private set; }
        internal Monster OpponentActiveMonster { get; private set; }

        internal BattleHuds BattleHUDs => battleHuds;
        internal BattleComponents Components => components;
        internal DialogueBox DialogueBox => dialogueBox;

        private void OnEnable()
        {
            StartCoroutine(PlayIntro());
        }

        protected override void Update()
        {
            stateMachine?.Update();
            base.Update();
        }

        /// <summary>
        /// Initializes a wild battle against a single Monster.
        /// </summary>
        internal void InitializeWildBattle(Character player, Monster wildMonster)
        {
            InitializeBattle(player, null, player.Party.Members[0], wildMonster);
            introState = new WildBattleIntroState(stateMachine);

            AudioManager.Instance.PlayBGM(battleBgm);
        }

        /// <summary>
        /// Initializes a trainer battle against another character.
        /// </summary>
        internal void InitializeTrainerBattle(Character player, Character opponent)
        {
            InitializeBattle(player, opponent, player.Party.Members[0], opponent.Party.Members[0]);
            introState = new TrainerBattleIntroState(stateMachine);

            opponentTrainerSprite.sprite = opponent.Definition.BattleSprite;
            AudioManager.Instance.PlayBGM(battleBgm);
        }

        /// <summary>
        /// Generalized initialization logic for wild and trainer battles.
        /// </summary>
        private void InitializeBattle(Character player, Character opponentOrNull, Monster playerMonster, Monster opponentMonster)
        {
            Player = player;
            Opponent = opponentOrNull;

            Player.Party.CaptureOrder();

            PlayerActiveMonster = playerMonster;
            OpponentActiveMonster = opponentMonster;

            battleHuds.PlayerBattleHud.Bind(PlayerActiveMonster);
            battleHuds.OpponentBattleHud.Bind(OpponentActiveMonster);

            stateMachine = new BattleStateMachine(this);
        }

        /// <summary>
        /// Replaces the opponent's active Monster (trainer battles only).
        /// </summary>
        internal void SetNextOpponentMonster(Monster monster)
        {
            OpponentActiveMonster = monster;
            battleHuds.OpponentBattleHud.Bind(monster);
        }

        /// <summary>
        /// Replaces the player's active Monster and updates party order.
        /// </summary>
        internal void SetNextPlayerMonster(Monster monster)
        {
            PlayerActiveMonster = monster;
            battleHuds.PlayerBattleHud.Bind(monster);
            Player.Party.Swap(0, Player.Party.SelectedIndex);
        }

        /// <summary>
        /// Cleans up the battle state and closes the battle view.
        /// </summary>
        internal void CloseBattle()
        {
            battleHuds.PlayerBattleHud.Unbind();
            battleHuds.OpponentBattleHud.Unbind();

            Player.Party.RestoreAllHealth();
            components.Animation.ResetIntro();

            OnBattleViewClose?.Invoke();
            dialogueBox.Clear();

            ViewManager.Instance.Close<BattleView>();
        }

        /// <summary>
        /// Plays the battle intro once the view transition has completed.
        /// </summary>
        private IEnumerator PlayIntro()
        {
            yield return new WaitUntil(() => !ViewManager.Instance.IsTransitioning);
            stateMachine.SetState(introState);
        }
    }
}
