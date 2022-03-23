using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay;
using UnityEngine;
using Cinemachine;
using Data_Scripts;
using Rewired;
using Debug = System.Diagnostics.Debug;

namespace Util
{
    public static class Game
    {
        #region CACHING
        
        private static readonly int Passing = Animator.StringToHash("passing");
        private static readonly int Shooting = Animator.StringToHash("shooting");
        private static readonly int Velocity = Animator.StringToHash("velocity");
        private static readonly int Index1 = Animator.StringToHash("Index");
        
        #endregion
        
        #region ENUMS
        public enum AgentIndex
        {
            Agent1,
            Agent2,
            Agent3,
            Agent4,
            Agent5,
            Agent6,
            Agent7,
            Agent8
        }
        public enum TeamEnum
        {
            Home,
            Away
        }

        public enum ControlEnum
        {
            Player1,
            Player2,
            CPU
        }

        public enum Character
        {
            Ella,
            Barry,
            Duck,
            Robot,
            Fox,
            Demon,
            Hippo,
            Moth,
            Wendigo,
            Rex,
            Raptor
        }
        
        public enum AbilityState
        {
            Ready,
            Active,
            Cooldown
        }
        #endregion

        #region TIME
        public static void FreezeTime()
        {
            Time.timeScale = 0;
        }
        
        public static void UnfreezeTime()
        {
            Time.timeScale = 1;
        }

        public static void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }
        #endregion

        #region DotProduct

        public static float GetDotProduct(Agent agent, Vector3 target)
        {
            var dirToTarget = Vector3.Normalize(target - agent.transform.position);
            return Vector3.Dot(agent.transform.forward, dirToTarget);
        }
        
        public static float GetDotProduct(Agent agent, Transform target)
        {
            var dirToTarget = Vector3.Normalize(target.position - agent.transform.position);
            return Vector3.Dot(agent.transform.forward, dirToTarget);
        }
        
        public static float GetDotProduct(Agent agent, GameObject target)
        {
            var dirToTarget = Vector3.Normalize(target.transform.position - agent.transform.position);
            return Vector3.Dot(agent.transform.forward, dirToTarget);
        }

        public static float GetDotProduct(Agent agent, Agent targetAgent)
        {
            var dirToTarget = Vector3.Normalize(targetAgent.transform.position - agent.transform.position);
            return Vector3.Dot(agent.transform.forward, dirToTarget);
        }
        
        public static float GetDotProductToBall(Agent agent)
        {
            var dirToTarget = Vector3.Normalize(Ball.instance.transform.position - agent.transform.position);
            return Vector3.Dot(agent.transform.forward, dirToTarget);
        }

        #endregion

        #region Distance

        public static float GetDistance(Agent agent, Vector3 target)
        {
            return Vector3.Distance(target, agent.transform.position);
        }
        
        public static float GetDistance(Agent agent, Transform target)
        {
            return Vector3.Distance(target.position, agent.transform.position);
        }
        
        public static float GetDistance(Agent agent, GameObject target)
        {
            return Vector3.Distance(target.transform.position, agent.transform.position);
        }
        
        public static float GetDistance(Agent agent, Agent target)
        {
            return Vector3.Distance(target.transform.position, agent.transform.position);
        }
        
        public static float GetDistanceToBall(Agent agent)
        {
            return Vector3.Distance(Ball.instance.transform.position, agent.transform.position);
        }

        #endregion

        #region RESETTING STUFF

        public static void ResetBall(Ball ball)
        {
            ball.transform.position = Ball.instance.ballData.startPos;
            ball.rb.velocity = Vector3.zero;
            ball.rb.angularVelocity = Vector3.zero;
        }

        public static void ResetAllAgents(List<Agent> agents)
        {
            foreach (var t in agents)
            {
                var transform = t.transform;
                
                transform.position = t.startPos;
                transform.rotation = t.startRot;
                t.rb.velocity = Vector3.zero;
                t.rb.angularVelocity = Vector3.zero;
            }
        }

        #endregion
        
        #region GET CURRENT USER
    
        public static Agent CurrentUser(Team team)
        {
            return team.currentUser[0];
        }

        public static Agent CurrentUser(Agent agent)
        {
            return agent.team.currentUser[0];
        }
        #endregion
        
        #region BRAIN MANAGEMENT

