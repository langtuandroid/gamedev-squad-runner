using System;
using Integration;
using Settings;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace JetSystems
{
	public class UIManager : MonoBehaviour
	{
        public enum Orientation { Portrait, Landscape }
        public Orientation orientation;

        public enum GameState { MENU, RAEDY, LEVELSELECT, GAME, LEVELCOMPLETE, GAMEOVER, SETTINGS, SHOP, PAUSE }
        public static GameState gameState;

        #region Static Variables

        public static int COINS;
        public static int DIAMONDS;
        public static UIManager instance;

        #endregion

        #region Delegates

        public delegate void SetLevelCompleteDelegate(int starsCount = 3);
        public static SetLevelCompleteDelegate setLevelCompleteDelegate;

        public delegate void OnLevelCompleteSet(int starsCount = 3);
        public static OnLevelCompleteSet onLevelCompleteSet;

        public static event Action LevelComplete;

        public delegate void SetGameoverDelegate();
        public static SetGameoverDelegate setGameoverDelegate;

        public delegate void OnGameoverSet();
        public static OnGameoverSet onGameoverSet;


        public delegate void UpdateProgressBarDelegate(float value);
        public static UpdateProgressBarDelegate updateProgressBarDelegate;

        
        public static event Action<int> onNextLevelButtonPressed;
        public static event Action<int> onRetryButtonPressed;
        
        public static event Action ActiveRuning;
        public static event Action StopRuning;

        #endregion
        
        // Canvas Groups
        public CanvasGroup MENU;
        public CanvasGroup LEVELSELECT;
        public CanvasGroup GAME;
        public CanvasGroup LEVELCOMPLETE;
        public CanvasGroup GAMEOVER;
        public CanvasGroup SETTINGS;
        public CanvasGroup DIAMONDSSHOP;
        public CanvasGroup PAUSE;
        public ShopManager shopManager;
        public CanvasGroup[] canvases;
        
        // Menu UI
        public TextMeshProUGUI menuCoinsText;
        public TextMeshProUGUI menuDiamondsText;
        
        // Level Select UI
        public LevelSelectManagersr levelSelectManagersr;
        public TextMeshProUGUI levelSelectCoinsText;

        // Game UI
        public Slider progressBar;
        public TextMeshProUGUI gameCoinsText;
        public TextMeshProUGUI levelText;
        public GameObject PlayButton;

        // Shop UI
        public TextMeshProUGUI shopCoinsText;
        public TextMeshProUGUI shopDiamondsText;
        
        // Shop Diamonds UI
        
        public TextMeshProUGUI shopDiamondsViewText;

        // Level Complete UI
        //public Text levelCompleteCoinsText;
        
        
        private const string IntegrationsCounter = "IntegrationsGameSceneCounter";
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
            if (instance == null)
                instance = this;
            
            COINS = PlayerPrefsManager.GetCoins();
            DIAMONDS = PlayerPrefsManager.GetDiamonds();
            UpdateCoins();
            UpdateDiamonds();
        }
        
        private void Start()
		{
            canvases = new CanvasGroup[] { MENU,LEVELSELECT, GAME, LEVELCOMPLETE, GAMEOVER, SETTINGS, DIAMONDSSHOP, PAUSE };
            ConfigureDelegates();
            SetMenu();
		}

        private void ConfigureDelegates()
        {
            setLevelCompleteDelegate += SetLevelComplete;
            setGameoverDelegate += SetGameover;
            updateProgressBarDelegate += UpdateProgressBar;
        }

        private void OnDestroy()
        {
            setLevelCompleteDelegate -= SetLevelComplete;
            setGameoverDelegate -= SetGameover;
            updateProgressBarDelegate -= UpdateProgressBar;
        }

        
        private void Update()
		{
            if (Input.GetKeyDown(KeyCode.C))
                SetLevelComplete();
		}
        
        private void ShowIntegration()
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

        public void SetMenu()
        {
            StopRuning?.Invoke();
            Time.timeScale = 1f;
            gameState = GameState.MENU;
            Utils.HideAllCGs(canvases, MENU);
        }
        
        public void SetLevelSelect()
        {
            gameState = GameState.LEVELSELECT;
            Utils.HideAllCGs(canvases, LEVELSELECT);
            levelSelectManagersr.RefreshLevelsViewsr();
        }

        public void SetGame()
        {
            gameState = GameState.RAEDY;
            StopRuning?.Invoke();
            Utils.HideAllCGs(canvases, GAME);
            PlayButton.SetActive(true);
            progressBar.value = 0;
            levelText.text = "Level " + (levelSelectManagersr.LevelSelectedsr);
            ShowIntegration();
        }
        public void StartGame()
        {
            gameState = GameState.GAME;
            ActiveRuning.Invoke();
            progressBar.value = 0;
            levelText.text = "Level " + levelSelectManagersr.LevelSelectedsr;
            AudioManagersr.Instancesr.PlaySFXOneShotsr(0);
        }

        public void SetLevelComplete(int starsCount = 3)
        {
            gameState = GameState.LEVELCOMPLETE;
            Utils.HideAllCGs(canvases, LEVELCOMPLETE);
            onLevelCompleteSet?.Invoke(starsCount);
            LevelComplete?.Invoke();
           //win AudioManagersr.Instancesr.PlaySFXOneShotsr(0);
        }

        public void SetGameover()
        {
            gameState = GameState.GAMEOVER;
            Utils.HideAllCGs(canvases, GAMEOVER);
            onGameoverSet?.Invoke();
            AudioManagersr.Instancesr.PlaySFXOneShotsr(2);
        }

        public void SetSettings()
        {
            AudioManagersr.Instancesr.PlaySFXOneShotsr(0);
            Utils.EnableCG(SETTINGS);
        }
        
        public void CloseSettings()
        {
            AudioManagersr.Instancesr.PlaySFXOneShotsr(0);
            Utils.DisableCG(SETTINGS);
        }
        
        public void SetDiamondsShop()
        {
            AudioManagersr.Instancesr.PlaySFXOneShotsr(0);
            Utils.EnableCG(DIAMONDSSHOP);
            UpdateDiamonds();
            if ( gameState == GameState.SHOP)
            {
                shopManager.gameObject.SetActive(false);
            }
        }
        
        public void CloseDiamondsShop()
        {
            AudioManagersr.Instancesr.PlaySFXOneShotsr(0);
            Utils.DisableCG(DIAMONDSSHOP);
            UpdateDiamonds();
            if ( gameState == GameState.SHOP)
            {
                shopManager.gameObject.SetActive(true);
            }
        }
        
        public void SetPause()
        {
            AudioManagersr.Instancesr.PlaySFXOneShotsr(0);
            Time.timeScale = 0f;
            Utils.EnableCG(PAUSE);
        }
        
        public void PauseDisable()
        {
            AudioManagersr.Instancesr.PlaySFXOneShotsr(0);
            Time.timeScale = 1f;
            Utils.DisableCG(PAUSE);
        }
        

        public void SetShop()
        {
            AudioManagersr.Instancesr.PlaySFXOneShotsr(0);
            gameState = GameState.SHOP;
            shopManager.gameObject.SetActive(true);
            Utils.HideAllCGs(canvases);
        }


        public void CloseShop()
        {
            AudioManagersr.Instancesr.PlaySFXOneShotsr(0);
            shopManager.gameObject.SetActive(false);
            SetMenu();
        }

        public void NextLevelButtonCallback()
        {
            AudioManagersr.Instancesr.PlaySFXOneShotsr(0);
            StopRuning?.Invoke();
            int Nextlevel = PlayerPrefsManager.GetLevel() + 1;
            onNextLevelButtonPressed?.Invoke(Nextlevel);
            SetLevelSelect();
        }

        public void RetryButtonCallback()
        {
            AudioManagersr.Instancesr.PlaySFXOneShotsr(0);
            Time.timeScale = 1f;
            Utils.DisableCG(PAUSE);
            int Presentlevel = PlayerPrefsManager.GetLevel();
            onRetryButtonPressed?.Invoke(Presentlevel);
            SetGame();
        }

        public void UpdateProgressBar(float value)
        {
            progressBar.value = value;
        }

        public void UpdateCoins()
        {
            menuCoinsText.text = Utils.FormatAmountString(COINS);
            gameCoinsText.text = menuCoinsText.text;
            shopCoinsText.text = menuCoinsText.text;
            levelSelectCoinsText.text = menuCoinsText.text;
        }
        
        public void UpdateDiamonds()
        {
            menuDiamondsText.text = Utils.FormatAmountString(DIAMONDS);
            shopDiamondsText.text = menuDiamondsText.text;
            shopDiamondsViewText.text = menuDiamondsText.text;
        }

        #region Static Methods

        public static void AddCoins(int amount)
        {
            COINS += amount;
            instance.UpdateCoins();
            PlayerPrefsManager.SaveCoins(COINS);
        }
        
        public static void RemoveCoins(int amount)
        {
            COINS -= amount;
            PlayerPrefsManager.SaveCoins(COINS);
            instance.UpdateCoins();
        }
        
        public static void AddDiamonds(int amount)
        {
            DIAMONDS += amount;
            PlayerPrefsManager.SaveDiamonds(DIAMONDS);
            instance.UpdateDiamonds();
        }
        
        public static void RemoveDiamonds(int amount)
        {
            DIAMONDS -= amount;
            PlayerPrefsManager.SaveDiamonds(DIAMONDS);
            instance.UpdateDiamonds();
        }

        public static bool IsGame()
        {
            return gameState == GameState.GAME;
        }

        public static bool IsLevelComplete()
        {
            return gameState == GameState.LEVELCOMPLETE;
        }

        public static bool IsGameover()
        {
            return gameState == GameState.GAMEOVER;
        }

        #endregion
    }


}