using System.Linq;
using MonsterTamer.Battle;
using MonsterTamer.Characters.Core;
using MonsterTamer.Characters.Player;
using MonsterTamer.Monsters;
using MonsterTamer.Monsters.Models;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MonsterTamer.Encounters
{
    /// <summary>
    /// Coordinates wild monster encounters by monitoring player movement over specific Tilemaps.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class WildEncounterManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Required] private Tilemap encounterTilemap;
        [SerializeField, Range(0, 100)] private int encounterChance = 10;
        [SerializeField, Required] private WildMonsterDatabase monsterDatabase;

        private Character player;
        private CharacterStateController playerStateController;
        private bool encounterLocked;

        private void Awake()
        {
            player = PlayerRegistry.Player;
            playerStateController = player.GetComponent<CharacterStateController>();
        }

        private void OnEnable()
        {
            if (player != null)
                playerStateController.TileMover.MoveCompleted += HandleMoveCompleted;
        }

        private void OnDisable()
        {
            if (player != null)
                playerStateController.TileMover.MoveCompleted -= HandleMoveCompleted;
        }

        private void HandleMoveCompleted()
        {
            if (encounterLocked) return;
            if (!IsPlayerOnEncounterTile()) return;
            if (!RollEncounter()) return;

            TriggerBattle();
        }

        private bool IsPlayerOnEncounterTile()
        {
            Vector3Int cell = encounterTilemap.WorldToCell(player.transform.position);
            return encounterTilemap.HasTile(cell);
        }

        private bool RollEncounter() => Random.Range(0, 100) < encounterChance;

        private void UnlockEncounter() => encounterLocked = false;

        private void TriggerBattle()
        {
            encounterLocked = true;
            playerStateController.CancelToIdle();

            WildMonsterEntry entry = ChooseWildMonster();
            int level = Random.Range(entry.MinLevel, entry.MaxLevel + 1);
            Monster monster = MonsterFactory.Create(level, entry.Definition);

            BattleView battle = ViewManager.Instance.Show<BattleView>();
            battle.InitializeWildBattle(player, monster);
            battle.OnBattleViewClose += UnlockEncounter;
        }

        private WildMonsterEntry ChooseWildMonster()
        {
            int totalWeight = monsterDatabase.Entries.Sum(e => e.EncounterRate);
            int roll = Random.Range(0, totalWeight);
            int cumulative = 0;

            return monsterDatabase.Entries.First(e => (cumulative += e.EncounterRate) > roll);
        }
    }
}