        public static void AgentBrain(Agent agent)
        {
            agent.aiPath.enabled = true;
            
            agent.user = false;
            agent.rend.material = agent.agentData.defaultMat;
        }

        public static void UserBrain(Agent agent, Team team)
        {
            agent.goal.position = agent.transform.position;
            agent.aiPath.enabled = false;

            agent.user = true;
            agent.rend.material = agent.agentData.userMat;
            team.currentUser.Insert(0, agent);

            CameraManager.instance.anim.SetInteger(Index1, !GameManager.instance.sideView ? agent.animIndex : 0);

            AgentBrain(team.currentUser[1]);
            
            if (team.currentUser.Count > 2)
                team.currentUser.RemoveAt(2);
        }
        
        public static void UserBrainOnStart(Agent agent)
        {
            agent.goal.position = agent.transform.position;
            agent.aiPath.enabled = false;

            agent.user = true;
            agent.rend.material = agent.agentData.userMat;
            agent.team.currentUser.Insert(0, agent);

            switch (GameManager.instance.sideView)
            {
                case true:
                    CameraManager.instance.anim.SetInteger(Index1, 0);
                    break;
                
                case false:
                    CameraManager.instance.anim.SetInteger(Index1, agent.animIndex);
                    break;
            }
        }
        
        public static void TeamUserBrain(Team team)
        {
            team.user = true;
        }

        public static void TeamAgentBrain(Team team)
        {
            team.user = false;
        }
        
        #endregion
        
        #region PLAYER SWITCHING
    
        public static void SelectPlayerOnStart(Team team)
        {
            var smallestDistance = team.teamAgents
                .OrderBy(t => Vector3.Distance(Ball.instance.transform.position, t.transform.position))
                .FirstOrDefault();

            if (smallestDistance is not null) 
                UserBrainOnStart(smallestDistance);
        }
    
        public static void SwitchPlayerClosestToBall(Team team, List<Agent> teamAgents)
        {
            var smallestDistance = teamAgents
                .Where(t => t != team.currentUser[0])    
                .OrderBy(t => Vector3.Distance(Ball.instance.transform.position, t.transform.position))
                .FirstOrDefault();

            if (smallestDistance is not null)
                UserBrain(smallestDistance, team);
        
        }
    
        public static void SwitchPlayerInDirection(Team team, Player player, List<Agent> teamAgents)
        {
            {
                var smallestAngle = teamAgents
                    .Where(t => t != team.currentUser[0])
                    .OrderBy(t => Vector3.Angle(Game.InputDirection(player), Game.DirectionTo(t, team.currentUser[0])))
                    .FirstOrDefault();

                if (smallestAngle is not null)
                    UserBrain(smallestAngle, team);
            
            }
        }
    
        #endregion

        #region INPUT DIRECTION

        private static Vector3 InputDirection(Player player)
        {
            var main = Camera.main;
            var camera = main ? main : Camera.current;

            var xInput = player.GetAxis("Horizontal");
            var zInput = player.GetAxis("Vertical");
            var transform = camera.transform;
            var forward = transform.forward;
            forward.y = 0;
            var right = transform.right;
            right.y = 0;

            return (right * xInput + forward * zInput).normalized;
        }

        #endregion
        
        #region DIRECTION TO

        private static Vector3 DirectionTo(Transform to, Transform from)
        {
            return Vector3.Normalize(to.position - from.position);
        }

        private static Vector3 DirectionTo(Agent to, Agent from)
        {
            return Vector3.Normalize(to.transform.position - from.transform.position);
        }
        
        public static Vector3 DirectionTo(Vector3 to, Vector3 from)
        {
            return Vector3.Normalize(to - from);
        }
        
        public static Vector3 DirectionTo(GameObject to, GameObject from)
        {
            return Vector3.Normalize(to.transform.position - from.transform.position);
        }
        
        #endregion

        #region PLAYER MOVEMENT

