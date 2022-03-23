using System;
using Data_Scripts;
using Gameplay;
using Rewired;
using Rewired.ComponentControls.Effects;
using Sirenix.Serialization;
using UnityEngine;

namespace _Duck._Scripts
{
    public class DuckController : MonoBehaviour
    {
        private static readonly int Velocity = Animator.StringToHash("velocity");
        
        [SerializeField] private DuckData duckData;
        [SerializeField] private int playerId = 0;
        
        [SerializeField] private float currentSpeed;
        [SerializeField] private float currentRotSpeed;
        [SerializeField] private float resetSpeed;
        
        [SerializeField] private bool moving;
        [SerializeField] private bool laying;
        [SerializeField] private bool turbo;

        [SerializeField] private ParticleSystem leftFootFX;
        [SerializeField] private ParticleSystem rightFootFX;

        private Rewired.Player player;
        private Animator anim;
        private Rigidbody rb;
        private CharacterController controller;

        private float verticalSpeed = 0;
        private float gravity = 10;
        

        private void Awake()
        {
            player = ReInput.players.GetPlayer(playerId);
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            controller = GetComponent<CharacterController>();
            resetSpeed = duckData.maxSpeed;
        }

        private void Update()
        {
            Controller();
            
            if(player.GetAxis("Turbo") > 0)
                TurboSpeed();
            else
                ResetSpeed();
        }

        #region PLAYER MOVEMENT

        public void Controller()
        {
            var xInput = player.GetAxisRaw("Horizontal");
            var zInput = player.GetAxisRaw("Vertical");
            var dir = new Vector3(xInput, 0, zInput).normalized;

            var vel = transform.forward.normalized * currentSpeed;
            
            if (dir.magnitude >= 0.1f)
            {
                moving = true;
                if (Camera.main is not null)
                {
                    var targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                    var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                        ref currentRotSpeed, duckData.turnSpeed);
                    transform.rotation = Quaternion.Euler(0, angle, 0);

                    if (currentSpeed < duckData.maxSpeed)
                        currentSpeed += Time.deltaTime * duckData.accel;
                    if (currentSpeed > duckData.maxSpeed)
                        currentSpeed -= Time.deltaTime * duckData.accel;
                    
                    
                    if (controller.isGrounded)
                    {
                        verticalSpeed = 0;
                        vel.y = verticalSpeed;
                        controller.Move(vel * Time.deltaTime);
                        
                    }
                    else
                    {
                        verticalSpeed -= gravity * Time.deltaTime;
                        vel.y = verticalSpeed;
                        controller.Move(vel * Time.deltaTime);
                    }

                }
                else
                {
                    Debug.Log("Camera.Main not present.  Controls won't work properly.");
                    
                    /*var targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.current.transform.eulerAngles.y;
                    var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                        ref currentRotSpeed, duckData.turnSpeed);
                    transform.rotation = Quaternion.Euler(0, angle, 0);

                    if (currentSpeed < duckData.maxSpeed)
                        currentSpeed += Time.deltaTime * duckData.accel;
                    if (currentSpeed > duckData.maxSpeed)
                        currentSpeed -= Time.deltaTime * duckData.accel;

                    var moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                    controller.Move(moveDir * currentSpeed * Time.deltaTime);*/
                }
            }
            else
            {
                moving = false;
                if (currentSpeed > 0)
                    currentSpeed -= Time.deltaTime * duckData.accel;
                if (currentSpeed < 0)
                    currentSpeed += 0.1f;
                if (currentSpeed is > -0.2f and < 0.2f)
                    currentSpeed = 0;
                
                verticalSpeed -= gravity * Time.deltaTime;
                vel.y = verticalSpeed;
                controller.Move(vel * Time.deltaTime);
            }
            
            anim.SetFloat(Velocity, currentSpeed);
        }

        public void TurboSpeed()
        {
            duckData.maxSpeed = duckData.turboSpeed;
            turbo = true;
            //agent.turboFx.Play();
        }

        public void ResetSpeed()
        {
            duckData.maxSpeed = resetSpeed;
            //agent.turboFx.Stop();
            turbo = false;
        }
        
        #region Animation Events

        public void LeftStep() 
        {
            leftFootFX.Play();
            duckData.footstepSfx.Play();
        }
        public void RightStep()  
        {
            rightFootFX.Play();
            duckData.footstepSfx.Play();
        }

        #endregion

        #endregion
    }
}

