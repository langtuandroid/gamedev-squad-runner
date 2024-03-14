using System.Collections.Generic;
using Gameplay;
using JetSystems;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LevelSelectManagersr : MonoBehaviour
    {
        [SerializeField]
        private bool _isUnlockAllLevelssr;
        [SerializeField]
        private int _totalLevelssr;
        [SerializeField]
        private ScrollRect _scrollRectsr;
        [SerializeField]
        private GameObject _contentsr;
        [SerializeField]
        private GameObject _levelButtonPrefabsr;
        
        private List<LevelButtonsr> _allLevelButtonssr = new List<LevelButtonsr>();
        private RectTransform _scrollContensr;
        private GameObject _currentbuttonsr;
        private int _levelSelectedsr;

        public int LevelSelectedsr
        {
            get => _levelSelectedsr;
            set => _levelSelectedsr = value;
        }
        
        private RoadManager _roadManager;
        private SquadControllersr _squadControllersr;

        [Inject]
        private void Context(RoadManager roadManager, SquadControllersr squadControllersr)
        {
            _roadManager = roadManager;
            _squadControllersr = squadControllersr;
        }
        
        private void Awake()
        {
            _scrollContensr = _contentsr.GetComponent<RectTransform>();
        }

        private void Start()
        {
            LevelSelectedsr = PlayerPrefsManager.GetLevel();
            if (_isUnlockAllLevelssr)
            {
                LevelSelectedsr = _totalLevelssr;
            }
            PlaceLevelsr();
        }
        
        private void OnDestroy()
        {
            foreach (var button in _allLevelButtonssr)
            {
                button.SelectLevelsr -= SelectLevelsr;
            }
        }

        public void OpeClosenLevelsr()
        {
            LevelSelectedsr = PlayerPrefsManager.GetLevel();
            _isUnlockAllLevelssr = !_isUnlockAllLevelssr;
            if (_isUnlockAllLevelssr)
            {
                LevelSelectedsr = _totalLevelssr;
            }
            for(int i = 1; i <= _allLevelButtonssr.Count; i++)
            {
                if( i <= LevelSelectedsr-1)
                {
                    _allLevelButtonssr[i].transform.GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    _allLevelButtonssr[i-1].transform.GetChild(1).gameObject.SetActive(true);
                }
            }
            SelectLevelsr(LevelSelectedsr);
            SnapToCurrentOpensr();
        }

        public void RefreshLevelsViewsr()
        {
            if (!_isUnlockAllLevelssr)
            {
                LevelSelectedsr = PlayerPrefsManager.GetLevel();
            }
            for(int i = 0; i <= _allLevelButtonssr.Count; i++)
            {
                if( i <= LevelSelectedsr-1)
                {
                    _allLevelButtonssr[i].transform.GetChild(1).gameObject.SetActive(false);
                }
            }
            SelectLevelsr(LevelSelectedsr);
            SnapToCurrentOpensr();
        }

        private void PlaceLevelsr()
        {
            _allLevelButtonssr.Clear();
            for(int i = 1; i <= _totalLevelssr; i++)
            {
                GameObject button = Instantiate(_levelButtonPrefabsr, _contentsr.transform);
                var buttonLevel = button.GetComponent<LevelButtonsr>();
                buttonLevel.LevelNumbersr = i;
                button.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = i.ToString();
                buttonLevel.SelectLevelsr += SelectLevelsr;
                _allLevelButtonssr.Add(buttonLevel);
            }

            RefreshLevelsViewsr();
        }
       
        private void SnapToCurrentOpensr()
        {
            Canvas.ForceUpdateCanvases();
            Vector3 contentPos = _scrollContensr.position;
            Vector3 buttonPos = _currentbuttonsr.transform.position;
            _scrollContensr.anchoredPosition = _scrollRectsr.transform.InverseTransformPoint(contentPos) 
                                                 - _scrollRectsr.transform.InverseTransformPoint(buttonPos);
            _scrollContensr.anchoredPosition = new Vector2(0, _scrollContensr.anchoredPosition.y - 200f);
        }

        private void SelectLevelsr(int level)
        {
            foreach (var button in _allLevelButtonssr)
            {
                if (button.LevelNumbersr == level)
                {
                    button.Selected();
                    _currentbuttonsr = button.gameObject;
                }
                else
                {
                    button.Unselected();
                }
            }
            LevelSelectedsr = level;
        }
        
        public void LoadLevelsr()
        {
            _roadManager.CreateLevel(LevelSelectedsr);
            _squadControllersr.CreateCharacter(LevelSelectedsr);
        }
    }
}
