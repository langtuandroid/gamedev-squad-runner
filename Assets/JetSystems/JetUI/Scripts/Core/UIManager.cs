using System;
using Audio;
using Cinemachine;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        public LevelSelectManager LevelSelectManager;
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

            // Get the coins amount
            COINS = PlayerPrefsManager.GetCoins();
            UpdateCoins();
        }

        // Start is called before the first frame update
        void Start()
		{
            // Store the canvases
            canvases = new CanvasGroup[] { MENU,LEVELSELECT, GAME, LEVELCOMPLETE, GAMEOVER, SETTINGS, PAUSE };

            // Configure the delegates
            ConfigureDelegates();

            // Set the menu at start
            SetMenu();
		}

        private void ConfigureDelegates()
        {
            // Basic events
           // setMenuDelegate += SetMenu;
           // setGameDelegate += SetGame;
            setLevelCompleteDelegate += SetLevelComplete;
            setGameoverDelegate += SetGameover;
            //setSettingsDelegate += SetSettings;

            // Progress bar events
            updateProgressBarDelegate += UpdateProgressBar;
        }

        private void OnDestroy()
        {

            // Basic events
            //setMenuDelegate -= SetMenu;
            //setGameDelegate -= SetGame;
            setLevelCompleteDelegate -= SetLevelComplete;
            setGameoverDelegate -= SetGameover;
            //setSettingsDelegate -= SetSettings;

            // Progress bar events
            updateProgressBarDelegate -= UpdateProgressBar;
        }

        // Update is called once per frame
        void Update()
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
            //shopManager.gameObject.SetActive(false);
        }
        
        public void SetLevelSelect()
        {
            gameState = GameState.LEVELSELECT;
            Utils.HideAllCGs(canvases, LEVELSELECT);
            LevelSelectManager.RefreshLevelsView();
        }

        public void SetGame()
        {
            gameState = GameState.RAEDY;
            StopRuning?.Invoke();
            Utils.HideAllCGs(canvases, GAME);
            PlayButton.SetActive(true);
            progressBar.value = 0;
            levelText.text = "Level " + (LevelSelectManager.LevelSelected);
        }
        public void StartGame()
        {
            gameState = GameState.GAME;
            ActiveRuning.Invoke();
            progressBar.value = 0;
            // Update the level text
            levelText.text = "Level " + LevelSelectManager.LevelSelected;
            AudioManager.Instance.PlaySFXOneShot(0);
        }

        public void SetLevelComplete(int starsCount = 3)
        {
            gameState = GameState.LEVELCOMPLETE;
            Utils.HideAllCGs(canvases, LEVELCOMPLETE);
            onLevelCompleteSet?.Invoke(starsCount);
            LevelComplete?.Invoke();
            
           // Audio_Manager.instance.play("Level_Complete");
            //Invoke("AdsControl", 2f);
            
           
        }

        public void SetGameover()
        {
            gameState = GameState.GAMEOVER;
            Utils.HideAllCGs(canvases, GAMEOVER);
            onGameoverSet?.Invoke();
            AudioManager.Instance.PlaySFXOneShot(2);
            //Invoke("AdsControl", 2f);
        }

        public void SetSettings()
        {
            Utils.EnableCG(SETTINGS);
        }
        
        public void CloseSettings()
        {
            Utils.DisableCG(SETTINGS);
        }
        
        public void SetPause()
        {
            //gameState = GameState.PAUSE;
            //StopRuning?.Invoke();
            Time.timeScale = 0f;
            Utils.EnableCG(PAUSE);
        }
        
        public void PauseDisable()
        {
            //gameState = GameState.GAME;
            Time.timeScale = 1f;
            //ActiveRuning.Invoke();
            Utils.DisableCG(PAUSE);
        }
        

        public void SetShop()
        {
            gameState = GameState.SHOP;
            shopManager.gameObject.SetActive(true);
            Utils.HideAllCGs(canvases);
        }


        public void CloseShop()
        {
            // Disable the shop object
            shopManager.gameObject.SetActive(false);
            SetMenu();
        }

        public void NextLevelButtonCallback()
        {
            StopRuning?.Invoke();
            int Nextlevel = PlayerPrefsManager.GetLevel() + 1;
            onNextLevelButtonPressed?.Invoke(Nextlevel);
            SetLevelSelect();
            //SceneManager.LoadScene(0);
        }

        public void RetryButtonCallback()
        {
            Time.timeScale = 1f;
            Utils.DisableCG(PAUSE);
            // if (gameState == GameState.PAUSE)
            // {
            //     
            // }
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
            // Increase the amount of coins
            COINS += amount;

            // Update the coins
            instance.UpdateCoins();

            // Save the amount of coins
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