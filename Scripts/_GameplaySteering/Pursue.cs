using _GameplaySteering;
using UnityEngine;

namespace _StrikerDucks._Gameplay._GameplaySteering
{
    public class Pursue : SteeringMono
    {
        [SerializeField] protected float maxPredict = 5.0f;
        protected Transform goal;

        protected override void Update()
        {
            destTarget.position = goal.position;
            var direction = destTarget.position - transform.position;
            var distance = direction.magnitude;
            var speed = agentRb.velocity.magnitude;
            
            float prediction;

            if (speed <= distance / maxPredict)
                prediction = maxPredict;
            else
                prediction = distance / speed;

            var velocity = ballRb.velocity;
            velocity.Normalize();
            destTarget.position += velocity * prediction;
            aiDestinationSetter.target = destTarget;
        }
    }
}
