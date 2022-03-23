using System;
using System.Collections.Generic;
using Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;
using Util;

namespace Data_Scripts
{
    [CreateAssetMenu(fileName = "Home Team", menuName = "Scriptable Objects/Home Team", order = 1)]
    public class HomeTeamData : ScriptableObject
    {
        [BoxGroup("Players")] public List<AgentData> teamAgentsData = new List<AgentData>();

        public int score;
        
        public Vector3 agent1StartPos = new Vector3(25, 0, 25);
        public Vector3 agent2StartPos = new Vector3(25, 0, -25);
        public Vector3 agent3StartPos = new Vector3(75, 0, 25);
        public Vector3 agent4StartPos = new Vector3(75, 0, -25);

        private void OnEnable()
        {
            score = 0;
        }
    }
}