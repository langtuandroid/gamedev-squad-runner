using System;
using Settings;
using UI;
using UnityEngine;
using UnityEngine.UI;


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
        public CanvasGroup PAUSE;
        public ShopManager shopManager;
        public CanvasGroup[] canvases;
        
        // Menu UI
        public Text menuCoinsText;
        
        // Level Select UI
        public LevelSelectManagersr levelSelectManagersr;
        public Text levelSelectCoinsText;

        // Game UI
        public Slider progressBar;
        public Text gameCoinsText;
        public Text levelText;
        public GameObject PlayButton;

        // Shop UI
        public Text shopCoinsText;

        // Level Complete UI
        public Text levelCompleteCoinsText;
        
        private void Awake()
        {
            if (instance == null)
                instance = this;
            
            COINS = PlayerPrefsManager.GetCoins();
            UpdateCoins();
        }
        
        private void Start()
		{
            canvases = new CanvasGroup[] { MENU,LEVELSELECT, GAME, LEVELCOMPLETE, GAMEOVER, SETTINGS, PAUSE };
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

        private void UpdateCoins()
        {
            menuCoinsText.text = Utils.FormatAmountString(COINS);
            gameCoinsText.text = menuCoinsText.text;
            shopCoinsText.text = menuCoinsText.text;
            levelCompleteCoinsText.text = menuCoinsText.text;
            levelSelectCoinsText.text = menuCoinsText.text;
        }

        #region Static Methods

        public static void AddCoins(int amount)
        {
            COINS += amount;
            instance.UpdateCoins();
            PlayerPrefsManager.SaveCoins(COINS);
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