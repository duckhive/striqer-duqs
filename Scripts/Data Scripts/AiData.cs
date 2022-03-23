using Gameplay;
using UnityEngine;

namespace Data_Scripts
{
    [CreateAssetMenu(fileName = "New Ai Steering", menuName = "Scriptable Objects/Ai Steering")]
    public class AiData : ScriptableObject
    {
        public float maxPredict;
        
        public void Tick(Agent agent)
        {
            if (!agent.team.poss && !agent.canKick)
                Steering.PursueBall(agent, maxPredict);
            else
                Steering.DoNothing(agent);

        }
    }
}
