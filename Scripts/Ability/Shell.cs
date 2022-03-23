using System;
using System.Collections;
using Audio.DATA;
using Gameplay;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Util;

namespace Ability
{
    public class Shell : MonoBehaviour
    {
        public SoundFXData shellHum;
        private void OnEnable()
        {
            StartCoroutine(DestroyAfter(5));
            shellHum.Play();
        }

        private void Update()
        {
            transform.Rotate(transform.up * Time.deltaTime * 500);
        }

        private void OnCollisionEnter(Collision other)
        {
            var a = other.collider.GetComponent<Agent>();
            if (a is not null)
            {
                Debug.Log("shell hit player");
                a.hitFX.Play();
                a.anim.SetTrigger("take hit");
                Game.AgentTakeHit(a);
                Destroy(gameObject);
            }
        }

        private IEnumerator DestroyAfter(float time)
        {
            yield return new WaitForSeconds(time);
            if(gameObject != null)
                Destroy(gameObject);
        }
    }
}
