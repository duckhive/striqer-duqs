using System;
using System.Collections;
using System.Linq;
using Ability;
using Data_Scripts;
using Pathfinding;
using Rewired;
using Sirenix.OdinInspector;
using UnityEngine;
using Util;

namespace Gameplay
{
    public class Agent : MonoBehaviour
    {
        [Required] [BoxGroup("Data")] public AgentData agentData;

        [BoxGroup("Ability System")] public AbilityData abilityData;
        [BoxGroup("Ability System")] public float abilityCooldownTime;
        [BoxGroup("Ability System")] public float abilityActiveTime;
        [BoxGroup("Ability System")] public Game.AbilityState abilityState = Game.AbilityState.Ready;

        [BoxGroup("Particle Systems")] public ParticleSystem leftFootFX;
        [BoxGroup("Particle Systems")] public ParticleSystem rightFootFX;
        [BoxGroup("Particle Systems")] public ParticleSystem attackVfx;
        [BoxGroup("Particle Systems")] public ParticleSystem hitFX;

        [ReadOnly] [BoxGroup("Current Speeds")] public float turnSpeedCurrent;
        [ReadOnly] [BoxGroup("Current Speeds")] public float verticalSpeed = 0;
        [ReadOnly] [BoxGroup("Current Speeds")] public float gravity = 50f;
        [ReadOnly] [BoxGroup("Current Speeds")] public float speedCurrent;
        [ReadOnly] [BoxGroup("Current Forces")] public float passForceCurrent;
        [ReadOnly] [BoxGroup("Current Forces")] public float shotForceCurrent;
        
        [ReadOnly] [BoxGroup("Current Switches")] public bool user;
        [ReadOnly] [BoxGroup("Current Switches")] public bool poss;
        [ReadOnly] [BoxGroup("Current Switches")] public bool canKick;
        [ReadOnly] [BoxGroup("Current Switches")] public bool canShoot;
        [ReadOnly] [BoxGroup("Current Switches")] public bool possCooldown;
        [ReadOnly] [BoxGroup("Current Switches")] public bool moving;
        [ReadOnly] [BoxGroup("Current Switches")] public bool turbo;
        [ReadOnly] [BoxGroup("Current Switches")] public bool attacking;
        [ReadOnly] [BoxGroup("Current Switches")] public bool shooting;
        [ReadOnly] [BoxGroup("Current Switches")] public bool covered;
        [ReadOnly] [BoxGroup("Current Switches")] public bool passing;
        [ReadOnly] [BoxGroup("Current Switches")] public bool recPass;

        [HideInInspector] public float speedReset;
        [HideInInspector] public float shotForceReset;
        [HideInInspector] public float shotSpinReset;
        [HideInInspector] public float passForceReset;
        [HideInInspector] public Team team;
        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public Vector3 startPos;
        [HideInInspector] public Quaternion startRot;
        [HideInInspector] public Transform ballPos;
        [HideInInspector] public Transform airPassTarget;
        [HideInInspector] public Animator anim;
        [HideInInspector] public SkinnedMeshRenderer rend;
        [HideInInspector] public float shotTimer;
        [HideInInspector] public float passTimer;
        [HideInInspector] public Game.AgentIndex index;
        [HideInInspector] public int animIndex;
        [HideInInspector] public AIDestinationSetter destSetter;
        [HideInInspector] public AIPath aiPath;
        [HideInInspector] public Transform goal;
        
        private static readonly int Attacking = Animator.StringToHash("sliding");
        private static readonly int Moving = Animator.StringToHash("moving");
        
        private Player player;
        private CharacterController controller;

