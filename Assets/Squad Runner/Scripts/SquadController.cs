using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetSystems;

public class SquadController : MonoBehaviour
{
    [Header(" Managers ")]
    [SerializeField] private SquadFormation _squadFormation;
    [SerializeField] private SquadDetection _squadDetection;
    
    [SerializeField] private GameObject _activeCharacterPrefabs;
    
    [Header("All Character Models")]
    [SerializeField] private List<GameObject> _allrunnerPrefabs;

    [Header(" Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveCoefficient;
    [SerializeField] private float platformWidth;
    
    private Vector3 clickedPosition;
    private Vector3 initialPosition;
    private int _indexHeroModel;

    private void Awake()
    {
        UIManager.onRetryButtonPressed += CreateCharacter;
        UIManager.onNextLevelButtonPressed += CreateCharacter;
        initialPosition = transform.position;
        CreateCharacter(1);
    }

    private void OnDestroy()
    {
        UIManager.onRetryButtonPressed -= CreateCharacter;
        UIManager.onNextLevelButtonPressed -= CreateCharacter;
    }


    public void CreateCharacter(int level)
    {
        foreach (Transform child in _squadFormation.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        transform.position = initialPosition;
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
        _indexHeroModel =  PlayerPrefsManager.GetSelectHeroModel();
    }

    public void CreateHero()
    {
        if (_activeCharacterPrefabs != null)
        {
            DestroyImmediate(_activeCharacterPrefabs);
        }

        _activeCharacterPrefabs = Instantiate(_allrunnerPrefabs[_indexHeroModel],
            _allrunnerPrefabs[_indexHeroModel].transform.position,
            _allrunnerPrefabs[_indexHeroModel].transform.rotation, _squadFormation.gameObject.transform);
        _squadFormation.SetCharacter(_allrunnerPrefabs[_indexHeroModel]);
        _squadDetection.SetCharacter(_activeCharacterPrefabs);
    }

    public void StoreClickedPosition()
    {
        clickedPosition = transform.position;
    }

    public void GetSlideValue(Vector2 slideInput)
    {
        slideInput.x *= moveCoefficient;
        float targetX = clickedPosition.x + slideInput.x;

        float maxX = platformWidth / 2 - _squadFormation.GetSquadRadius();

        targetX = Mathf.Clamp(targetX, -maxX, maxX);

        transform.position = transform.position.With(x: Mathf.Lerp(transform.position.x, targetX, 0.3f));
    }

    private void MoveForward()
    {
        transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
    }

    private void UpdateProgressBar()
    {
        float initialDistanceToFinish = RoadManager.GetFinishPosition().z - initialPosition.z;
        float currentDistanceToFinish = RoadManager.GetFinishPosition().z - transform.position.z;
        float distanceLeftToFinish = initialDistanceToFinish - currentDistanceToFinish;

        float progress = distanceLeftToFinish / initialDistanceToFinish;
        UIManager.updateProgressBarDelegate?.Invoke(progress);
    }
}
