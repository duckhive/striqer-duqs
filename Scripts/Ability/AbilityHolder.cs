using System;
using UnityEngine;

namespace Ability
{
    public class AbilityHolder : MonoBehaviour
    {
        public Ability ability;
        private float cooldownTime;
        private float activeTime;

        public enum AbilityState
        {
            Ready,
            Active,
            Cooldown
        }

        private AbilityState state = AbilityState.Ready;

        public KeyCode key;

        private void Update()
        {
            switch (state)
            {
                case AbilityState.Ready:
                    if (Input.GetKeyDown(key))
                    {
                        ability.Activate(gameObject);
                        state = AbilityState.Active;
                        activeTime = ability.activeTime;
                    }
                    break;
                case AbilityState.Active:
                    if (activeTime > 0)
                        activeTime -= Time.deltaTime;
                    else
                    {
                        ability.BeginCoolDown(gameObject);
                        state = AbilityState.Cooldown;
                        cooldownTime = ability.cooldownTime;
                    }
                    break;
                case AbilityState.Cooldown:
                    if (cooldownTime > 0)
                        cooldownTime -= Time.deltaTime;
                    else
                        state = AbilityState.Cooldown;
                    break;
            }
            
        }
    }
}
