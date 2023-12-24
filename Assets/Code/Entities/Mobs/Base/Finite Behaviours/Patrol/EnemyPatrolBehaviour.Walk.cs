using System.Collections.Generic;
using UnityEngine;

public partial class EnemyPatrolBehaviour
{
    public class Walk : EnemyBaseBehaviour
    {
        private readonly EnemyWalkModel _model;
        private float _directionMultiplier = 1;

        public Walk(EnemyWalkModel walkModel)
        {
            _model = walkModel;
        }

         public override void Start(Enemy enemy)
        {
            base.Start(enemy);
        }

        public override void OnEnterBehaviour()
        {
            GetRandomDirection();
            _model.OnChangeAnimationToWalk.Invoke();
        }

        public override void OnExitBehaviour()
        {
        }

        public override void Update()
        {
             if (CheckIfNeedToChangeDirection())
            {
                ChangeDirection();
                return;
            }
            _rigidbody2D.velocity = new Vector2(_enemy.Status.MovementSpeed.Get() * _directionMultiplier, _rigidbody2D.velocity.y);
        }

         private void ChangeDirection()
        {
            _directionMultiplier *= -1;
            _enemy.RotationalTransform.localScale = new Vector3(_directionMultiplier, 1, 1);
        }

        private bool CheckIfNeedToChangeDirection()
        {
            Collider2D wall = _model.WillHitTheWallCheck.DrawPhysics2D(_model.LayerMask);
            if (wall != null) return true;

            Collider2D ground = _model.WillHitTheGround.DrawPhysics2D(_model.LayerMask);
            return ground == null;
        }

         private void GetRandomDirection()
        {
            List<float> directions = new List<float>() { 1, -1 };
            _directionMultiplier = directions[Random.Range(0, 2)];

            _enemy.RotationalTransform.localScale = new Vector3(_directionMultiplier, 1, 1);
        }

    }
}