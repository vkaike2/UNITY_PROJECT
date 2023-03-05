using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MySeeker : Seeker
{
    [Header("configurations")]
    [SerializeField]
    private LayerMask _groundLayer;

    public bool IsTargetInRange { get; private set; }

    public float TICK_PATH_CDW => 0.5f;

    //private Vector2? _firstPositionAir = null;
    //private Vector2? _lastPositionAir = null;
    //private Vector2? _nextCurrentPosition = null;
    private JumpModel _jumpModel = new JumpModel();
    private List<Vector3> _pathTest = null;

    private void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;

        //if (_firstPositionAir != null)
        //{
        //    Gizmos.DrawSphere(_firstPositionAir.Value, 0.5f);
        //}

        //if (_lastPositionAir != null)
        //{
        //    Gizmos.DrawSphere(_lastPositionAir.Value, 0.5f);
        //}

        //if (_pathTest != null && _pathTest.Any())
        //{
        //    foreach (var item in _pathTest)
        //    {
        //        Gizmos.DrawSphere(item, 0.2f);
        //    }
        //}
    }

    public IEnumerator CheckIfTargetInRange(
        AvailableMovements availableMovements,
        GameObject currentObject,
        GameObject targetGameObject,
        float maximumVeticalDistance = 0)
    {

        StartPath(_jumpModel.NextCurrentPosition == null ? currentObject.transform.position : _jumpModel.NextCurrentPosition.Value, targetGameObject.transform.position, (Path path) =>
        {
            IsTargetInRange = false;
            switch (availableMovements)
            {
                case AvailableMovements.Walk:
                    IsTargetInRange = CheckIfTargetIsInRangeIfYouWalk(path);
                    break;
                case AvailableMovements.Jump:
                    IsTargetInRange = CheckIfTargetIsInRageIfYouJump(path);
                    break;
                case AvailableMovements.Fly:
                    throw new NotImplementedException("You need to implement fly");
            }
        });

        while (!IsDone())
        {
            yield return new WaitForFixedUpdate();
        }

        if (!IsTargetInRange)
        {
            yield return new WaitForSeconds(TICK_PATH_CDW);

            StartCoroutine(CheckIfTargetInRange(availableMovements, currentObject, targetGameObject));
        }
    }

    /// <summary>
    /// true  => Is in range 
    /// false => Isn't in range
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public bool CheckIfTargetIsInRangeIfYouWalk(Path path)
    {
        foreach (var pathPosition in path.vectorPath)
        {
            if (WillCollideWithGround(pathPosition)) continue;

            return false;
        }
        return true;
    }

    /// <summary>
    /// true  => Is in range 
    /// false => Isn't in range
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public bool CheckIfTargetIsInRageIfYouJump(Path path)
    {
        _jumpModel = new JumpModel();
        _pathTest = path.vectorPath.ToList();

        for (int i = 0; i < path.vectorPath.Count; i++)
        {
            if (WillCollideWithGround(path.vectorPath[i]))
            {
                //Debug.Log(path.vectorPath[i]);
                if (_jumpModel.FirstPositionAir != null && _jumpModel.LastPositionAir == null)
                {
                    _jumpModel.LastPositionAir = path.vectorPath[i];
                }
            }
            else
            {
                if (_jumpModel.FirstPositionAir == null)
                {
                    _jumpModel.FirstPositionAir = path.vectorPath[i];
                }
            }

            if (_jumpModel.FirstPositionAir != null && _jumpModel.LastPositionAir != null)
            {
                float yDirection = _jumpModel.LastPositionAir.Value.y - _jumpModel.FirstPositionAir.Value.y;

                _jumpModel.NextCurrentPosition = _jumpModel.LastPositionAir;
                return false;
                //break;
            }
        }

        //return false;
        return true;
    }

    private bool WillCollideWithGround(Vector2 position)
    {
        RaycastHit2D col = Physics2D.Linecast(position, new Vector2(position.x, position.y - 0.5f), _groundLayer);
        return col.collider != null;
    }

    public class JumpModel
    {
        public Vector2? FirstPositionAir { get; set; }
        public Vector2? LastPositionAir { get; set; }
        public Vector2? NextCurrentPosition { get; set; }
    }

    public enum AvailableMovements
    {
        Walk,
        Jump,
        Fly
    }
}
