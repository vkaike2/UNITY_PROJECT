using System.Collections.Generic;

public class MapInitial : Map
{
    private void Awake()
    {
        base.Awake();
        _startWithState = FiniteState.Wait;

        _finiteStates = new List<MapFiniteStateBase>()
        {
            new MapWaitState(),
        };
    }
}
