using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FlyingFish : Enemy
{
    private class Walk : FlyingFishBaseBehaviour
    {
        public override Behaviour Behaviour => FlyingFish.Behaviour.Walk;

        private Vector2 _direction;
        private List<Vector2> _randomDirections = new List<Vector2>()
        {
            new Vector2(-1, 0),
            new Vector2(1, 0),
        };

        public override void OnEnterBehaviour()
        {
            _walkModel.OnStartMoving.AddListener(StartMoving);

            GetRandomDirection();
            UpdateDirection(_direction);
            _flyingFish.Animator.PlayAnimation(FlyingFishAnimatorModel.AnimationName.Flying_Fish_Walk);
        }

        public override void OnExitBehaviour()
        {
            _direction = Vector2.zero;
            _walkModel.OnStartMoving.RemoveListener(StartMoving);
        }

        public override void Update()
        {

        }

        private void GetRandomDirection()
        {
            _direction = _randomDirections[UnityEngine.Random.Range(0, _randomDirections.Count)];
        }

        private void StartMoving()
        {
            _flyingFish.StartCoroutine(ManageMovement());
        }

        public IEnumerator ManageMovement()
        {
            Vector2 initalVelocity = _direction * _walkModel.InitialVelocity;
            SetVelocity(initalVelocity);
            Vector2 currentVelocity;

            float cdw = 0;
            while (cdw < _walkModel.MovementDuration)
            {
                cdw += Time.deltaTime;

                currentVelocity = new Vector2(
                    Mathf.Lerp((_direction * _walkModel.InitialVelocity).x, 0, cdw / _walkModel.MovementDuration),
                    0);

                _rigidbody2D.velocity = currentVelocity;

                if (!_flyingFish.WaterSection.ImUnderWater(_flyingFish.transform.position))
                {
                    InvertVelocity();
                }

                yield return new WaitForFixedUpdate();
            }

            _rigidbody2D.velocity = Vector2.zero;

            _flyingFish.ChangeBehaviour(Behaviour.Idle);
        }



        private void InvertVelocity()
        {
            SetVelocity(-_rigidbody2D.velocity);
            _direction = -_direction;
        }
    }
}