using System.Collections.Generic;

public partial class FirstMap : Map
{
    /// <summary>
    ///     OnChangeMapEvent
    ///     Map           -> 1 - Unlock First Platform
    ///     Map           -> 2 - Wall of spike move
    ///     Map           -> 2 - Camera Move to Auxiliar 01
    ///     Wall of Spike -> 3 - Called when Wall of Spike reached the Medium position
    /// </summary>

    protected override void BeforeStart()
    {
        _finiteStates = new List<MapFiniteStateBase>()
        {
            new Idle(),
            new InternalCombat(),
            new Wait()
        };
    }
}

public static class FirstMapChanges
{
    /// <summary>
    ///  Small Stage
    /// </summary>
    public const int SMALL_STAGE_UNLOCK_PLATFORM = 1;
    public const int WALL_OF_SPIKE_MOVE_TO_MEDIUM = 2;
    public const int WALL_OF_SPIKE_READY_MEDIUM = 3;

    /// <summary>
    ///  Medium Stage
    /// </summary>
    public const int MEDIUM_STAGE_UNLOCK_TWO_SMALL_PLATFORMS = 4;
    public const int MEDIUM_STAGE_UNLOCK_BIG_PLATFORM = 5;
    public const int WALL_OF_SPIKE_MOVE_TO_LARGE = 6;
    public const int WALL_OF_SPIKE_READY_LARGE = 7;
    
    /// <summary>
    ///  Large Stage
    /// </summary>
    public const int LARGE_STAGE_UNLOCK_TWO_SMAL_PLATFORMS = 8;
    public const int LARGE_STAGE_UNLOCK_MIDDLE_PLATFORMS = 9;
    public const int LARGE_STAGE_UNLOCK_SMALLER_PLATFORMS = 10;
    public const int LARGE_STAGE_UNLOCK_BIG_PLATFORMS = 11;

    /// <summary>
    ///  Boss Stage
    /// </summary>
    public const int PREPARE_MAP_TO_BOSS = 10;
}