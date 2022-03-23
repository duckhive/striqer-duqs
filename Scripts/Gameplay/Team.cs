using System;
using System.Collections.Generic;
using System.Linq;
using Data_Scripts;
using Rewired;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;
using Util;

namespace Gameplay
{
    public class Team : MonoBehaviour
    {
        
        public Game.TeamEnum teamEnum;
        public Game.ControlEnum controlEnum;

        //public AwayTeamData awayTeamData;
        //public HomeTeamData homeTeamData;
        
        
        [BoxGroup("User Control")] [Range(0,1)] public int playerId;

        [Sirenix.OdinInspector.ReadOnly] [BoxGroup("Current Switches")] public bool user;
        [Sirenix.OdinInspector.ReadOnly] [BoxGroup("Current Switches")] public bool passing;
        [Sirenix.OdinInspector.ReadOnly] [BoxGroup("Current Switches")] public bool shooting;
        [Sirenix.OdinInspector.ReadOnly] [BoxGroup("Current Switches")] public bool poss;
        
        [Sirenix.OdinInspector.ReadOnly] [BoxGroup("Current User")] public List<Agent> currentUser;

        public List<Agent> teamAgents;
        public ShotTarget shotTarget;

        private Player player;

        private GameManager gameManager;

        private void Awake()
        {
            gameManager = GetComponentInParent<GameManager>();
            if(GetComponentInParent<GameManager>().freeplay) return;
            
            InstantiateTeamAgents();
        }

        private void OnEnable()
        {
            if(GetComponentsInChildren<ShotTarget>() is not null)
                shotTarget = GetComponentInChildren<ShotTarget>();
        
            switch (controlEnum)
            {
                case Game.ControlEnum.Player1:
                    playerId = 0;
                    player = ReInput.players.GetPlayer(playerId);
                    Game.TeamUserBrain(this);
                    break;
                case Game.ControlEnum.Player2:
                    playerId = 1;
                    player = ReInput.players.GetPlayer(playerId);
                    Game.TeamUserBrain(this);
                    break;
                case Game.ControlEnum.CPU:
                    Game.TeamAgentBrain(this);
                    break;
                default:
                    Game.TeamAgentBrain(this);
                    break;
            }
        }

        private void Start()
        {
            if (!user) return;

            Game.SelectPlayerOnStart(this);
        }

        private void Update()
        {
            if (!user) return;
            
            if (player.GetButtonDown("Menu") && !gameManager.gamePaused)
                gameManager.PauseGame();
            else if (player.GetButtonDown("Menu") && gameManager.gamePaused)
                gameManager.ResumeGame();
            
            var xInput = player.GetAxis("Horizontal");
            var zInput = player.GetAxis("Vertical");
            
            if (!gameManager.gamePaused
                && !currentUser[0].canKick
                && xInput == 0
                && zInput == 0
                && player.GetButtonDown("Pass/Switch Player"))
                Game.SwitchPlayerClosestToBall(this, teamAgents);
    
            if (!gameManager.gamePaused
                && !currentUser[0].canKick
                && (xInput != 0 || zInput != 0)
                && player.GetButtonDown("Pass/Switch Player"))
                Game.SwitchPlayerInDirection(this, player, teamAgents);

            if(player.GetButtonDown("Ability"))
                currentUser[0].abilityData.Activate(currentUser[0].gameObject);
            
            if (!currentUser[0].canKick) return;

            if (!currentUser[0].attacking
                && currentUser[0].canKick
                && !currentUser[0].possCooldown)
            {
                Game.Pass(this, player, teamAgents);
                
                if(currentUser[0].canShoot)
                    Game.Shoot(this, player);
            }
                    
        }

        private void InstantiateTeamAgents()
        {
            if (teamEnum == Game.TeamEnum.Away)
            {
                var a1 = Instantiate(gameManager.awayTeamData.teamAgentsData[0].prefab, gameManager.awayTeamData.agent1StartPos, Quaternion.identity, transform);
                var a1Agent = a1.GetComponent<Agent>();
                teamAgents.Insert(0, a1Agent);
                a1Agent.index = Game.AgentIndex.Agent1;
                a1Agent.animIndex = 1;

                var a2 =  Instantiate(gameManager.awayTeamData.teamAgentsData[1].prefab, gameManager.awayTeamData.agent2StartPos, Quaternion.identity, transform);
                var a2Agent = a2.GetComponent<Agent>();
                teamAgents.Insert(1, a2Agent);
                a2Agent.index = Game.AgentIndex.Agent2;
                a2Agent.animIndex = 2;

                var a3 =  Instantiate(gameManager.awayTeamData.teamAgentsData[2].prefab, gameManager.awayTeamData.agent3StartPos, Quaternion.identity, transform);
                var a3Agent = a3.GetComponent<Agent>();
                teamAgents.Insert(2, a3Agent);
                a3Agent.index = Game.AgentIndex.Agent3;
                a3Agent.animIndex = 3;

                var a4 =  Instantiate(gameManager.awayTeamData.teamAgentsData[3].prefab, gameManager.awayTeamData.agent4StartPos, Quaternion.identity, transform);
                var a4Agent = a4.GetComponent<Agent>();
                teamAgents.Insert(3, a4Agent);
                a4Agent.index = Game.AgentIndex.Agent4;
                a4Agent.animIndex = 4;
            }
            if (teamEnum == Game.TeamEnum.Home)
            {
                var a5 = Instantiate(gameManager.homeTeamData.teamAgentsData[0].prefab, gameManager.homeTeamData.agent1StartPos, Quaternion.identity, transform);
                var a5Agent = a5.GetComponent<Agent>();
                teamAgents.Insert(0, a5Agent);
                a5Agent.index = Game.AgentIndex.Agent5;
                a5Agent.animIndex = 5;
            
                var a6 =  Instantiate(gameManager.homeTeamData.teamAgentsData[1].prefab, gameManager.homeTeamData.agent2StartPos, Quaternion.identity, transform);
                var a6Agent = a6.GetComponent<Agent>();
                teamAgents.Insert(1, a6Agent);
                a6Agent.index = Game.AgentIndex.Agent6;
                a6Agent.animIndex = 6;
            
                var a7 =  Instantiate(gameManager.homeTeamData.teamAgentsData[2].prefab, gameManager.homeTeamData.agent3StartPos, Quaternion.identity, transform);
                var a7Agent = a7.GetComponent<Agent>();
                teamAgents.Insert(2, a7Agent);
                a7Agent.index = Game.AgentIndex.Agent7;
                a7Agent.animIndex = 7;
            
                var a8 =  Instantiate(gameManager.homeTeamData.teamAgentsData[3].prefab, gameManager.homeTeamData.agent4StartPos, Quaternion.identity, transform);
                var a8Agent = a8.GetComponent<Agent>();
                teamAgents.Insert(3, a8Agent);
                a8Agent.index = Game.AgentIndex.Agent8;
                a8Agent.animIndex = 8;
            }
        }
    }
}
