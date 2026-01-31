using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _enemyPrefabs;

    [SerializeField]
    private int _timeBetweenSpawns = 5;
    private float _timeSinceLastSpawn = 0f;

    [SerializeField]
    private Transform _playerTransform;
    [SerializeField]
    private float _enemySpawnDistanceMaxX;
    [SerializeField]
    private float _enemySpawnDistanceMaxY;
    [SerializeField]
    private float _enemySpawnDistanceMinX;
    [SerializeField]
    private float _enemySpawnDistanceMinY;

    void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        if(_timeSinceLastSpawn > _timeBetweenSpawns)
        { 
            _timeSinceLastSpawn -= _timeBetweenSpawns;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (_playerTransform == null) { return; }
        int enemyPrefabIdx = UnityEngine.Random.Range(0, _enemyPrefabs.Count);
        GameObject enemy = GameObject.Instantiate(_enemyPrefabs[enemyPrefabIdx]);
        EnemyMovement enemyMovement = (EnemyMovement) enemy.GetComponent("EnemyMovement");
        enemyMovement.PlayerTransform = _playerTransform;
        bool invertedX = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        bool invertedY = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        int x;
        int y;
        if (invertedX) { x = -1; }
        else { x = 1; }
        if (invertedY) { y = -1; }
        else { y = 1; }

            //enemyMovement.transform = _playerTransform + new Vector3(x * _enemySpawnDistanceMinX);
    }
}
