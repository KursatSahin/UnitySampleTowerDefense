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

        // TIMER
        public const string TimeTickUpdated = "TIME_TICK_UPDATED";
        
        // HUD
        public const string UpdateMoney = "UPDATE_MONEY";
        public const string UpdateRemainingLife = "UPDATE_REMAINING_LIFE";
        public const string EnemyKilled = "ENEMY_KILLED";
        
        // GAME MANAGER
        public const string GameIsOver = "GAME_IS_OVER";
        public const string WonGame = "WON_GAME";

        // WAVE GENERATOR
        public const string WaveStartedToBeGenerated = "WAVE_STARTED_TO_BE_GENERATED";

        // UNCATEGORIZED
        public const string GenerateCoin = "GENERATE_COIN";
        
        public const string OpenTowerBuildCatalog = "OPEN_TOWER_BUILD_CATALOG";
        public const string OpenTowerMaintenanceCatalog = "OPEN_TOWER_MAINTENANCE_CATALOG";
        public const string CloseTowerCatalog = "CLOSE_TOWER_CATALOG";

        public const string ClickOutsideOfInteractiveArea = "CLICK_OUTSIDE_OF_INTERACTIVE_AREA";


        #endregion Custom Events

        // TODO : API, WEBSOCKET, ETC.. EVENTS 
    }
}