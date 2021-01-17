using System;

namespace Common
{
    /// <summary>
    /// Events class contains all constant variables to be used by our custom event handler class
    /// </summary>
    public static class Events
    {
        #region Custom Events
        // MAIN MENU
        public const string StartGame = "START_GAME";
        
        // INGAME MENU
        public const string RestartGame = "RESTART_GAME";

        // MODULAR MENU

        // TIMER
        public const string TimeTickUpdated = "TIME_TICK_UPDATED";
        
        // HUD
        public const string ChangeMoneyAmout = "CHANGE_MONEY_AMOUNT";
        public const string ChangeRemainingLifeAmount = "CHANGE_REMANING_LIFE_AMOUNT";
        public const string EnemyKilled = "ENEMY_KILLED";
        [Obsolete] public const string EnemyDied = "ENEMY_DIED";
        
        // GAME MANAGER
        public const string GameIsOver = "GAME_IS_OVER";
        public const string WonGame = "WON_GAME";
        public const string MoneyUpdated = "MONEY_UPDATED";

        // WAVE GENERATOR
        public const string WaveStartedToBeGenerated = "WAVE_STARTED_TO_BE_GENERATED";

        // UNCATEGORIZED
        public const string GenerateCoin = "GENERATE_COIN";
        
        public const string OpenBuildSpotMenu = "OPEN_BUILD_SPOT_MENU";
        public const string OpenTowerMenu = "OPEN_TOWER_MENU";
        public const string CloseTowerCatalog = "CLOSE_TOWER_CATALOG";

        public const string ClickBlockerClicked = "CLICK_BLOCKER_CLICKED";


        #endregion Custom Events

        // TODO : API, WEBSOCKET, ETC.. EVENTS 
    }
}