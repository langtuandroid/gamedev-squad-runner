﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace JetSystems
{
    public class RoadManager : MonoBehaviour
    {
        [Header(" Debug ")]
        public bool DEBUG;
        public int levelToPlay;
        private int _presentMaxOpenLevel;

        [FormerlySerializedAs("initialChunk")] [Header(" Road Chunks ")]
        public RoadChunksr initialChunksr;
        [FormerlySerializedAs("finishChunk")] public RoadChunksr finishChunksr;
        private RoadChunksr _previousChunksr;
        Vector3 finishPos;
        Vector3 spawnPos;

        [Header(" Predefined Levels ")]
        public LevelSequence[] levelSequences;

        List<RoadChunksr> levelChunks = new List<RoadChunksr>();

        static RoadManager instance;
        private int _presentlevel;
        private void Awake()
        {
            _presentMaxOpenLevel = PlayerPrefsManager.GetLevel();
        }

        // Start is called before the first frame update
        void Start()
        {
            instance = this;

            UIManager.LevelComplete += IncreaseLevelIndex;
            UIManager.onNextLevelButtonPressed += CreateLevel;
            UIManager.onRetryButtonPressed += RetryLevel;

            //SpawnLevel();
        }

        private void OnDestroy()
        {
            UIManager.LevelComplete -= IncreaseLevelIndex;
            UIManager.onNextLevelButtonPressed -= CreateLevel;
            UIManager.onRetryButtonPressed -= RetryLevel;
        }
        private void IncreaseLevelIndex()
        {
            Debug.Log("_presentlevel= " +_presentlevel);
            if (_presentlevel == PlayerPrefsManager.GetLevel())
            {
                Debug.Log("_presentMaxOpenLevel= " +_presentMaxOpenLevel);
                
                PlayerPrefsManager.SaveLevel(PlayerPrefsManager.GetLevel()+1);
                _presentMaxOpenLevel = PlayerPrefsManager.GetLevel();
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

            if (DEBUG)
                currentLevel = levelToPlay;

            SpawnLevelSequence(currentLevel);
            // if (currentLevel >= levelSequences.Length) //For Random level
            // {
            //     SpawnLevelSequence(Random.Range(0, levelSequences.Length));
            // }
            // else
            // {
            //     SpawnLevelSequence(currentLevel);
            // }

        }

        private void SpawnLevelSequence(int currentLevel)
        {
            for (int i = 0; i < levelSequences[currentLevel].chunks.Length; i++)
            {
                RoadChunksr chunksrToSpawn = levelSequences[currentLevel].chunks[i];
                Instantiate(chunksrToSpawn, spawnPos, Quaternion.identity, transform);

                spawnPos.z += chunksrToSpawn.Lengthsr;
                _previousChunksr = chunksrToSpawn;
                levelChunks.Add(chunksrToSpawn);
            }

            // We can then spawn the finish chunk
            Instantiate(finishChunksr, spawnPos, Quaternion.identity, transform);

            levelChunks.Add(finishChunksr);

            // Store the finish pos for progression use
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

        public Vector3 GetFinishLinePosition()
        {
            return finishPos;
        }

        public void RetryLevel(int level)
        {
            ClearLevel();
            spawnPos = Vector3.zero;

            for (int i = 0; i < levelChunks.Count; i++)
            {
                RoadChunksr spawnedChunksr = Instantiate(levelChunks[i], spawnPos, Quaternion.identity, transform);
                spawnPos.z += levelChunks[i].Lengthsr;
            }
        }

        public float GetFinishLineZ()
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