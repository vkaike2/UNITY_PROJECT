using System.Collections;
using UnityEngine;


public class WallOfSpikeAnimatiorHelper : MonoBehaviour
{
    [SerializeField]
    private Transform _upperPartTransoform;

    public void MoveUpperPartVertically(float value)
    {
        _upperPartTransoform.localPosition = new Vector3(_upperPartTransoform.localPosition.x, value, _upperPartTransoform.localPosition.y);
    }
}
