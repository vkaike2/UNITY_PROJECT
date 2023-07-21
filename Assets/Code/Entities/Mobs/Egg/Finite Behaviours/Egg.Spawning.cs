using UnityEngine;

public partial class Egg : Enemy
{
    private class Spawning : EggBaseBehaviour
    {
        public override Behaviour Behaviour => Egg.Behaviour.Spawning;

        public override void OnEnterBehaviour()
        {
            Enemy enemy = GameObject.Instantiate(_egg.SpawningModel.SpawnedEnemy, _egg.transform.position, Quaternion.Euler(0f, 0f, 0f));
            enemy.transform.parent = null;
            GameObject.Destroy(_egg.gameObject);
        }

        public override void OnExitBehaviour()
        {
        }

        public override void Update()
        {
        }
    }

}
