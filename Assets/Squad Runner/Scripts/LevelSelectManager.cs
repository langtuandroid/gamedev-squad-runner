using System;
using System.Collections.Generic;
using JetSystems;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelectManager : MonoBehaviour
    {
        [SerializeField]
        private bool _IsUnlockAllLevelsfc;
        [SerializeField] 
        private RoadManager _roadManager;
        [SerializeField] 
        private SquadController _squadController;
        [SerializeField]
        private int _totalLevelsfc;
        [SerializeField]
        private Sprite selectedspritewp;
        [SerializeField]
        private ScrollRect MyScrollRectwp;
        [SerializeField]
        private GameObject _spawnScrollfc;
        [SerializeField]
        private GameObject _levelButtonPrefabfc;
        [SerializeField]
        private GameObject currentbuttonwp;

        private List<LevelButton> _allLevelButtons = new List<LevelButton>();
        private RectTransform MyScrollContentwp;
        private int _levelSelected;

        public int LevelSelected
        {
            get => _levelSelected;
            set => _levelSelected = value;
        }

        private void Awake()
        {
            MyScrollContentwp = _spawnScrollfc.GetComponent<RectTransform>();
        }

        private void Start()
        {
            //SoundManager.Instance.PlayButtonPressedSound();
            LevelSelected = PlayerPrefsManager.GetLevel();
            if (_IsUnlockAllLevelsfc == true)
            {
                LevelSelected = _totalLevelsfc;
            }
            print(LevelSelected);
            PlaceLevelsfc();
        }

        public void RefreshLevelsView()
        {
            LevelSelected = PlayerPrefsManager.GetLevel();
            for(int i = 0; i <= _allLevelButtons.Count; i++)
            {
                if( i <= LevelSelected)
                {
                    _allLevelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
                    if (i == LevelSelected)
                    {
                        _allLevelButtons[i].gameObject.GetComponent<Image>().sprite = selectedspritewp;
                        currentbuttonwp = _allLevelButtons[i].gameObject;
                    }
                }
            }
            SnapToCurrentOpenfc();
        }

        private void PlaceLevelsfc()
        {
            _allLevelButtons.Clear();
            for(int i = 1; i <= _totalLevelsfc; i++)
            {
                GameObject button = Instantiate(_levelButtonPrefabfc, _spawnScrollfc.transform);
                var buttonLevel = button.GetComponent<LevelButton>();
                buttonLevel.LevelNumber = i;
                button.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = i.ToString();
                buttonLevel.SelectLevel += SelectLevel;
                _allLevelButtons.Add(buttonLevel);
               
                // if( i <= LevelSelected)
                // {
                //     button.transform.GetChild(1).gameObject.SetActive(false);
                //     if (i == LevelSelected)
                //     {
                //         button.gameObject.GetComponent<Image>().sprite = selectedspritewp;
                //         currentbuttonwp = button;
                //     }
                // }
            }

            RefreshLevelsView();
            //SnapToCurrentOpenfc();
            //Invoke("SnapToCurrentOpenfc", 0.01f);
        }
        private void OnDestroy()
        {
            foreach (var button in _allLevelButtons)
            {
                button.SelectLevel -= SelectLevel;
            }
        }
        
        private void SnapToCurrentOpenfc()
        {
            Canvas.ForceUpdateCanvases();
            Vector3 contentPos = MyScrollContentwp.position;
            Vector3 buttonPos = currentbuttonwp.transform.position;
            MyScrollContentwp.anchoredPosition = MyScrollRectwp.transform.InverseTransformPoint(contentPos) 
                                                 - MyScrollRectwp.transform.InverseTransformPoint(buttonPos);
            MyScrollContentwp.anchoredPosition = new Vector2(0, MyScrollContentwp.anchoredPosition.y - 200f);
        }

        private void SelectLevel(int level)
        {
            foreach (var button in _allLevelButtons)
            {
                if (button.LevelNumber == level)
                {
                    button.Selected();
                }
                else
                {
                    button.Unselected();
                }
            }
            LevelSelected = level;
        }


        public void LoadLevel()
        {
            _roadManager.CreateLevel(LevelSelected);
            _squadController.CreateCharacter(LevelSelected);
        }
    }
}