        public static void AgentController(Agent agent, CharacterController controller, Player player)
        {
            var xInput = player.GetAxisRaw("Horizontal");
            var zInput = player.GetAxisRaw("Vertical");
            var dir = new Vector3(xInput, 0, zInput).normalized;

            var vel = agent.transform.forward.normalized * agent.speedCurrent;
            
            if (dir.magnitude >= 0.1f)
            {
                agent.moving = true;
                if (Camera.main is not null)
                {
                    var targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                    var angle = Mathf.SmoothDampAngle(agent.transform.eulerAngles.y, targetAngle,
                        ref agent.turnSpeedCurrent, agent.agentData.turnSpeed);
                    agent.transform.rotation = Quaternion.Euler(0, angle, 0);

                    if (agent.speedCurrent < agent.agentData.maxSpeed)
                        agent.speedCurrent += Time.deltaTime * agent.agentData.accel;
                    if (agent.speedCurrent > agent.agentData.maxSpeed)
                        agent.speedCurrent -= Time.deltaTime * agent.agentData.accel;
                    
                    
                    if (controller.isGrounded)
                    {
                        agent.verticalSpeed = 0;
                        vel.y = agent.verticalSpeed;
                        controller.Move(vel * Time.deltaTime);
                        
                    }
                    else
                    {
                        agent.verticalSpeed -= agent.gravity * Time.deltaTime;
                        vel.y = agent.verticalSpeed;
                        controller.Move(vel * Time.deltaTime);
                    }

                }
                else
                {
                    var targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.current.transform.eulerAngles.y;
                    var angle = Mathf.SmoothDampAngle(agent.transform.eulerAngles.y, targetAngle,
                        ref agent.turnSpeedCurrent, agent.agentData.turnSpeed);
                    agent.transform.rotation = Quaternion.Euler(0, angle, 0);

                    if (agent.speedCurrent < agent.agentData.maxSpeed)
                        agent.speedCurrent += Time.deltaTime * agent.agentData.accel;
                    if (agent.speedCurrent > agent.agentData.maxSpeed)
                        agent.speedCurrent -= Time.deltaTime * agent.agentData.accel;

                    var moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                    controller.Move(moveDir * agent.speedCurrent * Time.deltaTime);
                }
            }
            else
            {
                agent.moving = false;
                if (agent.speedCurrent > 0)
                    agent.speedCurrent -= Time.deltaTime * agent.agentData.accel;
                if (agent.speedCurrent < 0)
                    agent.speedCurrent += 0.1f;
                
                agent.verticalSpeed -= agent.gravity * Time.deltaTime;
                vel.y = agent.verticalSpeed;
                controller.Move(vel * Time.deltaTime);
            }
            
            agent.anim.SetFloat(Velocity, agent.speedCurrent);
        }

        public static void TurboSpeed(Agent agent)
        {
            agent.agentData.maxSpeed = agent.agentData.turboSpeed;
            agent.turbo = true;
            //agent.turboFx.Play();
        }

        public static void ResetSpeed(Agent agent)
        {
            agent.agentData.maxSpeed = agent.speedReset;
            //agent.turboFx.Stop();
            agent.turbo = false;
        }

        #endregion
    
        #region PLAYER ACTIONS
        public static void SnapBallToBallPos(Agent agent, Ball ball)
        {
            ball.rb.velocity = Vector3.zero;
            ball.transform.position = agent.ballPos.transform.position;
        }
        public static async void AttackHandler(Agent agent)
        {
            var end = Time.time + 1;
            
            while (Time.time < end)
            {
                agent.attacking = true;
                //agent.rb.AddForce(agent.transform.forward * agent.agentData.attackSpeed);
                agent.attackVfx.Play();
                await Task.Yield();
            }
            agent.attacking = false;
        }

        public static async void AgentTakeHit(Agent agent)
        {
            var end = Time.time + 2;

            while (Time.time < end)
            {
                //agent.rb.velocity = Vector3.zero;
                //agent.rb.angularVelocity = Vector3.zero;
                await Task.Yield();
            }
        }

        #endregion

        #region ROTATE TO TARGET

