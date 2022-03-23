using System;
using System.Collections.Generic;
using Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;
using Util;

namespace Data_Scripts
{
    [CreateAssetMenu(fileName = "Team", menuName = "Scriptable Objects/Team", order = 1)]
    public class TeamData : ScriptableObject
    {
        public Game.TeamEnum teamEnum;
        public Game.ControlEnum controlEnum;

        
    }
}
