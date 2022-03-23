using _StrikerDucks._Gameplay._GameplaySteering;
using Gameplay;
using Pathfinding;
using UnityEngine;
using DestinationTarget = Gameplay.DestinationTarget;


// ReSharper disable once InconsistentNaming
    namespace _GameplaySteering
    {
        public class SteeringMono : MonoBehaviour
        {
            protected Agent agent;
            protected Rigidbody agentRb;
            protected Ball ball;
            protected Rigidbody ballRb;
            public AIDestinationSetter aiDestinationSetter;
            protected AIPath aStarAgent;
            protected Transform destTarget;

            protected virtual void Awake()
            {
                aiDestinationSetter = GetComponent<AIDestinationSetter>();
                aStarAgent = GetComponent<AIPath>();
                destTarget = GetComponentInChildren<DestinationTarget>().transform;
            }

            protected virtual void Update()
            {
            
            }

            private void OnDrawGizmos()
            {
                if (destTarget == null) return;
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(destTarget.transform.position, 1);
            }
        }
    }

