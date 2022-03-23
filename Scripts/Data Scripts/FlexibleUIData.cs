using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace Data_Scripts
{
    [CreateAssetMenu(fileName = "Flexible UI Data")]
    public class FlexibleUIData : ScriptableObject
    {
        public Sprite buttonSprite;
        public SpriteState buttonSpriteState;
        public ProceduralImage proceduralImage;
        public Color defaultColor;
        public Sprite defaultIcon;

        public Color confirmColor;
        public Sprite confirmIcon;

        public Color declineColor;
        public Sprite declineIcon;

        public Color warningColor;
        public Sprite warningIcon;
    }
}