        private void Awake()
        {
            anim = GetComponent<Animator>(); // require
            rb = GetComponent<Rigidbody>(); // require
            ballPos = GetComponentInChildren<BallPosition>().transform;
            airPassTarget = GetComponentInChildren<AirPassTarget>().transform;
            controller = GetComponent<CharacterController>(); // require
            destSetter = GetComponent<AIDestinationSetter>();
            goal = GetComponentInChildren<DestinationTarget>().transform;
            aiPath = GetComponent<AIPath>();

            if (GetComponentsInChildren
                    <SkinnedMeshRenderer>()
                .FirstOrDefault(t => t.CompareTag("Player Mesh")) is not null)
                rend = GetComponentsInChildren<SkinnedMeshRenderer>().FirstOrDefault(t => t.CompareTag("Player Mesh"));
            else
                Debug.LogWarning("Agent Mesh Renderer not assigned.  Check to make sure the mesh was assigned player mesh tag");
            
            if(GetComponentInParent<Team>() is not null)
                team = GetComponentInParent<Team>();
            else
                Debug.LogWarning("Team not assigned to agent.  Agent must be a child of the Team GameObject.");
            
            if (GetComponentInParent<GameManager>().freeplay)
            {
                index = Game.AgentIndex.Agent1;
                animIndex = 1;
                team.teamAgents.Insert(0, this);
              
            }
        }
        
        private void Start()
        {
            aiPath.maxSpeed = agentData.maxSpeed;
            aiPath.maxAcceleration = agentData.accel;
            speedReset = agentData.maxSpeed;
            speedCurrent = 0;
            passForceReset = agentData.maxPassForce * 0.5f;
            passForceCurrent = passForceReset;
            shotForceReset = agentData.maxShotForce * 0.5f;
            shotForceCurrent = shotForceReset;
            var transform1 = transform;
            startPos = transform1.position;
            startRot = transform1.rotation;
            destSetter.target = goal;

            if (team is not null)
                player = ReInput.players.GetPlayer(team.playerId);
            else
                player = ReInput.players.GetPlayer(0);
            
        }

        private void Update()
        {
            switch (user)
            {
                case true:
                    // turbo:
                    if(!canKick 
                       && player.GetAxis("Turbo") > 0)
                        Game.TurboSpeed(this);
                    else Game.ResetSpeed(this);
                
                    // movement:
                    if(!passing 
                       && !shooting)
                        Game.AgentController(this, controller, player);
                    
                    // Attack/Roll:
                    if (player.GetButtonDown("Attack") 
                        && !attacking
                        && moving
                        && !canKick)
                        Game.AttackHandler(this);

                    break;
                
                case false:
                    agentData.aiData.Tick(this);
                    break;
            }
            
            if(poss)
                CanShootCheck();

            anim.SetBool(Attacking, attacking);
            anim.SetBool(Moving, moving);
        }

        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(goal.transform.position, 1);
        }

        private void OnTriggerEnter(Collider other)
        {
            var isBall = other.GetComponent<Ball>();
            
            if (isBall != null 
                && !GameManager.instance.ballShot
                && !possCooldown
                && !canKick)
            {
                canKick = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            var isBall = other.GetComponent<Ball>();

            if(isBall != null)
                PossCheck();
                
            if (isBall != null && !possCooldown)
                canKick = true;
            
            if (isBall != null && !possCooldown && user)
            {
                if (player.GetAxis("Strafe") > 0)
                    Game.LerpGo(Ball.instance.gameObject, ballPos.position, 25 * Time.deltaTime);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var isBall = other.GetComponent<Ball>();

            if (isBall != null)
            {
                if(canKick)
                    canKick = false;
            }
            
            if (isBall != null && poss)
                Game.LosePossCooldown(this);
        }

        private void OnCollisionEnter(Collision other)
        {
            var isBall = other.collider.GetComponent<Ball>();
            if (isBall != null && !poss && team.user && !user)
            {
                Game.UserBrain(this, team);
            }
        }

        #region Animation Events

        public void LeftStep() 
        {
            leftFootFX.Play();
            if(user)
                agentData.footstepSfx.Play();
        }
        public void RightStep()  
        {
            rightFootFX.Play();
            if(user)
                agentData.footstepSfx.Play();
        }

        #endregion
        
        public void PossCheck()
        {
            if (Game.GetDistanceToBall(this) < 10
                && Game.GetDotProductToBall(this) > 0.95f)
                Game.GainPoss(this);
            else
                Game.LosePoss(this);
        }

        public void CanShootCheck()
        {
            
            if (team.teamEnum == Game.TeamEnum.Away)
                canShoot = transform.position.x > 0;
            else
                canShoot = transform.position.x < 0;
        }
    }
}
