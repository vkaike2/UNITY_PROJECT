using System.Collections.Generic;
using System.Threading;

public partial class FirstMap : BaseMap
{
    protected override void BeforeStart()
    {
        _finiteStates = new List<MapFiniteStateBase>()
        {
            new Idle(),
            new Combat(),
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
    public const int PREPARE_MAP_TO_BOSS = 12;
    public const int WALL_OF_SPIKE_READY_BOSS = 13;
    public const int CAMERA_READY_FOR_BOSS = 14;
}