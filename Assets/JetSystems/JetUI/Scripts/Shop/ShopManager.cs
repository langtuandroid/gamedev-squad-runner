using System;
using System.Collections.Generic;
using Gameplay;
using GoogleMobileAds.Api;
using Integration;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Zenject;

namespace JetSystems
{
    
    public class ShopManager : MonoBehaviour
    {
        #region Delegates

        public delegate void OnItemSelected(int itemIndex);
        public static OnItemSelected onItemSelected;

        #endregion

        [Header(" Managers ")]
        public UIManager uiManager;
        [SerializeField]
        private SquadControllersr squadControllersr;
        [Header(" Settings ")]
        public Transform itemParent;
        public Sprite[] itemsSprites;
        [SerializeField]
        private List<ShopButton> _allShopItemButtons;
        [SerializeField]
        private Sprite _coinSprite;
        [SerializeField]
        private Sprite _diamondSprite;
        [SerializeField]
        private Image _lockImage;
        [SerializeField]
        private Text _lockPrice;
        [SerializeField]
        private GameObject _lockbtn;
        [SerializeField]
        private GameObject _unlockCoinbtn;
        [SerializeField]
        private GameObject _unlockDiamondbtn;
        [SerializeField]
        private GameObject _watchAdbtn;
        [SerializeField]
        private Text _priceCoinText;
        [SerializeField]
        private Text _priceDiamondText;

        [Header(" Unlocking ")]
        bool unlocking;

        [Header(" Rendering ")]
        public Transform rotatingItemParent;

        [Header(" Design ")]
        public Sprite unlockedSprite;

        [Header(" Sounds ")]
        public AudioSource randomSound;

        private int _selectedItem;
        private AdMobController _adMobController;
        private RewardedAdController _rewardedAdController;
        
        [Inject]
        private void Construct(AdMobController adMobController, RewardedAdController rewardedAdController)
        {
            _adMobController = adMobController;
            _rewardedAdController = rewardedAdController;
        }
            
        private void Start()
        {
            LoadData();
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }


        private void OnEnable()
        {
            LoadData();
            SelectItem(PlayerPrefsManager.GetSelectHeroModel());
        }

        private void LoadData()
        {
            for (int i = 0; i < _allShopItemButtons.Count; i++)
            {
                _allShopItemButtons[i].Configure(itemsSprites[i]);

                if (i == PlayerPrefsManager.GetSelectHeroModel())
                {
                    _allShopItemButtons[i].SetSelected(true);
                    CheckUnlockStatus(i);
                }
                _allShopItemButtons[i].SetContainerSprite(unlockedSprite);
            }
           // UnlockSkin(0);
        }

        private void CheckUnlockStatus(int index)
        {

            if (IsItemUnlocked(index))
            {
                _lockbtn.SetActive(false);
                _unlockCoinbtn.SetActive(false);
                _unlockDiamondbtn.SetActive(false);
                _watchAdbtn.SetActive(false);
                ChooseCharacter(index);
            }
            else
            {

                if (_allShopItemButtons[index].ItemType == TypeItem.ForCoins)
                {
                    _watchAdbtn.SetActive(false);
                    _unlockDiamondbtn.SetActive(false);
                    _priceCoinText.text = _allShopItemButtons[index].Price.ToString();
                    if (UIManager.COINS >= _allShopItemButtons[index].Price)
                    {
                        _lockbtn.SetActive(false);
                        _unlockCoinbtn.SetActive(true);
                    }
                    else
                    {
                        _lockImage.sprite = _coinSprite;
                        _lockPrice.text = _allShopItemButtons[index].Price.ToString();
                        _lockbtn.SetActive(true);
                        _unlockCoinbtn.SetActive(false);
                    }
                }
                if (_allShopItemButtons[index].ItemType == TypeItem.ForDiamonds)
                {
                    _watchAdbtn.SetActive(false);
                    
                    _priceDiamondText.text = _allShopItemButtons[index].Price.ToString();
                    if (UIManager.DIAMONDS >= _allShopItemButtons[index].Price)
                    {
                        _lockbtn.SetActive(false);
                        _unlockDiamondbtn.SetActive(true);
                    }
                    else
                    {
                        _lockImage.sprite = _diamondSprite;
                        _lockPrice.text = _allShopItemButtons[index].Price.ToString();
                        _lockbtn.SetActive(true);
                        _unlockDiamondbtn.SetActive(false);
                    }
                }
                if (_allShopItemButtons[index].ItemType == TypeItem.ForAds)
                {
                    _lockbtn.SetActive(false);
                    _unlockCoinbtn.SetActive(false);
                    _unlockDiamondbtn.SetActive(false);
                    _watchAdbtn.SetActive(true);
                }
            }
        }

