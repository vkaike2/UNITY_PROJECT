using System.Collections.Generic;

public partial class ThirdMap : BaseMap
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

public static class ThirdMapChanges
{
    public const int UNLOCK_STEP_1 = 1;
    public const int UNLOCK_STEP_2 = 2;
    public const int UNLOCK_STEP_3 = 3;
    public const int UNLOCK_STEP_4 = 4;
    public const int UNLOCK_STEP_5 = 5;
    public const int UNLOCK_STEP_6 = 6;
}