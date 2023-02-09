using Pathfinding;
using System;
using System.Collections;
using UnityEngine;

public class MySeeker : Seeker
{
    [Header("configurations")]
    [SerializeField]
    private LayerMask _groundLayer;

    public bool IsTargetInRange { get; private set; }

    public float TICK_PATH_CDW => 0.5f;

    public IEnumerator CheckIfTargetInRange(
        AvailableMovements availableMovements,
        GameObject currentObject,
        GameObject targetGameObject)
    {

        StartPath(currentObject.transform.position, targetGameObject.transform.position, (Path path) =>
        {
            IsTargetInRange = false;
            switch (availableMovements)
            {
                case AvailableMovements.Walk:
                    IsTargetInRange = CheckIfTargetIsInRangeIfYouWalk(path);
                    break;
                case AvailableMovements.Jump:
                    throw new NotImplementedException("You need to implement jump");
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

    private bool WillCollideWithGround(Vector2 position)
    {
        RaycastHit2D col = Physics2D.Linecast(position, new Vector2(position.x, position.y - 1f), _groundLayer);
        return col.collider != null;
    }

    public enum AvailableMovements
    {
        Walk,
        Jump,
        Fly
    }
}