        public void UnlockSkin(int indexItem)
        {
            if ( _allShopItemButtons[indexItem].ItemType == TypeItem.ForCoins)
            {
                UIManager.RemoveCoins(_allShopItemButtons[indexItem].Price);
            }
            if ( _allShopItemButtons[indexItem].ItemType == TypeItem.ForDiamonds)
            {
                UIManager.RemoveDiamonds(_allShopItemButtons[indexItem].Price);
            }
            UnlockItem(indexItem);
            CheckUnlockStatus(indexItem);
        }
        

        private void AddListeners()
        {
            _unlockCoinbtn.GetComponent<Button>().onClick.AddListener(UnlockCharacter);
            _unlockDiamondbtn.GetComponent<Button>().onClick.AddListener(UnlockCharacter);
            _watchAdbtn.GetComponent<Button>().onClick.AddListener(ShowRewardedAd);
            _rewardedAdController.GetRewarded += UnlockCharacter;
            int k = 0;
            foreach (var ShopButton in _allShopItemButtons)
            {
                int _k = k;
                ShopButton.GetComponent<Button>().onClick.AddListener(delegate { SelectItem(_k); });
                k++;
            }
        }

        private void RemoveListeners()
        {
            _unlockCoinbtn.GetComponent<Button>().onClick.RemoveListener(UnlockCharacter);
            _unlockDiamondbtn.GetComponent<Button>().onClick.RemoveListener(UnlockCharacter);
            _watchAdbtn.GetComponent<Button>().onClick.RemoveListener(ShowRewardedAd);
            _rewardedAdController.GetRewarded -= UnlockCharacter;
            int k = 0;
            foreach (var ShopButton in _allShopItemButtons)
            {
                int _k = k;
                ShopButton.GetComponent<Button>().onClick.RemoveListener(delegate { SelectItem(_k); });
                k++;
            }
        }
        

        private void SelectItem(int itemIndex)
        {
            for (int i = 0; i < _allShopItemButtons.Count; i++)
            {
                if (i == itemIndex)
                {
                    _allShopItemButtons[i].SetSelected(true);
                    _selectedItem = itemIndex;
                }
                else
                {
                    _allShopItemButtons[i].SetSelected(false);
                }
            }
            CheckUnlockStatus(itemIndex);
            ShowItem(itemIndex);
        }

        public void UnlockCharacter()
        {
            UnlockSkin(_selectedItem);
        }
        

        private void ChooseCharacter(int indexCharacter)
        {
            PlayerPrefsManager.SaveSelectHeroModel(indexCharacter);
            squadControllersr.SelectNewCharacter();
        }

        public void CloseShop()
        {
            uiManager.CloseShop();
        }


        private void ShowItem(int itemIndex)
        {
            onItemSelected?.Invoke(itemIndex);

            for (int i = 0; i < rotatingItemParent.childCount; i++)
            {
                if (i == itemIndex)
                {
                    rotatingItemParent.GetChild(i).gameObject.SetActive(true);
                    itemParent.GetChild(i).gameObject.SetActive(true);
                    //ChooseCharacter(itemIndex);
                }
                else
                {
                    rotatingItemParent.GetChild(i).gameObject.SetActive(false);
                }
            }

            if (randomSound != null)
            {
                randomSound.Play();
            }
        }

        public void ShowRewardedAd()
        {
            _adMobController.ShowRewardedAd();
        }

        private bool IsItemUnlocked(int itemIndex)
        {
            return PlayerPrefsManager.GetItemUnlockedState(itemIndex) == 1;
        }

        private void UnlockItem(int itemIndex)
        {
            PlayerPrefsManager.SetItemUnlockedState(itemIndex, 1);
        }
    }
}