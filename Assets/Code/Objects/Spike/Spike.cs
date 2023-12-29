using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SpikeDamageDealer _damageDealer;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private bool _isActive;
    [SerializeField]
    private float _warningCdw = 3f;


    private const string ANIMATION_ACTIVE = "Spike_Active";
    private const string ANIMATION_DEACTIVE = "Spike_Deactive";
    private const string ANIMATION_WARNING = "Spike_Warning";

    public void Activate(bool value)
    {
        _isActive = value;
        UpdateActivationState();
    }

    private void UpdateActivationState()
    {
        if (_isActive)
        {
            StartCoroutine(ActivateSpike());
            return;
        }

        _animator.Play(ANIMATION_DEACTIVE);
        _damageDealer.enabled = false;
    }

    private IEnumerator ActivateSpike()
    {
        _animator.Play(ANIMATION_WARNING);
        yield return new WaitForSeconds(_warningCdw);
        _animator.Play(ANIMATION_ACTIVE);
        _damageDealer.enabled = true;
    }


}