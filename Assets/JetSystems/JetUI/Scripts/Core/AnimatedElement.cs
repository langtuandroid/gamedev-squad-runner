using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace JetSystems.JetUI.Scripts.Core
{
    public class AnimatedElement : MonoBehaviour
    {
        public float duration;
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
            // Анімація збільшення масштабу
            transform.DOScale(originalScale * 0.9f, duration)
                .OnComplete(() =>
                {
                    transform.DOScale(originalScale, duration);
                });
        }

        // private void OnDisable()
        // {
        //     // При деактивації об'єкта повертаємо масштаб до початкового значення
        //     transform.localScale = originalScale;
        // }
    }
}
    