using System;
using MonsterTamer.Battle.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Battle.Models
{
    /// <summary>
    /// References all HUD components used in a battle.
    /// </summary>
    [Serializable]
    internal struct BattleHuds
    {
        [SerializeField, Required] private PlayerBattleHud playerBattleHud;
        [SerializeField, Required] private OpponentBattleHud opponentBattleHud;

        internal readonly PlayerBattleHud PlayerBattleHud => playerBattleHud;
        internal readonly OpponentBattleHud OpponentBattleHud => opponentBattleHud;
    }
}
