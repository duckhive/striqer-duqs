using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio.DATA
{
    [CreateAssetMenu(fileName = "NewSoundFX", menuName = "Scriptable Objects/SoundFX")]
    public class SoundFXData : ScriptableObject
    {
        #region CONFIG

        private static readonly float semitonesConversionUnit = 1.05946F;

        [Required]
        public AudioClip[] clips;

        [BoxGroup("Config")] 
        public bool loop;

        [BoxGroup("Config")] 
        public bool playNext;

        [MinMaxSlider(0,1)]
        [BoxGroup("Config")] 
        public Vector2 volume = new Vector2(0.5f, 0.5f);
        
        [LabelWidth(100)]
        [HorizontalGroup("Config/pitch")]
        public bool useSemitones;
       
        [HideLabel] 
        [ShowIf("useSemitones")] 
        [MinMaxSlider(-10, 10)] 
        [HorizontalGroup("Config/pitch")]
        [OnValueChanged("SyncPitchAndSemitones")]
        public Vector2Int semitones = new Vector2Int(0, 0);
        
        [HideLabel] 
        [HideIf("useSemitones")] 
        [MinMaxSlider(0, 3)] 
        [HorizontalGroup("Config/pitch")] 
        [OnValueChanged("SyncPitchAndSemitones")]
        public Vector2 pitch = new Vector2(1, 1);

        [BoxGroup("Config")] 
        [SerializeField] 
        private SoundClipPlayOrder playOrder;
        
        [DisplayAsString] 
        [BoxGroup("config")] 
        [SerializeField] 
        private int playIndex;

        #endregion
        
        #region PREVIEW CODE

#if UNITY_EDITOR
        private AudioSource previewer;

        private void OnEnable()
        {
            previewer = EditorUtility
                .CreateGameObjectWithHideFlags("AudioPreview", HideFlags.HideAndDontSave, typeof(AudioSource))
                .GetComponent<AudioSource>();
        }

        private void OnDisable()
        {
            DestroyImmediate(previewer.gameObject);
        }

        [ButtonGroup("previewControls")] [GUIColor(.3f, 1f, .3f)] [Button(ButtonSizes.Gigantic)]
        private void PlayPreview()
        {
            Play(previewer);
        }

        [ButtonGroup("previewControls")]
        [GUIColor(1f, .3f, .3f)]
        [Button(ButtonSizes.Gigantic)]
        [EnableIf("@previewer.isPlaying")]
        private void StopPreview()
        {
            previewer.Stop();
        }
#endif
        #endregion

        public void SyncPitchAndSemitones()
        {
            if (useSemitones)
            {
                pitch.x = Mathf.Pow(semitonesConversionUnit, semitones.x);
                pitch.y = Mathf.Pow(semitonesConversionUnit, semitones.y);
            }
            else
            {
                semitones.x = Mathf.RoundToInt(Mathf.Log10(pitch.x) / Mathf.Log10(semitonesConversionUnit));
                semitones.y = Mathf.RoundToInt(Mathf.Log10(pitch.y) / Mathf.Log10(semitonesConversionUnit));
            }
        }

        public AudioClip GetAudioClip()
        {
            // get current clip:
            var clip = clips[playIndex >= clips.Length ? 0 : playIndex];
            
            // find next clip:
            switch (playOrder)
            {
                case SoundClipPlayOrder.InOrder:
                    playIndex = (playIndex + 1) % clips.Length;
                    break;
                case SoundClipPlayOrder.Random:
                    playIndex = Random.Range(0, clips.Length);
                    break;
                case SoundClipPlayOrder.Reverse:
                    playIndex = (playIndex + clips.Length - 1) % clips.Length;
                    break;
            }
            
            return clip;
        }
        
        public AudioSource Play(AudioSource audioSourceParam = null)
        {
            if (clips.Length == 0)
            {
                Debug.LogWarning($"Missing sound clips for {name}");
                return null;
            }

            var source = audioSourceParam;
            if (source == null)
            {
                var obj = new GameObject("Sound", typeof(AudioSource));
                source = obj.GetComponent<AudioSource>();
            }

            //  set source config:
            source.clip = GetAudioClip();
            source.volume = Random.Range(volume.x, volume.y);
            source.pitch = useSemitones?
                Mathf.Pow(semitonesConversionUnit, Random.Range(semitones.x,semitones.y))
                :Random.Range(pitch.x, pitch.y);
            
            source.Play();
            
#if UNITY_EDITOR
            if (source != previewer)
            {
                Destroy(source.gameObject, source.clip.length / source.pitch);
            }
#else            
                Destroy(source.gameObject, source.clip.length / source.pitch);
#endif
            return source;
        }

        enum SoundClipPlayOrder
        {
            Random,
            InOrder,
            Reverse
        }
    }
}
