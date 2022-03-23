using System;
using System.Collections;
using Audio.DATA;
using UnityEngine;
using Util;

namespace Gameplay
{
    public class GoalTrigger : MonoBehaviour
    { 
        [HideInInspector] public Team team;

        private void Awake()
        { 
            team = GetComponentInParent<Team>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var isBall = other.GetComponent<Ball>();
            if (isBall is not null && GameManager.instance.gameObject)
                StartCoroutine(GameManager.instance.TeamScoreEvent(team));
        }
    }
}
