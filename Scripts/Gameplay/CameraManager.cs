using System;
using UnityEngine;
using Util;
using Cinemachine;
using Sirenix.OdinInspector;
using System.Linq;
using Data_Scripts;

namespace Gameplay
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager instance;

        public Team awayTeam;
        public Team homeTeam;
        
        [HideInInspector] public Animator anim;

        #region TARGET GROUPS
        
        [BoxGroup("CM Target Groups")] public CinemachineTargetGroup targetGroup1;
        [BoxGroup("CM Target Groups")] public CinemachineTargetGroup targetGroup2;
        [BoxGroup("CM Target Groups")] public CinemachineTargetGroup targetGroup3;
        [BoxGroup("CM Target Groups")] public CinemachineTargetGroup targetGroup4;
        [BoxGroup("CM Target Groups")] public CinemachineTargetGroup targetGroup5;
        [BoxGroup("CM Target Groups")] public CinemachineTargetGroup targetGroup6;
        [BoxGroup("CM Target Groups")] public CinemachineTargetGroup targetGroup7;
        [BoxGroup("CM Target Groups")] public CinemachineTargetGroup targetGroup8;

        #endregion
        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
            
            anim = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if (FindObjectOfType<Team>() is not null)
            {
                awayTeam = FindObjectsOfType<Team>()
                    .FirstOrDefault(t => t.teamEnum == Game.TeamEnum.Away);
                homeTeam = FindObjectsOfType<Team>()
                    .FirstOrDefault(t => t.teamEnum == Game.TeamEnum.Home);
            }
        }

        private void Start()
        {
            AssignCameraTargets();
        }

        private void AssignCameraTargets()
        {
            if(awayTeam.teamAgents.Count > 0)
                targetGroup1.m_Targets[0].target = awayTeam.teamAgents[0].transform;
            
            if(awayTeam.teamAgents.Count > 1)
                targetGroup2.m_Targets[0].target = awayTeam.teamAgents[1].transform;
            
            if(awayTeam.teamAgents.Count > 2)
                targetGroup3.m_Targets[0].target = awayTeam.teamAgents[2].transform;
            
            if(awayTeam.teamAgents.Count > 3)
                targetGroup4.m_Targets[0].target = awayTeam.teamAgents[3].transform;
            
            if(homeTeam.teamAgents.Count > 0)
                targetGroup5.m_Targets[0].target = homeTeam.teamAgents[0].transform;
            
            if(homeTeam.teamAgents.Count > 1)
                targetGroup6.m_Targets[0].target = homeTeam.teamAgents[1].transform;
            
            if(homeTeam.teamAgents.Count > 2)
                targetGroup7.m_Targets[0].target = homeTeam.teamAgents[2].transform;
            
            if(homeTeam.teamAgents.Count > 3)
                targetGroup8.m_Targets[0].target = homeTeam.teamAgents[3].transform;     
        }
    }
}
