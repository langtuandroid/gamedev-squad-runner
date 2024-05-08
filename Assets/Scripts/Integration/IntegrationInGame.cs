using UnityEngine;
using Zenject;

namespace Integration
{
    public class IntegrationInGame : MonoBehaviour
    {
        [SerializeField]
        private bool _isGameScene;
        private const string IntegrationsCounter = "IntegrationsGameCounter";
        private int loadLevelCount = 0; 
    
        private IAPService _iapService;
        private AdMobController _adMobController;

        [Inject]
        private void Construct(IAPService iapService, AdMobController adMobController)
        {
            _iapService = iapService;
            _adMobController = adMobController;
        }

        private void Awake()
        {
            ShowIntegration();
        }

        private void ShowIntegration()
        {
            _adMobController.ShowBanner(true);
            if (_isGameScene)
            {
                loadLevelCount = PlayerPrefs.GetInt(IntegrationsCounter, 0);
                loadLevelCount++;
            
                if (loadLevelCount % 2 == 0)
                {
                    _adMobController.ShowInterstitialAd();
                }
                else if (loadLevelCount % 3 == 0)
                {
                    _iapService.ShowSubscriptionPanel();
                }

                if (loadLevelCount >= 3)
                {
                    loadLevelCount = 0;
                }
                PlayerPrefs.SetInt(IntegrationsCounter, loadLevelCount);
                PlayerPrefs.Save(); 
            }
        }
    
    }
}
