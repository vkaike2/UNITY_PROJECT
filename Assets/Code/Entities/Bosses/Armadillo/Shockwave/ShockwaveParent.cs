using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class ShockwaveParent : MonoBehaviour
{
    [Header("PREFAB")]
    [SerializeField]
    private Shockwave _shockwavePrefab;

    [Header("CONFIGURATION")]
    [SerializeField]
    private float _distanceBetweenShockwave = 0.3f;
    [SerializeField]
    private float _cdwBetweenSpawn = 1f;
    [SerializeField]
    private LayerMask _layerToStop;

    private ArmadilloDamageDealer _armadilloDamageDealer;

    public void SpawnShockwave(Direction direction, ArmadilloDamageDealer armadilloDamageDealer)
    {
        _armadilloDamageDealer = armadilloDamageDealer;
        StartCoroutine(SpawnShockwaveAction(direction));
    }

    private IEnumerator SpawnShockwaveAction(Direction direction)
    {
        float deltaDirection = direction == Direction.Left ? -_distanceBetweenShockwave : +_distanceBetweenShockwave;

        int shockwaveCount = 0;

        Vector2 spawnPosition = this.transform.position;

        while (CheckIfWillCollideWithLayer(new Vector2(spawnPosition.x + deltaDirection, spawnPosition.y)))
        {
            Shockwave shockwave = Instantiate(_shockwavePrefab, spawnPosition, quaternion.identity);
            _armadilloDamageDealer.OnRegisterShockwave.Invoke(shockwave);

            yield return new WaitForSeconds(_cdwBetweenSpawn);

            spawnPosition.x += deltaDirection;
            shockwaveCount++;
        }
    }

    private bool CheckIfWillCollideWithLayer(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            new Vector2(position.x, position.y),
            Vector2.down,
            10,
            _layerToStop);

        return hit.collider != null;
    }

    public enum Direction
    {
        Left,
        Right
    }
}