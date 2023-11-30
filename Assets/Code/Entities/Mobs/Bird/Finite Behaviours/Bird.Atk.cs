using System.Collections;
using UnityEngine;


public partial class Bird : Enemy
{
    private class Atk : BirdBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Atk;
        private Vector2? _direction;
        private Action _action;

        private BirdAtkModel _model;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _model = _bird.AtkModel;
        }

        public override void OnEnterBehaviour()
        {
            _action = Action.GoingTowardsPlayer;
            _bird.BirdAnimator.PlayAnimation(BridAnimatorModel.AnimationName.Bird_Atk);
            _direction = (_model.TargetPosition - (Vector2)_bird.transform.position).normalized;

            _bird.PatrolModel.OnFlappingWings.AddListener(OnFlappingWings);
        }


        public override void OnExitBehaviour()
        {
            _direction = null;
            _bird.PatrolModel.OnFlappingWings.RemoveListener(OnFlappingWings);
        }

        public override void Update()
        {
            BirdGoingTowardsPlayer();

            BirdGoingBackUp();
        }

        private void BirdGoingTowardsPlayer()
        {
            if (_action != Action.GoingTowardsPlayer) return;
            if (_direction == null) return;


            if (Vector2.Distance(_model.TargetPosition, _bird.transform.position) < 1F)
            {
                _action = Action.GoingBackUp;
                return;
            }

            if (_model.TargetPosition.x > _bird.transform.position.x)
            {
                _bird.RotationalTransform.localScale = new Vector3(1, 1, 1);
            }
            else if (_model.TargetPosition.x < _bird.transform.position.x)
            {
                _bird.RotationalTransform.localScale = new Vector3(-1, 1, 1);
            }


            _rigidbody2D.velocity = _direction.GetValueOrDefault() * _model.AtkFlyingSpeed;
        }

        private void BirdGoingBackUp()
        {
            if (_action != Action.GoingBackUp) return;

            _bird.BirdAnimator.PlayAnimation(BridAnimatorModel.AnimationName.Bird_Fly);

            Vector2 myHorizontalPosition = new Vector2(0, _bird.transform.position.y);
            Vector2 initialPosition = new Vector2(0, _bird.PatrolModel.InitialHorizontalPosition);

            if (Vector2.Distance(myHorizontalPosition, initialPosition) <= 0.2f)
            {
                _bird.ChangeBehaviour(Behaviour.Patrol);
            }
        }

        private void OnFlappingWings()
        {
            _rigidbody2D.velocity = new Vector2(0, _bird.AtkModel.MovementSpeedToGoUp);
        }


        private enum Action
        {
            GoingTowardsPlayer,
            GoingBackUp
        }
    }
}
