using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class LinkManager : MonoBehaviour
    {
        [SerializeField] 
        private GDPRLinksHolder _gdprLinksHolder;
        [SerializeField]
        private Button _privacyButtonsr;
        [SerializeField] 
        private Button _termsButtonsr;

        private bool _externalOpeningUrlDelayFlag = false;

        private void Awake()
        {
            if (_termsButtonsr != null)
                _termsButtonsr.onClick.AddListener(() => OpenUrlsr(_gdprLinksHolder.TermsOfUse));

            if (_privacyButtonsr != null)
                _privacyButtonsr.onClick.AddListener(() => OpenUrlsr(_gdprLinksHolder.PrivacyPolicy));
        }

        private void OnDestroy()
        {
            if (_termsButtonsr != null)
                _termsButtonsr.onClick.RemoveListener(() => OpenUrlsr(_gdprLinksHolder.TermsOfUse));

            if (_privacyButtonsr != null)
                _privacyButtonsr.onClick.RemoveListener(() => OpenUrlsr(_gdprLinksHolder.PrivacyPolicy));
        }

        private void Start()
        {
            Input.multiTouchEnabled = false;
        }

        private async void OpenUrlsr(string url)
        {
            if (_externalOpeningUrlDelayFlag) return;
            _externalOpeningUrlDelayFlag = true;
            await OpenURLAsyncsr(url);
            StartCoroutine(WaitForSecondssr(1, () => _externalOpeningUrlDelayFlag = false));
        }
    
        private async Task OpenURLAsyncsr(string url)
        {
            await Task.Delay(1);
            try
            {
                Application.OpenURL(url);
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка при открытии ссылки {url}: {e.Message}");
            }
        }

        private IEnumerator WaitForSecondssr(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        } 
    }
}



