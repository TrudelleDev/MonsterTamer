namespace MonsterTamer.Battle.Models
{
    /// <summary>
    /// Contains all player-facing battle messages.
    /// </summary>
    internal static class BattleMessages
    {
        // Battle intros
        internal static string TrainerIntro(string trainerName) => $"{trainerName} would like to battle!";
        internal static string WildIntro(string monsterName) => $"Wild {monsterName} appeared!";
        internal static string ChooseAction(string playerName) => $"What will {playerName} do?";

        // Escape messages
        internal const string EscapeSuccess = "Got away safely!";
        internal const string EscapeFail = "Can't escape!";
        public const string EscapeTrainer = "You cannot escape a Trainer battle!";

        // Battle flow
        internal static string TrainerWantsToBattle(string trainerName) => $"{trainerName} wants to battle!";
        internal static string TrainerSentOut(string trainerName, string monsterName) => $"{trainerName} sent out {monsterName}!";
        internal static string PlayerSendMonster(string monsterName) => $"Go {monsterName}!";
        internal static string MonsterReturnParty(string monsterName) => $"{monsterName} that's enough!\nCome back!";
        internal static string UseMove(string userName, string moveName) => $"{userName} used {moveName}!";
        internal static string FaintedMessage(string monsterName) => $"{monsterName} fainted!";

        // Defeat / blackout
        internal const string BlackoutMessage = "All your monsters have fainted!\nYou blacked out...";
        internal const string CheckpointRelocationMessage = "You were rushed to\nthe nearest checkpoint.";

        // Progression
        internal static string GainExperience(string monsterName, int exp) => $"{monsterName} gained\n{exp} Exp. Points.";
        internal static string LevelUp(string monsterName, int level) => $"{monsterName} grew to Level {level}!";

        // Validation
        internal static string MonsterHasNoEnergy(string monsterName) => $"{monsterName} has no energy left to fight!";
        internal const string MonsterAlreadyInBattle = "That Monster is already in battle!";
    }
}
