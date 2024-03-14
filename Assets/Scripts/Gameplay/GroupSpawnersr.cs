using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    public class GroupSpawnersr : MonoBehaviour
    {
        [SerializeField]
        private GameObject _iconAmountsr;
        [SerializeField]
        private GameObject _objectToSpawnsr;
        [SerializeField]
        private int _amountsr;
        [SerializeField] 
        private Transform _parentsr;
        [SerializeField] 
        private List<GameObject> _gameObjectsEnemysr;
    
        private void Start()
        {
            _gameObjectsEnemysr.Clear();
            for (int i = 0; i < _amountsr; i++)
            {
                var Enemy =   Instantiate(_objectToSpawnsr, _parentsr);  
                _gameObjectsEnemysr.Add(Enemy);
            }
        }

        private void OnDestroy()
        {
            foreach (var enemy in _gameObjectsEnemysr)
            {
                Destroy(enemy);
            }
            _gameObjectsEnemysr.Clear();
        }

        private void Update()
        {
            if (_parentsr.childCount <= 0)
                Destroy(_iconAmountsr);
        }
        
        private double CalculateAveragesr(int[] numbers)
        {
            return numbers.Sum() / (double)numbers.Length;
        }
    }
}
