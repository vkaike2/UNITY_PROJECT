using System.Collections;
using UnityEngine;

public class WormAnimatorEvents : MonoBehaviour
{
    [Header("components")]
    [SerializeField]
    private Worm _worm;

    public void SetInitialBehaviour() => _worm.SetInitialBehaviour();
    public void CanMove() => _worm.CanMove = true;
    public void CanNotMove() => _worm.CanMove = false;
}