        public static void RotateToTarget(Agent agent, Transform target)
        {
            var a = agent.transform;
            var aP = a.position;
            var t = target.position;
            var newDir = Quaternion.LookRotation(new Vector3(t.x - aP.x, 0, t.z - aP.z));
        
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation,  newDir, Time.deltaTime * 20);

        }
        public static void RotateToTarget(Agent agent, Vector3 t)
        {
            var a = agent.transform;
            var aP = a.position;
            var newDir = Quaternion.LookRotation(new Vector3(t.x - aP.x, 0, t.z - aP.z));
            
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation,  newDir, Time.deltaTime * 20);

        }
        public static void RotateToTarget(Agent agent, Agent target)
        {
            var aP = agent.transform.position;
            var t = target.transform.position;
            var newDir = Quaternion.LookRotation(new Vector3(t.x - aP.x, 0, t.z - aP.z));
            
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation,  newDir, Time.deltaTime * 20);
        }
        public static void RotateToTarget(Agent agent, ShotTarget target)
        {
            var aP = agent.transform.position;
            var t = target.transform.position;
            var newDir = Quaternion.LookRotation(new Vector3(t.x - aP.x, 0, t.z - aP.z));
            
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation,  newDir, Time.deltaTime * 20);

        }
        public static void RotateToTarget(Agent agent, Ball target)
        {
            var aP = agent.transform.position;
            var t = target.transform.position;
            var newDir = Quaternion.LookRotation(new Vector3(t.x - aP.x, 0, t.z - aP.z));
            
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation,  newDir, Time.deltaTime * 20);

        }

        #endregion

        #region LERPING/SLERPING

        private static void AgentLerp(Agent agent, Vector3 target)
        {
            agent.transform.position = Vector3.Lerp(agent.transform.position, target, 0.5f);
        }

        public static void LerpGo(GameObject go, Vector3 target, float amount)
        {
            go.transform.position = Vector3.Lerp(go.transform.position, target, amount);
        }

        #endregion

        #region DRIBBLING

        public static void Dribble(Agent agent)
        {
            var dribbleLinForce = 0.2f;
            var angularForce = 1;
            Ball.instance.rb.AddForce(agent.transform.forward * dribbleLinForce);
            Ball.instance.rb.AddTorque(agent.transform.right * -angularForce);
        }
    
        public static void DribbleBackForce(Agent agent)
        {
            var backForce = 2;
            var angularForce = 1;
            Ball.instance.rb.AddForce(-agent.transform.forward * backForce * Time.deltaTime);
            Ball.instance.rb.AddTorque(agent.transform.right * -angularForce * Time.deltaTime);
        }
        
        public static void SnapBallToBallPos(Agent agent)
        {
            Ball.instance.rb.velocity = Vector3.zero;
            Ball.instance.transform.position = agent.ballPos.transform.position;
        }
        public static void LerpBallToBallPos(Agent agent, Ball ball)
        {
            ball.rb.velocity = Vector3.zero;
            LerpGo(ball.gameObject, agent.ballPos.transform.position, 0.5f);
        }
        public static void LerpBallToBallPos(Agent agent)
        {
            Ball.instance.rb.velocity = Vector3.zero;
            LerpGo(Ball.instance.gameObject, agent.ballPos.transform.position, 0.5f);
        }

        #endregion

        #region GAIN/LOSE POSSESSION

        public static void GainPoss(Agent agent)
        {
            agent.poss = true;
            agent.team.poss = true;
            GameManager.instance.lastAgentWithPoss.Insert(0, agent);
            GameManager.instance.lastAgentWithPoss.RemoveAt(1);
            
        }
        
        public static void LosePoss(Agent agent)
        {
            agent.poss = false;

            if (agent.passing)
                agent.passing = false;
            if (agent.shooting)
                agent.shooting = false;
            
            if (GameManager.instance.lastAgentWithPoss.Count > 0)
            {
                if (GameManager.instance.lastAgentWithPoss[0].team != agent.team)
                    agent.team.poss = false;
                else
                    agent.team.poss = true;
            }
        }

        public static async void LosePossCooldown(Agent agent)
        {

            var end = Time.time + 1f;
            
            while (Time.time < end)
            {
                agent.possCooldown = true;
                await Task.Yield();
            }
            agent.possCooldown = false;
        }
        #endregion
        
        #region FREEZE/UNFREEZE GAMEOBJECTS

        public static void FreezeAllAgents(List<Agent> agents)
        {
            foreach (var a in agents)
                a.rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        
        public static void UnfreezeAllAgents(List<Agent> agents)
        {
            foreach (var a in agents)
                a.rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public static void FreezeRb(Rigidbody rb)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        
        public static void UnFreezeRb(Rigidbody rb)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public static void RbFreezePos(Rigidbody rb)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }

        public static void RbZero(Rigidbody rb)
        {
            rb.velocity = Vector3.zero;
        }
    
        public static void RbConstraintsReset(Team team)
        {
            team.currentUser[0].rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }

        public static void RbConstraintsWhenShooting(Team team)
        {
            team.currentUser[0].rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        #endregion

        #region PASSING

        public static void Pass(Team team, Player player, List<Agent> teamAgents)
        {
            if (teamAgents.Count < 2) return;
            
            var a = team.currentUser[0];
            var i = player.GetButton("Pass/Switch Player");
            var iUp = player.GetButtonUp("Pass/Switch Player");
            var iDown = player.GetButtonDown("Pass/Switch Player");
            var xInput = player.GetAxis("Horizontal");
            var zInput = player.GetAxis("Vertical");

            if (iDown && a.canKick)
            {
                a.passing = true;
                a.team.passing = true;
                a.anim.SetBool(Passing, true);
            }

            Agent rotTarget = null;
            if (i && a.passing)
            {
                if (xInput != 0 || zInput != 0)
                {
                    rotTarget = teamAgents
                        .Where(t => t != a)
                        .OrderBy(t => Vector3.Angle(InputDirection(player), DirectionTo(t, a)))
                        .FirstOrDefault();
                }
                else if (xInput == 0 && zInput == 0)
                {
                    rotTarget = teamAgents
                        .Where(t => t != a)
                        .OrderBy(t => Vector3.Distance(a.transform.position, t.transform.position))
                        .FirstOrDefault();
                }
                
                RotateToTarget(a, rotTarget);
                SnapBallToBallPos(a, Ball.instance);
                a.passTimer += Time.deltaTime;
                
                if(a.passForceCurrent < a.agentData.maxPassForce)
                    a.passForceCurrent += Time.deltaTime * 50;
            }

            switch ((iUp && a.passing) || a.passTimer > 0.7f)
            {
                case true when xInput != 0 || zInput != 0:
                {
                    var smallestAngle = teamAgents
                        .Where(t => t != a)
                        .OrderBy(t => Vector3.Angle(InputDirection(player), DirectionTo(t, a)))
                        .FirstOrDefault();
                    
                    if(smallestAngle is null) return;
                    Vector3 dir = default;
            
                    if (!player.GetButton("LB"))
                        dir = DirectionTo(smallestAngle.transform, a.transform);
                    else if (player.GetButton("LB"))
                        dir = DirectionTo(smallestAngle.airPassTarget.transform, a.transform);
                    
                    LosePossCooldown(a);
                    LosePoss(a);
                    Ball.instance.passFx.Play();
                    Ball.instance.ballKickedSfx.Play();
                    Ball.instance.rb.AddForce(dir * a.passForceCurrent, ForceMode.Impulse);
                    GameManager.instance.ballPassed = true;
                    UserBrain(smallestAngle, team);
                    a.anim.SetBool(Passing, false);
                    a.passing = false;
                    a.team.passing = false;
                    a.passTimer = 0;
                    a.passForceCurrent = a.passForceReset;
                    
                    break;
                }
                case true when xInput == 0 && zInput == 0:
                    PassToClosest(team, player, teamAgents);
                    a.passing = false;
                    a.team.passing = false;
                    a.passTimer = 0;
                    break;
            }
        }

        private static void PassToClosest(Team team, Player player, List<Agent> teamAgents)
        {
            var a = team.currentUser[0];
        
            var closestPlayer = teamAgents
                .Where(t => t != a)
                .OrderBy(t => Vector3.Distance(a.transform.position, t.transform.position))
                .FirstOrDefault();

            if (closestPlayer is null) return;
            Vector3 dir = default; 
            
            if (!player.GetButton("LB"))
                dir = DirectionTo(closestPlayer.transform, a.transform);
            else if (player.GetButton("LB"))
                dir = DirectionTo(closestPlayer.airPassTarget, a.transform);
            
            LosePoss(a);
            LosePossCooldown(a);
            Ball.instance.passFx.Play();
            Ball.instance.ballKickedSfx.Play();
            Ball.instance.rb.AddForce(dir * a.passForceCurrent, ForceMode.Impulse);
            GameManager.instance.ballPassed = true;
            UserBrain(closestPlayer, team);
            a.anim.SetBool(Passing, false);
            a.passForceCurrent = a.passForceReset;
        }

        private static void AgentPassRot(Agent agent, Vector3 target)
        {
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, Quaternion.LookRotation(new Vector3(target.x, 0, target.z)), Time.deltaTime * 15);
        }
        #endregion
    
        #region SHOOTING
        public static void Shoot(Team team, Player player)
        {
            var a = team.currentUser[0];
            var aT = a.transform;
            var tar = team.shotTarget.transform;


            if (player.GetButtonDown("Shoot/Slide") 
                && !a.attacking)
            {
                Ball.instance.rb.angularVelocity = Vector3.zero;
                a.shooting = true;
                a.anim.SetBool(Shooting, true);
                GameManager.instance.shooting = true;
            }
            
            if (player.GetButton("Shoot/Slide") && a.shooting)
            {
                tar.Translate(Vector3.up * Time.deltaTime * 30f);
                
                a.shotTimer += Time.deltaTime;
                AimShotTarget(team, player);
                RotateToTarget(a, team.shotTarget);
                SnapBallToBallPos(a, Ball.instance);
                SideSpin(a, player);
                
                if(a.shotForceCurrent < a.agentData.maxShotForce)
                    a.shotForceCurrent += Time.deltaTime;
            }
        
            if (a.shooting 
                && (player.GetButtonUp("Shoot/Slide") || a.shotTimer > 0.6f))
            {
                LosePoss(a);
                LosePossCooldown(a);
                a.shooting = false;
                a.anim.SetBool(Shooting, false);
                GameManager.instance.shooting = false;
                Ball.instance.shootFX.Play();
                Ball.instance.ballKickedSfx.Play();
                Ball.instance.KickCooldown();
                Ball.instance.rb.AddForce((tar.position - aT.position).normalized * a.agentData.maxShotForce/*shotForceCurrent*/, ForceMode.Impulse);
                Ball.instance.curve = true;
                Ball.instance.trail.enabled = true;
                GameManager.instance.ballShot = true;
                ResetShotTarget(team.shotTarget);
                a.shotTimer = 0;
                a.shotForceCurrent = a.shotForceReset;
            }
        }

        private static void SideSpin(Agent agent, Player player)
        {
            if (player.GetButton("LB"))
                Ball.instance.rb.AddTorque(Vector3.up * -agent.agentData.maxShotSpin * Time.deltaTime);
            if (player.GetButton("RB"))
                Ball.instance.rb.AddTorque(Vector3.up * agent.agentData.maxShotSpin * Time.deltaTime);
        }
        
        private static void AimShotTarget(Team team, Player player)
        {
            var xInput = player.GetAxis("Horizontal");
            var zInput = player.GetAxis("Vertical");
        
            var tar = team.shotTarget.transform;
            var tarPos = tar.position;

            if (GameManager.instance.sideView is not true)
            {
                if(xInput is not 0)
                    tar.Translate(team.shotTarget.transform.forward * -xInput * Time.deltaTime * 200);
                else 
                    team.shotTarget.transform.position = team.shotTarget.reset;
            }
            else
            {
                if(zInput is not 0)
                    tar.Translate(team.shotTarget.transform.forward * -zInput * Time.deltaTime * 200);
                else 
                    team.shotTarget.transform.position = team.shotTarget.reset;
            }
                    
            
            
            /*if(xInput is not 0 || zInput is not 0){}
            
            if (!GameManager.instance.sideView)
            {
                if (xInput > 0 && tarPos.z > -300) 
                    tar.Translate(Vector3.forward * xInput * Time.deltaTime * 300f);
                else if (xInput < 0 && tarPos.z < 300)
                    tar.Translate(Vector3.forward * xInput * Time.deltaTime * 300f);
                else if (xInput == 0)
                    team.shotTarget.transform.position = team.shotTarget.reset;
            }
            
            if (GameManager.instance.sideView)
            {
                if (zInput > 0 && tarPos.z > -300) 
                    tar.Translate(Vector3.forward * -zInput * Time.deltaTime * 300f);
                else if (zInput < 0 && tarPos.z < 300)
                    tar.Translate(Vector3.forward * -zInput * Time.deltaTime * 300f);
                else if (zInput == 0)
                    team.shotTarget.transform.position = team.shotTarget.reset;
            }*/
        }

        private static void ResetShotTarget(ShotTarget target)
        {
            target.transform.position = target.reset;
        }

        #endregion Shooting
    }
}
