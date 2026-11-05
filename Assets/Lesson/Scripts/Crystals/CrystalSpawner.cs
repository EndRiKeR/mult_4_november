using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace NetLesson
{
    public class CrystalSpawner : NetworkBehaviour
    {
        [SerializeField]
        private NetworkObject _crystalPrefab;

        [SerializeField]
        private float _spawnInterval = 2f;

        [SerializeField]
        private int _maxCrystals = 5;

        [SerializeField]
        private float _arenaHalfSize = 8f;

        private readonly List<NetworkObject> _crystals = new();
        private float _timer;

        private void Update()
        {
            if (!IsServer || !IsSpawned)
                return;

            _crystals.RemoveAll(crystal => crystal == null || !crystal.IsSpawned);

            _timer += Time.deltaTime;
            if (_timer < _spawnInterval || _crystals.Count >= _maxCrystals)
                return;

            _timer = 0f;
            SpawnCrystal();
        }

        private void SpawnCrystal()
        {
            var position = new Vector3(
                Random.Range(-_arenaHalfSize, _arenaHalfSize),
                0.5f,
                Random.Range(-_arenaHalfSize, _arenaHalfSize)
            );

            var crystal = NetworkManager.SpawnManager.InstantiateAndSpawn(_crystalPrefab, position: position);
            _crystals.Add(crystal);
        }
    }
}
