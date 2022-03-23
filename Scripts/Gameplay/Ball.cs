using Audio.DATA;
using DarkTonic.MasterAudio;
using Data_Scripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    public class Ball : MonoBehaviour
    {
        [BoxGroup("Scriptable Object")] public BallData ballData;
        
        public static Ball instance;
        
        [BoxGroup("Particle Systems")] public ParticleSystem shootFX;
        [BoxGroup("Particle Systems")] public ParticleSystem passFx;

        [BoxGroup("Current States")] public bool curve;
        [BoxGroup("Current States")] public bool cooldown;
        
        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public TrailRenderer trail;

        [BoxGroup("Audio")] public SoundFXData ballKickedSfx;
        

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            
            rb = GetComponent<Rigidbody>();
            trail = GetComponent<TrailRenderer>();
        }
        private void Update()
        {
            if(curve)
                MagnusEffect();
            
            ResetBallWhenOb();
        }

        private void ResetBallWhenOb()
        {
            var transformPosition = transform.position;
            if (transformPosition.x is > 275 or < -275 || transformPosition.z is > 175 or < -175)
            {
                gameObject.transform.position = new Vector3(0, 1.5f, 0);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        
        private void MagnusEffect() // activate to make ball curve
        {
            const float magnusConstant = 10f;

            rb.AddForce(Vector3.Cross(rb.angularVelocity, rb.velocity) * magnusConstant * Time.deltaTime);
        }
        public void SpinBall(Vector3 spinDirection, Vector3 forward)
        {
            rb.AddTorque(spinDirection * 200 * Time.deltaTime, ForceMode.VelocityChange);
        }
        
        public async void KickCooldown()
        {
            var end = Time.time + 0.1f;
            
            while (Time.time < end)
            {
                cooldown = true;
                await System.Threading.Tasks.Task.Yield();
            }
            cooldown = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            var agent = other.gameObject.GetComponent<Agent>();
            var post = other.gameObject.GetComponent<GoalPost>();
            
            if (post is not null)
                ballData.hitPostSfx.Play();

            if (curve)
            {
                curve = false;
                trail.enabled = false;
            }

            if (GameManager.instance.ballShot && !cooldown)
            {
                GameManager.instance.ballShot = false;
                trail.enabled = false;
            }
            if (GameManager.instance.ballPassed)
            {
                GameManager.instance.ballPassed = false;
                trail.enabled = false;
            }
        }
    }
}
