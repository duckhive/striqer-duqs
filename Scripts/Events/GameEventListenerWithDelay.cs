using System.Collections;
using Gameplay;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public class GameEventListenerWithDelay : GameEventListener
    {
        [SerializeField] private float delay = 1f;
        [SerializeField] private UnityEvent delayedUnityEvent;

        public override void RaiseEvent()
        {
            unityEvent.Invoke();
            StartCoroutine(RunDelayedEvent());
        }

        private IEnumerator RunDelayedEvent()
        {
            yield return new WaitForSeconds(delay);
            delayedUnityEvent.Invoke();
        }
    }
}
