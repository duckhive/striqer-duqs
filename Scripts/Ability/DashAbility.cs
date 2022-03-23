using Gameplay;
using UnityEngine;

namespace Ability
{
    [CreateAssetMenu]
    public class DashAbility : Ability
    {
        public float speedMultiplier;

        public override void Activate(GameObject parent)
        {
            Agent agent = parent.GetComponent<Agent>();
            agent.agentData.maxSpeed *= speedMultiplier;
        }

        public override void BeginCoolDown(GameObject parent)
        {
            Agent agent = parent.GetComponent<Agent>();
            agent.agentData.maxSpeed /= speedMultiplier;
        }
    }
}
