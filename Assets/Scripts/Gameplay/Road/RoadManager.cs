using System.Collections.Generic;
using JetSystems;
using UnityEngine;

namespace Gameplay.Road
{
    public class RoadManager : MonoBehaviour
    {
        [Header("Roads")]
        public RoadChunksr initialChunksr;
        public RoadChunksr finishChunksr;
       
        private Vector3 spawnPos;
        private Vector3 finishPos;
        public LevelSequence[] levelSequences;

        private List<RoadChunksr> levelChunks = new List<RoadChunksr>();
        private int _presentlevel;

        static RoadManager instance;
        private void Start()
        {
            instance = this;
            UIManager.LevelComplete += IncreaseLevelIndex;
            UIManager.onNextLevelButtonPressed += CreateLevel;
            UIManager.onRetryButtonPressed += RetryLevel;
        }

        private void OnDestroy()
        {
            UIManager.LevelComplete -= IncreaseLevelIndex;
            UIManager.onNextLevelButtonPressed -= CreateLevel;
            UIManager.onRetryButtonPressed -= RetryLevel;
        }
        private void IncreaseLevelIndex()
        {
            if (_presentlevel == PlayerPrefsManager.GetLevel())
            {
                PlayerPrefsManager.SaveLevel(PlayerPrefsManager.GetLevel()+1);
            }
        }

        public void CreateLevel(int levelSelected)
        {
            _presentlevel = levelSelected;
            // Delete the children
            ClearLevel();

            levelChunks.Clear();

            spawnPos = Vector3.zero;

            int currentLevel = levelSelected;

            SpawnLevelSequence(currentLevel);
        }

        private void SpawnLevelSequence(int currentLevel)
        {
            Instantiate(initialChunksr, spawnPos, Quaternion.identity, transform);
            levelChunks.Add(initialChunksr);
            spawnPos.z += initialChunksr.Lengthsr;

            for (int i = 0; i < levelSequences[currentLevel].chunks.Length; i++)
            {
                RoadChunksr chunksrToSpawn = levelSequences[currentLevel].chunks[i];
                Instantiate(chunksrToSpawn, spawnPos, Quaternion.identity, transform);

                spawnPos.z += chunksrToSpawn.Lengthsr;
                levelChunks.Add(chunksrToSpawn);
            }
            
            Instantiate(finishChunksr, spawnPos, Quaternion.identity, transform);
            levelChunks.Add(finishChunksr);
            finishPos = spawnPos;
        }


        private void ClearLevel()
        {
            while (transform.childCount > 0)
            {
                Transform t = transform.GetChild(0);
                t.SetParent(null);
                Destroy(t.gameObject);
            }
        }

        private Vector3 GetFinishLinePosition()
        {
            return finishPos;
        }

        private void RetryLevel(int level)
        {
            ClearLevel();
            spawnPos = Vector3.zero;

            for (int i = 0; i < levelChunks.Count; i++)
            {
                RoadChunksr spawnedChunksr = Instantiate(levelChunks[i], spawnPos, Quaternion.identity, transform);
                spawnPos.z += levelChunks[i].Lengthsr;
            }
        }

        private float GetFinishLineZ()
        {
            return finishPos.z;
        }

        public static Vector3 GetFinishPosition()
        {
            return instance.GetFinishLinePosition();
        }
    }

    [System.Serializable]
    public struct LevelSequence
    {
        public RoadChunksr[] chunks;
    }
}