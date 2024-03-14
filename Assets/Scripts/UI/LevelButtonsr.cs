using System;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButtonsr : MonoBehaviour
    {
        public Action<int> SelectLevelsr;
        
        [SerializeField] 
        private int _levelNumbersr;
        [SerializeField] 
        private Sprite _selectedSpritesr;
        [SerializeField] 
        private Sprite _unSelectedSpritesr;

        private Button _buttonsr;
        public int LevelNumbersr
        {
            get => _levelNumbersr;
            set => _levelNumbersr = value;
        }

        private void Awake()
        {
            _buttonsr = gameObject.GetComponent<Button>();
            _buttonsr.onClick.AddListener(SelectLevelfc);
        }

        private void OnDestroy()
        {
            _buttonsr.onClick.RemoveListener(SelectLevelfc);
        }

        private void SelectLevelfc()
        {
            AudioManagersr.Instancesr.PlaySFXOneShotsr(0);
            if (gameObject.transform.GetChild(1).gameObject.activeInHierarchy) return;
            SelectLevelsr.Invoke(LevelNumbersr);
        }

        public void Selected()
        {
            gameObject.GetComponent<Image>().sprite = _selectedSpritesr;
        }
        public void Unselected()
        {
            gameObject.GetComponent<Image>().sprite = _unSelectedSpritesr;
        }
    }
}
