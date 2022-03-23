using Data_Scripts;
using Gameplay;
using UnityEngine;

namespace Ability
{
    [CreateAssetMenu(fileName = "ShellAbility", menuName = "Scriptable Objects/ShellAbility", order = 1)]
    public class ShellAbility : AbilityData
    {
        public GameObject shell;

        public override void Activate(GameObject agent)
        {
            var a = agent.GetComponent<Agent>();
            
            var t = Instantiate(shell, a.ballPos.transform.position, Quaternion.identity);
            var shellRb = t.GetComponent<Rigidbody>();
            shellRb.AddForce(a.transform.forward * 100, ForceMode.Impulse);
        }
    }
}
