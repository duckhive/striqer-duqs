using UnityEngine;

namespace Data_Scripts
{
    public class AbilityData : ScriptableObject
    {
        public new string name;
        public float cooldownTime;
        public float activeTime;
        
        public virtual void Activate(GameObject agent){}
    }
}
