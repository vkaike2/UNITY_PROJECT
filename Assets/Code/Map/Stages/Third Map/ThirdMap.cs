using System.Collections.Generic;

public partial class ThirdMap : Map
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