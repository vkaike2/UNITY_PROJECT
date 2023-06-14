using System;

[Serializable]
public class ChickenAtkWormBehaviourModel
{
    public Worm WormTarget { get; set; }
    public Action InteractWithWorm { get; set; }
    public Action EndAtkAnimation { get; set; }
}
