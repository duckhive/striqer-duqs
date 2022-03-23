using System;
using Audio.DATA;
using Sirenix.OdinInspector;
using UnityEngine;
using Util;

namespace Data_Scripts
{
    [CreateAssetMenu(fileName = "Agent", menuName = "Scriptable Objects/Agent", order = 1)]
    public class AgentData : ScriptableObject
    {
        #region CONFIG

        [BoxGroup("Agent Info")] public Game.Character character;
        [BoxGroup("Agent Info")] public string agentName;
        [BoxGroup("Agent Info")] public string description;

        [BoxGroup("Prefab")] public GameObject prefab;
        
        [BoxGroup("Materials")] public Material defaultMat;
        [BoxGroup("Materials")] public Material userMat;
        
        [BoxGroup("Speeds")] [Range(1,100)] public float maxSpeed = 20f;
        [BoxGroup("Speeds")] [Range(1,200)] public float accel = 40f;
        [BoxGroup("Speeds")] [DisplayAsString] public float turboSpeed;
        [BoxGroup("Speeds")] [Range(1,0)] public float turnSpeed = 0.1f;
        [BoxGroup("Speeds")] [Range(1,50)] public float attackSpeed = 15f;
        [BoxGroup("Forces")] [Range(1,200)] public float maxPassForce = 100f;
        [BoxGroup("Forces")] [Range(1,300)] public float maxShotForce = 200f;
        [BoxGroup("Forces")] [Range(1,1000)] public float maxShotSpin = 500f;

        [BoxGroup("Audio")] public SoundFXData footstepSfx;  // grass

        public AiData aiData;
        
        #endregion

        public void OnEnable()
        {
            turboSpeed = maxSpeed * 1.5f;
        }

        public void OnDisable()
        {
            turboSpeed = 0;
        }
    }
}