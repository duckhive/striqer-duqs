using _GameplaySteering;
using UnityEngine;

namespace _StrikerDucks._Gameplay._GameplaySteering
{
    public class Wander : SteeringMono
    {
        [SerializeField] private int wanderAmount = 10;
        [SerializeField] private float wanderRate = 2.0f;
        [SerializeField] private float wanderRadius = 5.0f;
        
        protected override void Update()
        {
            var wanderOffset = transform.forward * wanderAmount;
            Vector3 targetPos;
            targetPos = wanderOffset +
                                  new Vector3
                                  (Random.Range(-1.0f, 1.0f) * wanderRate, 
                                      0, 
                                      Random.Range(-1.0f, 1.0f) * wanderRate);
            targetPos *= wanderRadius;
            var targetLocal = targetPos;
            var targetWorld = gameObject.transform.InverseTransformVector(targetLocal);
            destTarget.position = targetLocal;
        }
    }
}
