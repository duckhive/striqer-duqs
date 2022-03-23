using System;
using Audio.DATA;
using UnityEngine;

namespace Data_Scripts
{
    [CreateAssetMenu(fileName = "New Duck", menuName = "Scriptable Objects/Duck")]
    public class DuckData : ScriptableObject
    {
        public float maxSpeed = 20;
        public float turnSpeed = 0.5f;
        public float accel = 40;
        public float turboSpeed;

        public SoundFXData footstepSfx;

        private void OnEnable()
        {
            turboSpeed = maxSpeed * 1.5f;
        }
        
    }
}
