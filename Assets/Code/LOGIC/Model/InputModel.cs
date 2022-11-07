using System;

public class InputModel<T>
{
    public T Value { get; set; }

    public Action Performed { get; set; }
    public Action Started { get; set; }
    public Action Canceled { get; set; }

    public void ClearActions()
    {
        Performed = () => { };
        Started = () => { };
        Canceled = () => { };
    }
}
