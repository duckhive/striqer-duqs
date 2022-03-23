using Audio.DATA;
using UnityEngine;

namespace Data_Scripts
{
    [CreateAssetMenu(fileName = "Ball", menuName = "Scriptable Objects/Ball", order = 1)]
    public class BallData : ScriptableObject
    {
        public Vector3 startPos = new Vector3(0, 1.25f, 0);
        
        public SoundFXData hitPostSfx;
    }
}
