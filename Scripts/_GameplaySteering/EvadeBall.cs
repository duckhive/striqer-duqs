using _GameplaySteering;
using UnityEngine;

namespace _StrikerDucks._Gameplay._GameplaySteering
{
    public class EvadeBall : SteeringMono
    {
        [SerializeField] private float maxPrediction = 10.0f;
        [SerializeField] private float lookAhead = 15.0f;
        private float _prediction;
        private Vector3 _targetVelocity;
        
        protected override void Update()
        {
            destTarget.position = transform.position + new Vector3(0,0, lookAhead);
            var direction = transform.position - destTarget.position;
            var distance = direction.magnitude;
            var speed = agentRb.velocity.magnitude;

            if (speed <= distance / maxPrediction)
                _prediction = maxPrediction;
            else
                _prediction = distance / speed;

            _targetVelocity = ballRb.velocity;
            _targetVelocity.Normalize();
            destTarget.position += _targetVelocity * _prediction;
            base.Update();
        }
    }
}
