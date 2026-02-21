using System;
using MonsterTamer.Monsters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Summary
{
    /// <summary>
    /// Coordinates the data binding for the summary header and all secondary information tabs. 
    /// </summary>
    [Serializable]
    internal sealed class SummaryTabGroup
    {
        [SerializeField, Required] private SummaryHeader header;
        [SerializeField, Required] private SummaryInfoTab infoTab;
        [SerializeField, Required] private SummarySkillTab skillTab;
        [SerializeField, Required] private SummaryMoveTab moveTab;

        internal void Bind(Monster monster)
        {
            Unbind();

            if (monster?.Definition == null) return;

            header.Bind(monster);
            infoTab.Bind(monster);
            skillTab.Bind(monster);
            moveTab.Bind(monster);
        }

        internal void Unbind()
        {
            header.Unbind();
            infoTab.Unbind();
            skillTab.Unbind();
            moveTab.Unbind();
        }

        internal void ResetControllers()
        {
            moveTab.MenuController.ResetToFirst();
        }
    }
}