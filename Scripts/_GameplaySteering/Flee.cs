using _GameplaySteering;
using UnityEngine;

namespace _StrikerDucks._Gameplay._GameplaySteering
{
    public class Flee : SteeringMono
    {
        [SerializeField] protected float fleeDistance = 5.0f;
        protected Vector3 Linear;
        protected override void Update()
        {
            base.Update();
            Linear = transform.position - destTarget.position;
            Linear.Normalize();
            Linear *= aStarAgent.maxSpeed * fleeDistance;
            destTarget.position = Linear;
        }
    }
}
