using System.Collections.Generic;
using Audio.DATA;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data_Scripts
{
    [CreateAssetMenu(fileName = "Game", menuName = "Scriptable Objects/Game")]
    public class GameData : ScriptableObject
    {
        [BoxGroup("Match Settings")] public float matchLength = 300;

        [BoxGroup("All Agents")] public List<AgentData> globalAgents;
        
        public SoundFXData crowdCheerOnScoreSfx;
        public SoundFXData crowdIdleSfx;
    }
}
