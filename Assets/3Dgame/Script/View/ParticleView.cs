using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{

    public class ParticleView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _enemyParticlePrefab;

        [SerializeField]
        private GameObject _playerParticlePrefab;

        public GameObject EnemyParticlePrefab => _enemyParticlePrefab;
        public GameObject PlayerParticlePrefab => _playerParticlePrefab;
    }
}
