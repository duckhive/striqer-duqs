using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Gameplay
{
    public class ShotTarget : MonoBehaviour
    {
        public Vector3 reset;

        private void Awake()
        {
            reset = transform.position;
        }
    }
}
