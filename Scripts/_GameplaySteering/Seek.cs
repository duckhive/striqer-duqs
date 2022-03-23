using _GameplaySteering;
using UnityEngine;

namespace _StrikerDucks._Gameplay._GameplaySteering
{
    public class Seek : SteeringMono
    {
        protected Transform goal;
        protected override void Update()
        {
            destTarget.position = goal.position;
        }
    }
}
