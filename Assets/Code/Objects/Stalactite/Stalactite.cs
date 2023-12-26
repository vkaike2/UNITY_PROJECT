using System;
using System.Collections;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private CdwIndicationUI _cdwIndication;


    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _raycastLenght;
    [SerializeField]
    private float _cdwToBeCompleteAgain = 2f;
    [SerializeField]
    private LayerMask _playerLayer;

    [Header("FALL")]
    [SerializeField]
    private StalactiteProjectile _projectile;
    [SerializeField]
    private Transform _spawnPosition;
    [SerializeField]
    private float _cdwToFallAgain;
    [SerializeField]
    private float _damage;

    private Animator _animator;

    private const string ANIMATION_STRING_INCOMPLETE = "Incomplete";
    private const string ANIMATION_STRING_COMPLETE = "Complete";

    private bool _canAttack = true;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(this.transform.position,
            new Vector3(this.transform.position.x, this.transform.position.y - _raycastLenght, this.transform.position.z));
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!_canAttack || !CheckIfPlayerIsUnderRaycast())
        {
            return;
        }

        _canAttack = false;
        StartCoroutine(Fall());
    }

    private bool CheckIfPlayerIsUnderRaycast()
    {
        RaycastHit2D hit = Physics2D.Linecast(
            this.transform.position,
             new Vector3(this.transform.position.x, this.transform.position.y - _raycastLenght, this.transform.position.z),
             _playerLayer);

        return hit.collider != null;
    }

    private IEnumerator Fall()
    {
        StartCoroutine(CalculateCdw());
        _animator.Play(ANIMATION_STRING_INCOMPLETE);

        StalactiteProjectile projectile = Instantiate(_projectile, _spawnPosition.position, Quaternion.identity);
        projectile.transform.parent = null;
        projectile.SetInitialValues(_damage);

        yield return new WaitForSeconds(_cdwToBeCompleteAgain);
        _animator.Play(ANIMATION_STRING_COMPLETE);
    }

    private IEnumerator CalculateCdw()
    {
        _cdwIndication.StartCdw(_cdwToFallAgain);
        yield return new WaitForSeconds(_cdwToFallAgain);
        _canAttack = true;
    }

}