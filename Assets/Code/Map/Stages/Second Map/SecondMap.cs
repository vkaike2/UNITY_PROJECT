using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SecondMap : Map
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

public static class SecondMapChanges
{
    /// <summary>
    ///     Step (1)
    /// </summary>
    public const int UNLOCK_FIRST_STEP = 1;
    // Step (1) - 1
    public const int UNLOCK_FIRST_STEP_ONE = 2;
    // Step (1) - 2
    public const int UNLOCK_FIRST_STEP_TWO = 3;
    // Step (1) - 3 [moving to second step]
    public const int UNLOCK_FIRST_STEP_THREE = 4;
    public const int PLAYER_IS_MOVING_TO_SECOND_STEP = 5;
    // Step (1) - 4 
    public const int UNLOCK_FIRST_STEP_FOUR = 6;

    /// <summary>
    ///     Step (2)
    /// </summary>
    public const int UNLOCK_SECOND_STEP = 7;
    // Step (2) - 1
    public const int UNLOCK_SECOND_STEP_ONE = 8;
    // Step (2) - 2
    public const int UNLOCK_SECOND_STEP_TWO = 9;
    // Step (2) - 3
    public const int UNLOCK_SECOND_STEP_THREE = 10;

    /// <summary>
    ///     Step (3)
    /// </summary>
    public const int UNLOCK_THIRD_STEP = 11;
}
