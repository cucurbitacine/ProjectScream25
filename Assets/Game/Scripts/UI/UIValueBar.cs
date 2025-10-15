using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class UIValueBar : MonoBehaviour
    {
        [SerializeField] private Sprite iconDefault;
        [SerializeField] private Sprite iconEmpty;

        [Space]
        [SerializeField] private Image imageIcon;
        [SerializeField] private Image imageFill;

        public void SetValue(float value, float maximum)
        {
            var fillAmount = maximum > 0f ? value / maximum : 1f;
            
            imageFill.fillAmount = fillAmount;

            imageIcon.sprite = fillAmount > 0f ? iconDefault : iconEmpty;
        }
    }
}
