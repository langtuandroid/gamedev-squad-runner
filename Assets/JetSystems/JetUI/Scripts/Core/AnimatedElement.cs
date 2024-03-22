using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace JetSystems.JetUI.Scripts.Core
{
    public class AnimatedElement : MonoBehaviour
    {
        public float duration = 0.1f;
        private Button _button;
        private Vector3 originalScale;

        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();
            _button.onClick.AddListener(ClickEffect);
            originalScale = transform.localScale; 
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ClickEffect);
        }

        private void ClickEffect()
        {
            transform.DOScale(originalScale * 0.9f, duration)
                .OnComplete(() =>
                {
                    transform.DOScale(originalScale, duration);
                });
        }
    }
}
    