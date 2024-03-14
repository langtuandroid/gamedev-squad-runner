using System.Collections.Generic;
using System.Linq;
using JetSystems;
using UnityEngine;

namespace Gameplay
{
    public class SquadControllersr : MonoBehaviour
    {
        [Header(" Managers ")]
        [SerializeField] 
        private SquadFormation _squadFormationsr;
        [SerializeField] 
        private SquadDetection _squadDetectionsr;
        
        [Header(" Movement Settings")]
        [SerializeField] 
        private float _moveSpeedsr;
        [SerializeField] 
        private float _moveCoefficientsr;
        [SerializeField] 
        private float _platformWidthsr;
        
        [Header("All Character Models")]
        [SerializeField] private List<GameObject> _allrunnerPrefabssr;
    
        private GameObject _activeCharacterPrefabssr;
        private Vector3 _clickedPositionsr;
        private Vector3 _initialPositionsr;
        private int _indexHeroModelsr;

        private void Awake()
        {
            UIManager.onRetryButtonPressed += CreateCharacter;
            UIManager.onNextLevelButtonPressed += CreateCharacter;
            _initialPositionsr = transform.position;
            CreateCharacter(1);
        }

        private void OnDestroy()
        {
            UIManager.onRetryButtonPressed -= CreateCharacter;
            UIManager.onNextLevelButtonPressed -= CreateCharacter;
        }


        public void CreateCharacter(int level)
        {
            foreach (Transform child in _squadFormationsr.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
            transform.position = _initialPositionsr;
            SelectNewCharacter();
        }

        private void Update()
        {
            if (UIManager.IsGame())
                MoveForward();

            if (!UIManager.IsGame()) return;

            UpdateProgressBar();
        }

        public void SelectNewCharacter()
        {
            LoadHeroData();
            CreateHero();
        }
    
    
        private void LoadHeroData()
        {
            _indexHeroModelsr =  PlayerPrefsManager.GetSelectHeroModel();
        }

        public void CreateHero()
        {
            if (_activeCharacterPrefabssr != null)
            {
                DestroyImmediate(_activeCharacterPrefabssr);
            }

            _activeCharacterPrefabssr = Instantiate(_allrunnerPrefabssr[_indexHeroModelsr],
                _allrunnerPrefabssr[_indexHeroModelsr].transform.position,
                _allrunnerPrefabssr[_indexHeroModelsr].transform.rotation, _squadFormationsr.gameObject.transform);
            _squadFormationsr.SetCharacter(_allrunnerPrefabssr[_indexHeroModelsr]);
            _squadDetectionsr.SetCharacter(_activeCharacterPrefabssr);
        }

        public void StoreClickedPosition()
        {
            _clickedPositionsr = transform.position;
        }

        public void GetSlideValue(Vector2 slideInput)
        {
            slideInput.x *= _moveCoefficientsr;
            float targetX = _clickedPositionsr.x + slideInput.x;

            float maxX = _platformWidthsr / 2 - _squadFormationsr.GetSquadRadiussr();

            targetX = Mathf.Clamp(targetX, -maxX, maxX);

            transform.position = transform.position.With(x: Mathf.Lerp(transform.position.x, targetX, 0.3f));
        }

        private void MoveForward()
        {
            transform.position += Vector3.forward * _moveSpeedsr * Time.deltaTime;
        }

        private void UpdateProgressBar()
        {
            float initialDistanceToFinish = RoadManager.GetFinishPosition().z - _initialPositionsr.z;
            float currentDistanceToFinish = RoadManager.GetFinishPosition().z - transform.position.z;
            float distanceLeftToFinish = initialDistanceToFinish - currentDistanceToFinish;

            float progress = distanceLeftToFinish / initialDistanceToFinish;
            UIManager.updateProgressBarDelegate?.Invoke(progress);
        }
        
        private int CalculateArraySumsr(int[] array)
        {
            return array.Sum();
        }
    }
}
